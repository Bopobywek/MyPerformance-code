using CommunityToolkit.Maui.Core;
using MyPerformance.ViewModels;
using CommunityToolkit.Maui.Alerts;
using MyPerformance.Services;
using MyPerformance.Services.Interfaces;

namespace MyPerformance.Views;

public partial class PerformancePage : ContentPage
{
    private readonly IAlertService alertService;

    public PerformancePage(PerformanceViewModel viewModel, IAlertService alertService)
    {
        InitializeComponent();

        BindingContext = viewModel;
        this.alertService = alertService;
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

    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            var leave = await alertService.ShowConfirmationAsync("Вернуться на главную страницу?",
                "Введенная информация будет потеряна. Вы уверены, что хотите вернуться?", "Да", "Нет");

            if (leave)
            {
                await Shell.Current.GoToAsync("..", new Dictionary<string, object>() {
                { "upd-main", 1 }
                });
            }
        });

        return true;
    }
}