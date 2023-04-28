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
    public class PerformancePartViewModel : ObservableObject
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
    }
}
