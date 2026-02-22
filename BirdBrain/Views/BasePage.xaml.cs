
namespace BirdBrain.Views;

public partial class BasePage : ContentPage
{
    public BasePage()
    {
        InitializeComponent();
    }

    public Task OpenSettingsAsync()
        => SettingsDrawer.OpenAsync();
}