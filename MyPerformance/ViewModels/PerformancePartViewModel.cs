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
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string description;

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectColor { get; }

        public PerformancePartViewModel(IPopupService popupService)
        {
            SelectColor = new Command(async () =>
            {
                var popup = new ColorPopup();
                Color = (Color)await popupService.ShowPopup(popup);
            });
        }

        [RelayCommand]
        public async void Save()
        {
            var model = new PerformancePartModel
            {
                Id = id,
                PerformanceId = performanceId,
                Name = Name,
                Description = Description,
                Duration = Time,
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
                Name = model.Name;
                Description = model.Description;
                Time = model.Duration;
                Color = Color.Parse(model.Color);
                performanceId = model.PerformanceId;
            }

            query.Clear();
        }
    }
}
