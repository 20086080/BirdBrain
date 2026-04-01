using System.Windows.Input;
using BirdBrain.Services;

namespace BirdBrain.Controls;

public partial class LeftSettingsDrawer : ContentView
{
    public LeftSettingsDrawer()
    {
        InitializeComponent();
        BindingContext = App.State;
    }

    async Task GoHome()
    {
        await Shell.Current.GoToAsync("//IntroPage");
    }

    // ===== Sliders =====
    public static readonly BindableProperty RadiusProperty =
        BindableProperty.Create(nameof(Radius), typeof(double), typeof(LeftSettingsDrawer), 10.0);

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public static readonly BindableProperty DaysProperty =
        BindableProperty.Create(nameof(Days), typeof(double), typeof(LeftSettingsDrawer), 30.0);

    public double Days
    {
        get => (double)GetValue(DaysProperty);
        set => SetValue(DaysProperty, value);
    }

    public static readonly BindableProperty CloseCommandProperty =
        BindableProperty.Create(
            nameof(CloseCommand), 
            typeof(ICommand), 
            typeof(LeftSettingsDrawer));

    public ICommand CloseCommand
    {
        get => (ICommand)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public static readonly BindableProperty SightingsCommandProperty =
        BindableProperty.Create(
            nameof(SightingsCommand), 
            typeof(ICommand), 
            typeof(LeftSettingsDrawer),
            null);

    public ICommand SightingsCommand
    {
        get => (ICommand)GetValue(SightingsCommandProperty);
        set => SetValue(SightingsCommandProperty, value);
    }

    public static readonly BindableProperty ProfileCommandProperty =
        BindableProperty.Create(
            nameof(ProfileCommand), 
            typeof(ICommand), 
            typeof(LeftSettingsDrawer), null);

    public ICommand ProfileCommand
    {
        get => (ICommand)GetValue(ProfileCommandProperty);
        set => SetValue(ProfileCommandProperty, value);
    }

    
    public static readonly BindableProperty SelectLocationCommandProperty =
        BindableProperty.Create(
            nameof(SelectLocationCommand), 
            typeof(ICommand), 
            typeof(LeftSettingsDrawer), 
            null);

    public ICommand SelectLocationCommand
    {
        get => (ICommand)GetValue(SelectLocationCommandProperty);
        set => SetValue(SelectLocationCommandProperty, value);
    }

    public static readonly BindableProperty HomeCommandProperty =
     BindableProperty.Create(
         nameof(HomeCommand),
         typeof(ICommand),
         typeof(LeftSettingsDrawer),
         null);

    public ICommand HomeCommand
    {
        get => (ICommand)GetValue(HomeCommandProperty);
        set => SetValue(HomeCommandProperty, value);
    }

    public static readonly BindableProperty SelectBirdCommandProperty =
     BindableProperty.Create(
         nameof(SelectBirdCommand),
         typeof(ICommand),
         typeof(LeftSettingsDrawer),
         null);
    public ICommand SelectBirdCommand
    {
        get => (ICommand)GetValue(SelectBirdCommandProperty);
        set => SetValue(SelectBirdCommandProperty, value);
    }
}