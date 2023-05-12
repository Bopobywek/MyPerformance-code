using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPerformance.Models;
using MyPerformance.Repositories;
using MyPerformance.Services.Interfaces;
using MyPerformance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#if ANDROID
using MyPerformance.Platforms.Android;
#endif

namespace MyPerformance.ViewModels
{
    [ObservableObject]
    public partial class PerformanceViewModel : IQueryAttributable
    {
        private readonly PerformancesRepository repository;
        private readonly IAlertService alertService;
        public ValidatableObject<string> Name { get; private set; } = new();

        [ObservableProperty]
        private bool isVibrationEnable;

        [ObservableProperty]
        private bool isNotificationEnable;

        [ObservableProperty]
        private bool isDateEnable;

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        [ObservableProperty]
        private TimeSpan time;

        private int id;
        private PerformancePartModel draggedItem;
        public ICommand AddCommand { get; set; }
        public ObservableCollection<PerformancePartModel> PerformanceParts { get; } = new();
        private List<PerformancePartModel> removedParts = new();

        public PerformanceViewModel(PerformancesRepository repository, IAlertService alertService)
        {
            AddCommand = new Command(async () =>
            {
                // await Application.Current.MainPage.Navigation.PushAsync(new PerformancePartPage());
                await Shell.Current.GoToAsync(nameof(PerformancePartPage));
            });
            this.repository = repository;
            this.alertService = alertService;

            Name.Validations.Add(new LengthRule<string>()
            {
                ValidationMessage = "Название не должно быть пустым"
            });
        }

        [RelayCommand]
        public void ItemDragged(PerformancePartModel performancePart)
        {
            performancePart.IsBeingDragged = true;
            draggedItem = performancePart;
        }

        [RelayCommand]
        public void ItemDragLeave(PerformancePartModel performancePart)
        {
            performancePart.IsBeingDraggedOver = false;
        }

        [RelayCommand]
        public void ItemDraggedOver(PerformancePartModel performancePart)
        {
            if (performancePart == draggedItem)
            {
                performancePart.IsBeingDragged = false;
            }
            performancePart.IsBeingDraggedOver = performancePart != draggedItem;
        }

        [RelayCommand]
        public void ItemDropped(PerformancePartModel performancePart)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var itemToMove = draggedItem;
                var itemToInsertBefore = performancePart;
                if (itemToMove == null || itemToInsertBefore == null || itemToMove == itemToInsertBefore)
                    return;

                int insertAtIndex = PerformanceParts.IndexOf(itemToInsertBefore);
                if (insertAtIndex >= 0 && insertAtIndex < PerformanceParts.Count)
                {
                    PerformanceParts.Remove(itemToMove);
                    PerformanceParts.Insert(insertAtIndex, itemToMove);
                    itemToMove.IsBeingDragged = false;
                    itemToInsertBefore.IsBeingDraggedOver = false;
                }
            });
        }

        [RelayCommand]
        public void SetNotificationToCalendar()
        {
#if ANDROID
            var beginTime = new DateTime(Date.Ticks + Time.Ticks);
            var calendarEvent = new CalendarEvent
            {
                Title = Name.Value,
                BeginTime = beginTime,
                IsEndTimeSet = true,
                EndTime = beginTime.AddTicks(PerformanceParts.Sum(x => x.Duration.Ticks))
            };

            CalendarEventService.SetEvent(calendarEvent);
#endif
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("add"))
            {
                var model = (PerformancePartModel)query["add"];
                if (model.Id != 0)
                {
                    for (int i = 0; i < PerformanceParts.Count; ++i)
                    {
                        if (model.Id == PerformanceParts[i].Id)
                        {
                            PerformanceParts[i] = model;
                            break;
                        }
                    }
                }
                else
                {
                    PerformanceParts.Add(model);
                }
                OnPropertyChanged(nameof(PerformanceParts));
                query.Clear();
            }
            else if (query.ContainsKey("edit"))
            {
                id = (int)query["id"];
                var model = repository.Query(id);
                Name.Value = model.Name;
                foreach (var part in model.PerformanceParts.OrderBy(x => x.Position))
                {
                    PerformanceParts.Add(part);
                }
                IsDateEnable = model.IsDateSet;
                if (model.IsDateSet)
                {
                    Date = model.Date;
                }
                else
                {
                    Date = DateTime.Now;
                }
                Time = new TimeSpan(model.Date.Hour, model.Date.Minute, model.Date.Second);
                IsNotificationEnable = model.IsNotificationEnable;
                IsVibrationEnable = model.IsVibrationEnable;
                OnPropertyChanged(nameof(PerformanceParts));
                query.Clear();
            }
        }

        [RelayCommand]
        public async void Save()
        {
            if (!Name.Validate())
            {
                await alertService.ShowAlertAsync("Ошибка", string.Join("\n", Name.Errors));
                return;
            }

            if (PerformanceParts.Count == 0)
            {
                await alertService.ShowAlertAsync("Ошибка", string.Join("\n", "Выступление должно содержать хотя бы одну часть"));
                return;
            }

            for (int i = 1; i < PerformanceParts.Count + 1; ++i)
            {
                PerformanceParts[i - 1].Position = i;
            }

            var model = new PerformanceModel
            {
                Id = id,
                Name = Name.Value,
                IsDateSet = IsDateEnable,
                Date = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds),
                Duration = new TimeSpan(PerformanceParts.Sum(x => x.Duration.Ticks)),
                IsVibrationEnable = IsVibrationEnable,
                IsNotificationEnable = IsNotificationEnable,
                PerformanceParts = PerformanceParts.ToArray()
            };

            repository.AddOrUpdate(model);

            foreach (var removedPart in removedParts)
            {
                repository.DeletePart(removedPart);
            }

            await Shell.Current.GoToAsync("..", new Dictionary<string, object>() {
                { "upd-main", 1 }
            });
        }

        [RelayCommand]
        public async void Edit(PerformancePartModel model)
        {
            var parameters = new Dictionary<string, object>
            {
                { "edit-part", model}
            };
            await Shell.Current.GoToAsync(nameof(PerformancePartPage), parameters);
        }

        [RelayCommand]
        public async Task Delete(PerformancePartModel model)
        {
            var result = await alertService.ShowConfirmationAsync("Удаление части выступления",
                "Вы действительно хотите удалить данную часть выступления?", "Да", "Нет");
            if (!result)
            {
                return;
            }
            PerformanceParts.Remove(model);
            removedParts.Add(model);
            OnPropertyChanged(nameof(PerformanceParts));
        }
    }
}
