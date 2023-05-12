using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using MyPerformance.Services.Interfaces;
using MyPerformance.ViewModels;

namespace MyPerformance.Views;

public partial class PerformancePartPage : ContentPage
{
    private readonly IAlertService alertService;

    public PerformancePartPage(PerformancePartViewModel viewModel, IAlertService alertService)
    {
        InitializeComponent();
        NoteEditor.WidthRequest = NameInput.Width;
        NoteEditor.MinimumWidthRequest = NameInput.Width;

        BindingContext = viewModel;
        this.alertService = alertService;
    }

    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            var leave = await alertService.ShowConfirmationAsync("��������� � �����������?",
                "��������� ���������� ����� ��������. �� �������, ��� ������ ���������?", "��", "���");

            if (leave)
            {
                await Shell.Current.GoToAsync("..");
            }
        });

        return true;
    }
}