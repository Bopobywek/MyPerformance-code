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
                PerformanceParts.Add(model);
                OnPropertyChanged(nameof(PerformanceParts));
            }
            else if (query.ContainsKey("edit"))
            {
                var id = (int)query["edit"];
                var model = repository.Query(id);
                foreach (var part in model.PerformanceParts) 
                {
                    PerformanceParts.Add(part);
                }
                OnPropertyChanged(nameof(PerformanceParts));
            }
            query.Clear();
        }

        [RelayCommand]
        public async void Save()
        {
            var model = new PerformanceModel {
                Name = Name,
                Date = new DateTime(2023, 5, 1),
                Duration = new TimeSpan(PerformanceParts.Sum(x => x.Duration.Ticks)),
                PerformanceParts = PerformanceParts.ToArray()
            };

            repository.Add(model);
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>() {
                { "upd-main", 1 }
            });
        }
    }
}
