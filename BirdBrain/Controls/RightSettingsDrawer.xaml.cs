
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

        await Application.Current.MainPage.DisplayAlertAsync("Debug", "OpenAsync hit", "OK");

        if (_isOpen) return;
        _isOpen = true;

        Overlay.IsVisible = true;

        await Task.WhenAll(
            Overlay.FadeToAsync(1, AnimationSpeed, Easing.CubicIn),
            Drawer.TranslateToAsync(0, 0, AnimationSpeed, Easing.CubicOut)
        );
    }

    public async Task CloseAsync()
    {
        if (!_isOpen) return;

        await Task.WhenAll(
            Overlay.FadeToAsync(0, AnimationSpeed, Easing.CubicOut),
            Drawer.TranslateToAsync(320, 0, AnimationSpeed, Easing.CubicIn)
        );

        Overlay.IsVisible = false;
        _isOpen = false;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}