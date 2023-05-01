using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPerformance.Models;
using MyPerformance.Repositories;
using MyPerformance.Views;
using System.Collections.ObjectModel;

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
        public void Delete(PerformanceModel model)
        {
            performancesRepository.Delete(model.Id);
            Performances.Remove(model);
        }

        [RelayCommand]
        public async void Run(PerformanceModel model)
        {
            var parameters = new Dictionary<string, object> {
                { "timer", performancesRepository.Query(model.Id) }
            };

            await Shell.Current.GoToAsync(nameof(TimerPage), parameters);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("upd-main"))
            {
                UpdatePerformances();
                query.Clear();
            }
        }


    }
}
