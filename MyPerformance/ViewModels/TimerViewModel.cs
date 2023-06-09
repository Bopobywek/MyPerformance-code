﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPerformance.Models;
using MyPerformance.Repositories;
using Plugin.LocalNotification;

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

        [ObservableProperty]
        private bool isTimerRunning;

        [ObservableProperty]
        private bool isSkipForwardAvailable;

        [ObservableProperty]
        private bool isSkipBackwardAvailable;

        [ObservableProperty]
        private bool isEndSignalSend;

        public bool IsTimerAlreadyStarted { get; private set; }

        private int position = 0;
        private int Position
        {
            get => position;
            set
            {
                var lastPosition = performance.PerformanceParts?.Length - 1 ?? 0;
                if (value < 0 || value > lastPosition)
                {
                    return;
                }
                position = value;

                if (position == 0)
                {
                    IsSkipBackwardAvailable = false;
                }
                else
                {
                    IsSkipBackwardAvailable = true;
                }
                if (position == lastPosition)
                {
                    IsSkipForwardAvailable = false;
                }
                else
                {
                    IsSkipForwardAvailable = true;
                }


            }
        }

        private PerformanceModel performance;

        IDispatcherTimer timer;
        private readonly PerformancesRepository performancesRepository;

        public TimerViewModel(PerformancesRepository performancesRepository)
        {
            this.performancesRepository = performancesRepository;
            timer = Dispatcher.GetForCurrentThread().CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += OnTimerTickThreadSafe;
        }

        private void OnTimerTickThreadSafe(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                OnTimerTick(sender, e);
            });
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Time -= TimeSpan.FromSeconds(1);

            if (PartTime != TimeSpan.Zero)
            {
                PartTime -= TimeSpan.FromSeconds(1);
                return;
            }

            if (Position == performance.PerformanceParts.Length - 1 && isEndSignalSend)
            {
                return;
            }
            else if (Position == performance.PerformanceParts.Length - 1)
            {
                if (performance.IsVibrationEnable)
                {
                    Vibrate(TimeSpan.FromMilliseconds(1000));
                }

                if (performance.IsNotificationEnable)
                {
                    SendNotification("Выступление подошло к концу",
                        $"Запланированное время для выступления {performance.Name} истекло");
                }


                isEndSignalSend = true;
                return;
            }

            Position += 1;
            UpdateTimerPart(performance.PerformanceParts[Position], 1);

            if (performance.IsVibrationEnable)
            {
                Vibrate(TimeSpan.FromMilliseconds(500));
            }

            if (performance.IsNotificationEnable)
            {
                SendNotification($"Новая часть выступления: {PartName}",
                "Вы переходите к новой части Вашего выступления");
            }
        }

        private void Vibrate(TimeSpan duration)
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(duration);
            }
        }

        private void SendNotification(string title, string description)
        {
            var request = new NotificationRequest
            {
                NotificationId = 217,
                Title = title,
                Description = description,
                BadgeNumber = 1
            };

            LocalNotificationCenter.Current.Show(request);
        }


        private void UpdateTimerPart(PerformancePartModel partModel, int offset)
        {
            PartTime = partModel.Duration;
            Color = Color.Parse(partModel.Color);
            PartNote = partModel.Description;
            PartTime -= TimeSpan.FromSeconds(offset);
            PartName = partModel.Name;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("timer"))
            {
                var model = (PerformanceModel)query["timer"];
                performance = model;
                Array.Sort(performance.PerformanceParts, (x, y) => x.Position.CompareTo(y.Position));
                Position = 0;
                UpdateTimerPart(performance.PerformanceParts[0], 0);
                Time = model.Duration;
                query.Clear();
            }

        }

        [RelayCommand]
        public void ChangeTimerState()
        {
            if (timer.IsRunning)
            {
                Stop();
                return;
            }
            Run();
        }

        [RelayCommand]
        public void Run()
        {
            if (timer.IsRunning)
            {
                return;
            }

            timer.Start();
            IsTimerAlreadyStarted = true;
            IsTimerRunning = true;
        }

        [RelayCommand]
        public void SkipForward()
        {
            if (Position != (performance.PerformanceParts?.Length - 1 ?? 0))
            {
                Position += 1;
                UpdateTimerPart(performance.PerformanceParts[Position], 0);
            }
        }

        [RelayCommand]
        public void SkipBackward()
        {
            Position -= 1;
            UpdateTimerPart(performance.PerformanceParts[Position], 0);
        }

        [RelayCommand]
        public void Stop()
        {
            timer.Stop();
            IsTimerRunning = false;
        }

        [RelayCommand]
        public void SaveStatistics()
        {
            if (IsTimerAlreadyStarted)
            {
                var delay = Math.Abs(Math.Min(Time.TotalSeconds, 0));
                var totalDuration = performance.Duration.TotalSeconds - Time.TotalSeconds;
                ++performance.NumberOfLaunches;
                performance.TotalDuration += totalDuration;
                performance.TotalDelayTime += delay;

                performancesRepository.AddOrUpdate(performance);
            }
        }
    }
}
