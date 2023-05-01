using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPerformance.Models;

namespace MyPerformance.ViewModels
{
    [ObservableObject]
    public partial class TimerViewModel : IQueryAttributable
    {
        [ObservableProperty]
        private TimeSpan time;

        [ObservableProperty]
        private TimeSpan partTime;

        [ObservableProperty]
        private string partName;

        [ObservableProperty]
        private string partNote;

        [ObservableProperty]
        private Color color;

        private int position = 0;
        private PerformanceModel performance;

        IDispatcherTimer timer;

        public TimerViewModel()
        {
            timer = Dispatcher.GetForCurrentThread().CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (s, e) =>
            {
                Time -= TimeSpan.FromSeconds(1);

                if (PartTime == TimeSpan.Zero)
                {
                    ++position;
                    if (position < performance.PerformanceParts.Length)
                    {
                        PartTime = performance.PerformanceParts[position].Duration;
                        Color = Color.Parse(performance.PerformanceParts[position].Color);
                        PartNote = performance.PerformanceParts[position].Description;
                        PartTime -= TimeSpan.FromSeconds(1);
                        PartName = performance.PerformanceParts[position].Name;
                        if (Vibration.Default.IsSupported)
                        {
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
                        }
                    }
                    else
                    {
                        if (Vibration.Default.IsSupported)
                        {
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(300));
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                        }
                    }
                }
                else
                {
                    PartTime -= TimeSpan.FromSeconds(1);
                }
            };
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("timer"))
            {
                var model = (PerformanceModel)query["timer"];
                performance = model;
                PartTime = model.PerformanceParts[0].Duration;
                PartName = performance.PerformanceParts[0].Name;
                PartNote = performance.PerformanceParts[0].Description;
                Time = model.Duration;
                Color = Color.Parse(model.PerformanceParts[0].Color);
                query.Clear();
            }

        }

        [RelayCommand]
        public void Run()
        {
            if (timer.IsRunning)
            {
                return;
            }

            timer.Start();
        }

        [RelayCommand]
        public void Stop()
        {
            timer.Stop();
        }
    }
}
