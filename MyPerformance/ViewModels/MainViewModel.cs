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
    public partial class MainViewModel : IQueryAttributable
    {
        private readonly PerformancesRepository performancesRepository;

        public ObservableCollection<PerformanceModel> Performances { get; } = new();

        public MainViewModel(PerformancesRepository performancesRepository)
        {
            this.performancesRepository = performancesRepository;
            UpdatePerformances();
        }
        private void UpdatePerformances()
        {
            Performances.Clear();
            var result = performancesRepository.QueryAll();

            foreach (var item in result)
            {
                Performances.Add(item);
            }

            OnPropertyChanged(nameof(Performances));
        }

        [RelayCommand]
        public async void Add()
        {
            await Shell.Current.GoToAsync(nameof(PerformancePage));
        }

        [RelayCommand]
        public async void Edit(int id) 
        {
            var parameters = new Dictionary<string, object> {
                { "edit", true },
                { "id", id}
            };
            await Shell.Current.GoToAsync(nameof(PerformancePage), parameters);
        }

        [RelayCommand]
        public async void Delete(int id)
        {
            performancesRepository.Delete(id);
            UpdatePerformances();
        }

        [RelayCommand]
        public async void Run(int id)
        {
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            UpdatePerformances();
        }


    }
}
