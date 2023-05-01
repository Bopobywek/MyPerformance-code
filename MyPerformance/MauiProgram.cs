using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using MyPerformance.Services;
using MyPerformance.Views;
using MyPerformance.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;
using MyPerformance.Repositories;

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
            .AddTransient<PerformancePage>()
            .AddTransient<MainPage>()
            .AddTransient<MainViewModel>()
            .AddTransient<PerformancePartPage>();

        Routing.RegisterRoute(nameof(PerformancePage), typeof(PerformancePage));
        Routing.RegisterRoute(nameof(PerformancePartPage), typeof(PerformancePartPage));
        Routing.RegisterRoute(nameof(TimerPage), typeof(TimerPage));

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "performances.db3");
        builder.Services.AddSingleton<PerformancesRepository>(s => ActivatorUtilities.CreateInstance<PerformancesRepository>(s, dbPath));

        return builder.Build();
    }
}
