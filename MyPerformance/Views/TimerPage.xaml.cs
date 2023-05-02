using MyPerformance.ViewModels;
#if ANDROID
using MyPerformance.Platforms.Android;
#endif
namespace MyPerformance.Views;

public partial class TimerPage : ContentPage
{
#if ANDROID
    ForegroundServiceDemo _service;
#endif
    public TimerPage()
    {
        InitializeComponent();

        BindingContext = new TimerViewModel();
        DeviceDisplay.KeepScreenOn = true;
#if ANDROID
        _service = new ForegroundServiceDemo();
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
    }
}