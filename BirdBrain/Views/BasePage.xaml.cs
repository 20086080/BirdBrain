
using BirdBrain.Controls;
using BirdBrain.Models;
using BirdBrain.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using System.Windows.Markup;

namespace BirdBrain.Views;

public partial class BasePage : ContentPage
{
    private SummaryService? _summaryService;
    BoxView? _drawerOverlay;
    LeftSettingsDrawer? _leftDrawer;
    RightSettingsDrawer? _rightDrawer;
    String? CutoffDate;
    bool _isDrawerOpen;
    BoxView? _swipeCatcher;
    const double DrawerHiddenX = -360;
    const double RightDrawerHiddenX = 360;
    bool _initialized;

    public BasePage()
    {
        InitializeComponent();
        Microsoft.Maui.Controls.NavigationPage.SetHasBackButton(this, false);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (_initialized)
            { return; }
        _initialized = true;
        _drawerOverlay = GetTemplateChild("DrawerOverlay") as BoxView
            ?? throw new InvalidOperationException("DrawerOverlay not found");
        
        _leftDrawer = GetTemplateChild("LeftDrawer") as LeftSettingsDrawer
            ?? throw new InvalidOperationException("Left Drawer not found");
        _rightDrawer = GetTemplateChild("RightDrawer") as RightSettingsDrawer
            ?? throw new InvalidOperationException("Right Drawer not found");
        
        Dispatcher.Dispatch(() =>
        {
            SetupDrawers();
        } );
        
        _drawerOverlay?.Opacity = 0;
        _drawerOverlay?.InputTransparent = true;
    }


