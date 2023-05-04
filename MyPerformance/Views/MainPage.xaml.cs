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

	private void Button_Clicked_1(object sender, EventArgs e)
	{
		PerformancesView.IsVisible = true;
		StatisticsView.IsVisible = false;
	}

	private void Button_Clicked_2(object sender, EventArgs e)
	{
        PerformancesView.IsVisible = false;
        StatisticsView.IsVisible = true;
    }
}

