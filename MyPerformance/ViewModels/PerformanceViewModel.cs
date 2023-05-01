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

namespace MyPerformance.ViewModels
{
    [ObservableObject]
    public partial class PerformanceViewModel : IQueryAttributable
    {
        private readonly PerformancesRepository repository;
        [ObservableProperty]
        private string name;
        private int id;
        public ICommand AddCommand { get; set; }
        public ObservableCollection<PerformancePartModel> PerformanceParts { get; } = new();

        public PerformanceViewModel(PerformancesRepository repository)
        {
            AddCommand = new Command(async () =>
            {
                // await Application.Current.MainPage.Navigation.PushAsync(new PerformancePartPage());
                await Shell.Current.GoToAsync(nameof(PerformancePartPage));
            });
            this.repository = repository;
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
                name = model.Name;
                foreach (var part in model.PerformanceParts.OrderBy(x => x.Position)) 
                {
                    PerformanceParts.Add(part);
                }
                OnPropertyChanged(nameof(PerformanceParts));
                OnPropertyChanged("Name");
                query.Clear();
            }
            query.Clear();
        }

        [RelayCommand]
        public async void Save()
        {
            for (int i = 1; i < PerformanceParts.Count + 1; ++i)
            {
                PerformanceParts[i - 1].Position = i;
            }

            var model = new PerformanceModel {
                Id = id,
                Name = Name,
                Date = new DateTime(2023, 5, 1),
                Duration = new TimeSpan(PerformanceParts.Sum(x => x.Duration.Ticks)),
                PerformanceParts = PerformanceParts.ToArray()
            };

            repository.AddOrUpdate(model);
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
        public void Delete(PerformancePartModel model)
        {
            PerformanceParts.Remove(model);
            repository.DeletePart(model);
            OnPropertyChanged(nameof(PerformanceParts));
        }
    }
}
