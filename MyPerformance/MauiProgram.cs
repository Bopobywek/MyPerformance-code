using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MyPerformance;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
				fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
			});
            return builder.Build();
	}
}
