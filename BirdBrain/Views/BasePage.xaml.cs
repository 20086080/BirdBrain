
using BirdBrain.Controls;
using BirdBrain.Services;
using System.Windows.Markup;
namespace BirdBrain.Views;

public partial class BasePage : ContentPage
{
    BoxView _drawerOverlay;
    LeftSettingsDrawer _leftDrawer;
    bool _isDrawerOpen;
    BoxView _swipeCatcher;
    const double DrawerHiddenX = -360;
    public BasePage()
    {
        InitializeComponent();        
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _drawerOverlay = GetTemplateChild("DrawerOverlay") as BoxView;
        _leftDrawer = GetTemplateChild("LeftDrawer") as LeftSettingsDrawer;

        if (_drawerOverlay == null || _leftDrawer == null)
            return;

        _drawerOverlay.IsVisible = false;

        // 🔒 GUARANTEED hidden
        _leftDrawer.TranslationX = DrawerHiddenX;
    }

    async void OnSwipeRight(object sender, SwipedEventArgs e)
    {
        await OpenDrawer();
    }

    async void OnSwipeLeft(object sender, SwipedEventArgs e)
    {
        await CloseDrawer();
    }

   async Task OpenDrawer()
    {
        if (_leftDrawer == null) return;

        _drawerOverlay.IsVisible = true;
        await _leftDrawer.TranslateTo(0, 0, 250, Easing.CubicOut);
    }

    async Task CloseDrawer()
    {
        if (_leftDrawer == null) return;

        await _leftDrawer.TranslateTo(DrawerHiddenX, 0, 250, Easing.CubicIn);
        _drawerOverlay.IsVisible = false;
    }

    


    protected AppState AppState =>
    Application.Current
        .Handler
        .MauiContext
        .Services
        .GetRequiredService<AppState>();
}