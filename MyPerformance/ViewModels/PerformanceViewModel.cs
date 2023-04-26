using MyPerformance.Models;
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
    internal class PerformanceViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand AddCommand { get; set; }
        public ObservableCollection<PerformancePartModel> PerformanceParts { get; } = new();

        public PerformanceViewModel()
        {
            AddCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PerformancePartPage());
                // await Shell.Current.GoToAsync(nameof(PerformancePage));
            });
        }
    }
}
