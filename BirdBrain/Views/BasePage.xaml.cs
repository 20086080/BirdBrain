
using BirdBrain.Controls;
using BirdBrain.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using System.Windows.Markup;

namespace BirdBrain.Views;

public partial class BasePage : ContentPage
{
    private SummaryService _summaryService;

    BoxView _drawerOverlay;
    LeftSettingsDrawer _leftDrawer;
    RightSettingsDrawer _rightDrawer;
    bool _isDrawerOpen;
    BoxView _swipeCatcher;
    const double DrawerHiddenX = -360;
    const double RightDrawerHiddenX = 360;
    public BasePage()
    {
        InitializeComponent();
        Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page
        .SetUseSafeArea(this, false);
        Microsoft.Maui.Controls.NavigationPage.SetHasBackButton(this, false);
    }

    protected override void OnApplyTemplate()
    {
        try
        {
            base.OnApplyTemplate();
            _drawerOverlay = GetTemplateChild("DrawerOverlay") as BoxView;
            _leftDrawer = GetTemplateChild("LeftDrawer") as LeftSettingsDrawer;
            _rightDrawer = GetTemplateChild("RightDrawer") as RightSettingsDrawer;
            var header = GetTemplateChild("AppHeader") as AppHeader;

            if (_leftDrawer != null)
            {
                //Home Selection 
                _leftDrawer.HomeCommand = new Command(async () =>
                {
                    if (IsAnyDrawerOpen())
                        await CloseAllDrawers();

                    await Shell.Current.GoToAsync("//IntroPage");
                });

                // Bird Selection
                _leftDrawer.SelectBirdCommand = new Command(async () =>
                {
                    if (IsAnyDrawerOpen())
                        await CloseAllDrawers();

                    await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
                });

                // Sighting Selection
                _leftDrawer.SightingsCommand = new Command(async () =>
                {
                    if (IsAnyDrawerOpen())
                        await CloseAllDrawers();
                    if (App.State.LeftSelected)     //Location Selected on Page
                        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
                    else
                        await Shell.Current.GoToAsync(nameof(BirdSightingPage));
                });

                // Profile Selection
                _leftDrawer.ProfileCommand = new Command(async () =>
                {
                    if (IsAnyDrawerOpen())
                        await CloseAllDrawers();
                    if (App.State.LeftSelected)     //Location Selected on Page
                        await Shell.Current.GoToAsync(nameof(LocationProfilePage));
                    else
                        await Shell.Current.GoToAsync(nameof(BirdProfilePage));
                });

                
            }


            if (header != null)
            {
                header.HamburgerClicked += async (_, __) =>
                {
                    if (IsAnyDrawerOpen())
                        await CloseAllDrawers();
                    else
                        await OpenDrawer(); // ← existing left drawer logic
                };
                // Right drawer
                header.SettingsClicked += async (_, __) =>
                {
                    if (IsAnyDrawerOpen())
                        await CloseAllDrawers();
                    else
                        await OpenRightDrawer();
                };
            }

            bool IsAnyDrawerOpen()
            {
                return (_leftDrawer?.TranslationX == 0) || (_rightDrawer?.TranslationX == 0);
            }

            // 🔒 GUARANTEED hidden
            if (_leftDrawer!= null)
               _leftDrawer.TranslationX = DrawerHiddenX;
            if (_rightDrawer != null)
                _rightDrawer.TranslationX = RightDrawerHiddenX;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("TEMPLATE CRASH: " + ex);
            throw;
        }
    }
    async Task OpenRightDrawer()
    {
        if (_rightDrawer == null) return;

        _leftDrawer?.AbortAnimation("TranslateTo");
        _rightDrawer.AbortAnimation("TranslateTo");

        await CloseDrawer(); // closes left if open

        if (_drawerOverlay != null)
            _drawerOverlay.IsVisible = true;

        await _rightDrawer.TranslateToAsync(0, 0, 250, Easing.CubicOut);
    }

    async Task CloseRightDrawer()
    {
        if (_rightDrawer == null) return;

        await _rightDrawer.TranslateToAsync(RightDrawerHiddenX, 0, 250, Easing.CubicIn);
        if (_drawerOverlay != null)
            _drawerOverlay.IsVisible = false;
    }

    async void OnSwipeRight(object? sender, SwipedEventArgs e)
    {
        if (_rightDrawer?.TranslationX == 0)
            await CloseRightDrawer();
        else
            await OpenDrawer(); // existing LEFT drawer
    }

    async void OnSwipeLeft(object? sender, SwipedEventArgs e)
    {
        if (_leftDrawer?.TranslationX == 0)
            await CloseDrawer();
        else
            await OpenRightDrawer();
    }

    async Task OpenDrawer()
    {
        if (_leftDrawer == null) return;

        _leftDrawer.AbortAnimation("TranslateTo");
        _rightDrawer?.AbortAnimation("TranslateTo");

        if (_drawerOverlay != null)
            _drawerOverlay.IsVisible = true;

        await _leftDrawer.TranslateToAsync(0, 0, 250, Easing.CubicOut);
    }

    async Task CloseDrawer()
    {
        if (_leftDrawer == null) return;
        await _leftDrawer.TranslateToAsync(DrawerHiddenX, 0, 250, Easing.CubicIn);
        if (_drawerOverlay != null)
        _drawerOverlay.IsVisible = false;
    }

    async void OnOverlayTapped(object sender, EventArgs e)
    {
        await CloseAllDrawers();
    }

    async Task CloseAllDrawers()
    {
        if (_leftDrawer?.TranslationX == 0)
            await CloseDrawer();

        if (_rightDrawer?.TranslationX == 0)
            await CloseRightDrawer();
    }
    public AppState AppState =>
    Microsoft.Maui.Controls.Application.Current
        .Handler
        .MauiContext
        .Services
        .GetRequiredService<AppState>();

    public SummaryService SummaryService =>
    _summaryService ??=
        Microsoft.Maui.Controls.Application.Current
            .Handler
            .MauiContext
            .Services
            .GetRequiredService<SummaryService>();
}