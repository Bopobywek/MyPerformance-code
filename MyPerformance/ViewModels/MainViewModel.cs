using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPerformance.Models;
using MyPerformance.Repositories;
using MyPerformance.Services.Interfaces;
using MyPerformance.Views;
using System.Collections.ObjectModel;

namespace MyPerformance.ViewModels
{
    [ObservableObject]
    public partial class MainViewModel : IQueryAttributable
    {
        private readonly PerformancesRepository performancesRepository;
        private readonly IAlertService alertService;

        public ObservableCollection<PerformanceModel> Performances { get; } = new();
        [ObservableProperty]
        private bool isRefreshing;

        public MainViewModel(PerformancesRepository performancesRepository, IAlertService alertService)
        {
            this.performancesRepository = performancesRepository;
            this.alertService = alertService;
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
        public async Task Delete(PerformanceModel model)
        {
            var result = await alertService.ShowConfirmationAsync("Удаление выступления",
                "Вы действительно хотите удалить данное выступление?", "Да", "Нет");
            if (!result)
            {
                return;
            }
            performancesRepository.Delete(model.Id);
            MainThread.BeginInvokeOnMainThread(() => {
                Performances.Remove(model);
            });
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

        [RelayCommand]
        public void Refresh()
        {
            IsRefreshing = true;
            UpdatePerformances();
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task ClearStatistics(PerformanceModel performance)
        {
            var result = await alertService.ShowConfirmationAsync("Очистить историю?",
                "Вы действительно хотите удалить историю для данного выступления?", "Да", "Нет");
            if (!result)
            {
                return;
            }

            var newPerformance = performancesRepository.Query(performance.Id);

            newPerformance.NumberOfLaunches = 0;
            newPerformance.TotalDelayTime = 0;
            newPerformance.TotalDuration = 0;

            performancesRepository.AddOrUpdate(newPerformance);
            MainThread.BeginInvokeOnMainThread(() => {
                UpdatePerformances();
            });
        }
    }
}
