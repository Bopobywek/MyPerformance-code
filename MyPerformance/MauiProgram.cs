using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using MyPerformance.Services;
using MyPerformance.Views;
using MyPerformance.ViewModels;
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
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
                fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
            });

        builder.Services
            .AddServices()
            .AddTransient<PerformancePartViewModel>()
            .AddTransient<PerformanceViewModel>()
            .AddTransient<MainViewModel>()
            .AddTransient<PerformancePartPage>();

        Routing.RegisterRoute(nameof(PerformancePage), typeof(PerformancePage));
        Routing.RegisterRoute(nameof(PerformancePartPage), typeof(PerformancePartPage));


        return builder.Build();
    }
}
