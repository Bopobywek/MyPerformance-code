using MyPerformance.Views;
namespace MyPerformance;

public partial class AppShell : Shell
{
	public AppShell()
	{
		Routing.RegisterRoute(nameof(PerformancePage), typeof(PerformancePage));
		InitializeComponent();
	}
}