    void SetupDrawers()
    {   
        

        try
        {
            var header = GetTemplateChild("AppHeader") as AppHeader;
            if (_leftDrawer != null)
            {
                _leftDrawer.HomeCommand = new Command(async () =>               //Home Selection 
                {
                    await ExecuteWithDrawerClose(() =>
                        Shell.Current.GoToAsync("//IntroPage"));
                });
               
                _leftDrawer.SelectBirdCommand = new Command(async () =>         // Bird Selection
                {
                    if (App.State.HasLocation)
                    { 
                        await ExecuteWithDrawerClose(() =>
                        Shell.Current.GoToAsync(nameof(BirdSelectionPage)));
                    }
                });
                
                _leftDrawer.SightingsCommand = new Command(async () =>          // Sighting Selection
                {
                    if (App.State.LeftSelected)                                 // Originating page is Location
                    {
                        if (!App.State.HasLocation)                             // Location is not selected
                        {
                            await ErrorService.Show(ErrorType.LocationNotSelected);
                            await CloseDrawer();
                            return;
                        }
                        CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
                        var result = await SummaryService.LocationHasDataAsync(App.State.SelectedSavedLocation!.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate);
                        if (!result)                                            // Location has no Data in Sql table for date range 
                        {
                            await ErrorService.Show(ErrorType.NoLocationDataFound);
                            await CloseDrawer();
                            return;
                        }
                        await ExecuteWithDrawerClose(() =>
                        Shell.Current.GoToAsync(nameof(LocationSightingPage)));                    
                    }
                    else
                    {
                        if (!App.State.HasBird)                                  // If bird not selected 
                        {
                            await ErrorService.Show(ErrorType.BirdNotSelected);
                            await CloseDrawer();
                            return;
                        }
                        CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
                        string? comName = App.State.SelectedSavedBird?.CommonName;
                        var result = await SummaryService.BirdHasDataAsync(App.State.SelectedSavedLocation!.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate, comName!);
                        if (!result)                                            // Bird has no data for this Location and date range 
                        {
                            await ErrorService.Show(ErrorType.NoBirdsFound);
                            await CloseDrawer();
                            return;
                        }
                        await ExecuteWithDrawerClose(() =>
                        Shell.Current.GoToAsync(nameof(BirdSightingPage)));
                        
                    }
                });
              
                _leftDrawer.ProfileCommand = new Command(async () =>            // Profile Selection
                {
                    if (App.State.LeftSelected)                                 // Originating page is Location 
                    {
                        if (App.State.HasLocation)                              // If a location has been selected
                        {
                            await ExecuteWithDrawerClose(() =>
                                Shell.Current.GoToAsync(nameof(LocationProfilePage)));
                        }
                    }
                    else
                    {
                        if (App.State.HasBird)                                  // If a bird has been selected & has data in table
                        {
                            await ExecuteWithDrawerClose(() =>
                                 Shell.Current.GoToAsync(nameof(BirdProfilePage)));
                        }
                    }
                });
            }

            if (header != null)
            {
                header.HamburgerClicked += async (_, __) =>             // Left drawer
                {
                    if (_drawerState == DrawerState.LeftOpen)
                        await CloseDrawer();
                    else
                        await OpenDrawer();
                };
                
                header.SettingsClicked += async (_, __) =>              // Right drawer
                {
                    if (_drawerState == DrawerState.RightOpen)
                        await CloseRightDrawer();
                    else
                        await OpenRightDrawer();
                };
            }

            if (_leftDrawer != null)
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

    enum DrawerState
    {
        Closed,
        LeftOpen,
        RightOpen
    }

    DrawerState _drawerState = DrawerState.Closed;

    bool IsAnyDrawerOpen()
    {
        return _drawerState != DrawerState.Closed;
    }

    async Task ExecuteWithDrawerClose(Func<Task> action)
    {
        if (IsAnyDrawerOpen())
            await CloseAllDrawers();
        await action();
    }

    async Task OpenRightDrawer()
    {
        if (_rightDrawer == null) return;
        _leftDrawer?.AbortAnimation("TranslateTo");
        _rightDrawer.AbortAnimation("TranslateTo");
        if (_drawerState == DrawerState.LeftOpen)
            await CloseDrawer();

        if (_drawerOverlay != null)
        { 
            _drawerOverlay.Opacity = 1;
            _drawerOverlay.InputTransparent = false;
        }
        await _rightDrawer.TranslateToAsync(0, 0, 250, Easing.CubicOut);
        _drawerState = DrawerState.RightOpen;
    }

    async Task CloseRightDrawer()
    {
        if (_rightDrawer == null) return;
        await _rightDrawer.TranslateToAsync(RightDrawerHiddenX, 0, 250, Easing.CubicIn);
        if (_drawerOverlay != null)
        {
            _drawerOverlay.Opacity = 0;
            _drawerOverlay.InputTransparent = true;
        }
        _drawerState = DrawerState.Closed;
    }

    async void OnSwipeRight(object? sender, SwipedEventArgs e)
    {
        if (_drawerState == DrawerState.RightOpen)
            await CloseRightDrawer();
        else
            await OpenDrawer();
    }

    async void OnSwipeLeft(object? sender, SwipedEventArgs e)
    {
        if (_drawerState == DrawerState.LeftOpen)
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
        {
            _drawerOverlay.Opacity = 1;
            _drawerOverlay.InputTransparent = false;
        }
        await _leftDrawer.TranslateToAsync(0, 0, 250, Easing.CubicOut);
        _drawerState = DrawerState.LeftOpen;
    }

    async Task CloseDrawer()
    {
        if (_leftDrawer == null) return;
        await _leftDrawer.TranslateToAsync(DrawerHiddenX, 0, 250, Easing.CubicIn);
        if (_drawerOverlay != null)
        {
            _drawerOverlay.Opacity = 0;
            _drawerOverlay.InputTransparent = true;
        }
        _drawerState = DrawerState.Closed;
    }

    async void OnOverlayTapped(object sender, EventArgs e)
    {
        await CloseAllDrawers();
    }

    async Task CloseAllDrawers()
    {
        if (_drawerState == DrawerState.LeftOpen)
            await CloseDrawer();
        else if (_drawerState == DrawerState.RightOpen)
            await CloseRightDrawer();
    }


    public SummaryService SummaryService =>
    _summaryService ??=
        Microsoft.Maui.Controls.Application.Current
            .Handler
            .MauiContext
            .Services
            .GetRequiredService<SummaryService>();
}