
namespace BirdBrain.Controls;

public partial class RightSettingsDrawer : ContentView
{
    const uint AnimationSpeed = 250;
    bool _isOpen;

    public RightSettingsDrawer()
    {
        InitializeComponent();
    }

    public async Task OpenAsync()
    {

        await Application.Current.MainPage.DisplayAlert("Debug", "OpenAsync hit", "OK");

        if (_isOpen) return;
        _isOpen = true;

        Overlay.IsVisible = true;

        await Task.WhenAll(
            Overlay.FadeTo(1, AnimationSpeed, Easing.CubicIn),
            Drawer.TranslateTo(0, 0, AnimationSpeed, Easing.CubicOut)
        );
    }

    public async Task CloseAsync()
    {
        if (!_isOpen) return;

        await Task.WhenAll(
            Overlay.FadeTo(0, AnimationSpeed, Easing.CubicOut),
            Drawer.TranslateTo(320, 0, AnimationSpeed, Easing.CubicIn)
        );

        Overlay.IsVisible = false;
        _isOpen = false;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}