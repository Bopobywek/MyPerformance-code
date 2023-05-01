using MyPerformance.ViewModels;
using Plugin.LocalNotification;

namespace MyPerformance.Views;

public partial class TimerPage : ContentPage
{
    public TimerPage()
	{
		InitializeComponent();

		BindingContext = new TimerViewModel();
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
	{
		base.OnNavigatingFrom(args);

		var viewModel = (TimerViewModel)BindingContext;
		viewModel.StopCommand.Execute(null);
    }
}