

using BirdBrain.Services;
using BirdBrain.ViewModels;
using BirdBrain.Views;
using LiveChartsCore.Themes;
using System.Windows.Input;

namespace BirdBrain.Controls;

public partial class RightSettingsDrawer : ContentView
{
    public RightSettingsDrawer()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, EventArgs e)
    {
        setDarkLightUI();
    }

    // ===== Sliders =====
    public static readonly BindableProperty RadiusProperty =
        BindableProperty.Create(nameof(Radius), typeof(double), typeof(RightSettingsDrawer), 10.0);

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public static readonly BindableProperty CloseCommandProperty =
        BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(RightSettingsDrawer));

    public ICommand CloseCommand
    {
        get => (ICommand)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    void OnDarkTapped(object sender, EventArgs e)
    {
        App.State!.DarkLightMode = true;
        setDarkLightUI();
        if (ThemeManager.CurrentThemeType != null)
        {
            ThemeManager.ReapplyThemeForMode(ThemeManager.CurrentThemeType);
        }
    }

    void OnLightTapped(object sender, EventArgs e)
    {
        App.State!.DarkLightMode = false;
        setDarkLightUI();
        if (ThemeManager.CurrentThemeType != null)
        {
            ThemeManager.ReapplyThemeForMode(ThemeManager.CurrentThemeType);
        }
    }

    void setDarkLightUI()
    {
        if (App.State!.DarkLightMode)
        {
            DarkTab.Style =
            (Style)Application.Current!.Resources["SegmentSelectedStyle"];

            LightTab.Style =
                (Style)Application.Current.Resources["SegmentUnselectedStyle"];

            DarkLabel.Style =
                (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

            LightLabel.Style =
                (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        }
        else
        {
            LightTab.Style =
            (Style)Application.Current!.Resources["SegmentSelectedStyle"];

            DarkTab.Style =
                (Style)Application.Current.Resources["SegmentUnselectedStyle"];

            LightLabel.Style =
                (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

            DarkLabel.Style =
                (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        }
    }
}