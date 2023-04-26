namespace MyPerformance.Views;

public partial class PerformancePartPage : ContentPage
{
	public PerformancePartPage()
	{
		InitializeComponent();
	}

	private void ColorPicker_PickedColorChanged(object sender, Color e)
	{
		ColorResult.BackgroundColor = e;
	}
}