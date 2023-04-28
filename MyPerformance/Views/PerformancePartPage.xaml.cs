using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using MyPerformance.Services.Interfaces;
using MyPerformance.ViewModels;

namespace MyPerformance.Views;

public partial class PerformancePartPage : ContentPage
{
    public PerformancePartPage(PerformancePartViewModel viewModel)
    {
        InitializeComponent();
        NoteEditor.WidthRequest = NameInput.Width;
        NoteEditor.MinimumWidthRequest = NameInput.Width;

        BindingContext = viewModel;
    }
}