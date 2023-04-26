using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace MyPerformance;

[Activity(Theme = "@style/Maui.SplashTheme", WindowSoftInputMode = Android.Views.SoftInput.AdjustPan, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
