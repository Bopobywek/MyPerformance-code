using MyPerformance.ViewModels;
using MyPerformance.Services.Interfaces;
#if ANDROID
using MyPerformance.Platforms.Android;
#endif
namespace MyPerformance.Views;

public partial class TimerPage : ContentPage
{
#if ANDROID
    ScreenOffService _service;
#endif
    private readonly IAlertService alertService;

    public TimerPage(TimerViewModel viewModel, IAlertService alertService)
    {
        InitializeComponent();

        this.alertService = alertService;

        BindingContext = viewModel;
        DeviceDisplay.KeepScreenOn = true;
#if ANDROID
        _service = new ScreenOffService();
        _service.Start();
#endif
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);

        var viewModel = (TimerViewModel)BindingContext;
        viewModel.StopCommand.Execute(null);

#if ANDROID
        _service.Stop();
#endif
        DeviceDisplay.KeepScreenOn = false;
    }

    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is TimerViewModel viewModel && !viewModel.IsTimerAlreadyStarted)
        {
            return base.OnBackButtonPressed();
        }

        Dispatcher.Dispatch(async () =>
        {
            var leave = await alertService.ShowConfirmationAsync("Завершить таймер?",
                "Прогресс будет потерян. Вы уверены, что хотите завершить таймер?", "Да", "Нет");

            if (leave)
            {
                await Shell.Current.GoToAsync("..");
            }
        });

        return true;
    }
}