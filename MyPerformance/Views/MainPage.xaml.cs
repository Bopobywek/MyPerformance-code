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
}

