using MyPerformance.ViewModels;

namespace MyPerformance.Views;

public partial class PerformancePage : ContentPage
{
	public PerformancePage()
	{
		InitializeComponent();
		
		BindingContext = new PerformanceViewModel();
	}
}