
using BirdBrain.Services;
using System.Windows.Markup;
namespace BirdBrain.Views;

public partial class BasePage : ContentPage
{
    public BasePage()
    {
        InitializeComponent();
        
    }

    protected AppState AppState =>
    Application.Current
        .Handler
        .MauiContext
        .Services
        .GetRequiredService<AppState>();

    // public Task OpenSettingsAsync()
    //  => SettingsDrawer.OpenAsync();
}