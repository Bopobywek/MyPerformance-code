using CommunityToolkit.Mvvm.Input;
using MyPerformance.Models;
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
    internal class MainViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand AddCommand { get; set; }
        public ObservableCollection<PerformanceModel> Performances { get; } = new();

        public MainViewModel() {
            AddCommand = new AsyncRelayCommand(async () =>
            {
                // await Application.Current.MainPage.Navigation.PushAsync(new PerformancePage());
                await Shell.Current.GoToAsync(nameof(PerformancePage));
            });
        }
    }
}
