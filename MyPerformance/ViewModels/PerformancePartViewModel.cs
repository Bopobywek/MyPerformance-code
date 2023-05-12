using MyPerformance.Models;
using MyPerformance.Services.Interfaces;
using MyPerformance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyPerformance.ViewModels
{
    [ObservableObject]
    public partial class PerformancePartViewModel : IQueryAttributable
    {
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private int id;
        private int performanceId;

        public ValidatableObject<string> Name { get; private set; } = new();

        [ObservableProperty]
        private string description;

        public ValidatableObject<TimeSpan> Time { get; private set; } = new();
        private readonly IAlertService alertService;

        public ICommand SelectColor { get; }

        public PerformancePartViewModel(IPopupService popupService, IAlertService alertService)
        {
            Color = Color.Parse("#A6E1FA");
            SelectColor = new Command(async () =>
            {
                var popup = new ColorPopup();
                Color = (Color)await popupService.ShowPopup(popup);
            });

            Name.Validations.Add(new LengthRule<string>()
            {
                ValidationMessage = "Название не должно быть пустым"
            });

            Time.Validations.Add(new NonZeroTimeSpanRule
            {
                ValidationMessage = "Длительность части выступления не может быть нулевой"
            });
            this.alertService = alertService;
        }

        [RelayCommand]
        public void ValidateName()
        {
            Name.Validate();
        }

        [RelayCommand]
        public async void Save()
        {
            var isValid = Name.Validate() && Time.Validate();

            if (!isValid) 
            {
                var errors = Name.Errors.Union(Time.Errors);
                await alertService.ShowAlertAsync("Ошибка", string.Join("\n", errors));
                return;
            }

            var model = new PerformancePartModel
            {
                Id = id,
                PerformanceId = performanceId,
                Name = Name.Value,
                Description = Description,
                Duration = Time.Value,
                Color = Color?.ToHex() ?? "#FEFFFF"
            };
            var parameters = new Dictionary<string, object>
            {
                { "add", model }
            };

            await Shell.Current.GoToAsync("..", true, parameters);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("edit-part"))
            {
                var model = (PerformancePartModel)query["edit-part"];
                id = model.Id;
                Name.Value = model.Name;
                Description = model.Description;
                Time.Value = model.Duration;
                Color = Color.Parse(model.Color);
                performanceId = model.PerformanceId;
                query.Clear();
            }
        }
    }
}
