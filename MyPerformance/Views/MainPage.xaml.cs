using CommunityToolkit.Maui.Alerts;
using MyPerformance.ViewModels;

namespace MyPerformance;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

    }

	private void Button_Clicked(object sender, EventArgs e)
	{

	}

	private void PerformancesListButton_Clicked(object sender, EventArgs e)
	{
		if (PerformancesView.IsVisible)
		{
			Toast.Make("Вы уже на этой странице").Show();
			return;
		}

		PerformancesView.IsVisible = true;
		StatisticsView.IsVisible = false;
	}

	private void StatisticsButton_Clicked(object sender, EventArgs e)
	{
        if (StatisticsView.IsVisible)
        {
            Toast.Make("Вы уже на этой странице").Show();
            return;
        }

        PerformancesView.IsVisible = false;
        StatisticsView.IsVisible = true;
    }
}

