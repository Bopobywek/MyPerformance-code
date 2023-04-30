using CommunityToolkit.Maui.Core;
using MyPerformance.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace MyPerformance.Views;

public partial class PerformancePage : ContentPage
{
	public PerformancePage(PerformanceViewModel viewModel)
	{
		InitializeComponent();
		
		BindingContext = viewModel;
	}

	private void TimerButton_Clicked(object sender, EventArgs e)
	{
		TimerButton.BackgroundColor = Color.Parse("#1282A2");
		PlanButton.BackgroundColor = Color.Parse("Gray");

		PlanSettings.IsVisible = false;
		TimerSettings.IsVisible = true;
    }

	private void PlanButton_Clicked(object sender, EventArgs e)
	{
        TimerButton.BackgroundColor = Color.Parse("Gray");
        PlanButton.BackgroundColor = Color.Parse("#1282A2");

        PlanSettings.IsVisible = true;
        TimerSettings.IsVisible = false;
    }
}