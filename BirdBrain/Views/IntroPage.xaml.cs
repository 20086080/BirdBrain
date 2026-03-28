
using BirdBrain.Helpers;
using BirdBrain.Models;
using BirdBrain.Services;
using System.ComponentModel.Design;
namespace BirdBrain.Views;

public partial class IntroPage : BasePage
{
    public bool IsNewLocation = false;
    public bool IsCarouselVisible { get; set; }

    private SavedLocation _carouselItem;

    public IntroPage()
    {
        InitializeComponent();
        App.State.LeftSelected = true;
        BindingContext = App.State;
    }

    protected override void OnAppearing()
    {

        base.OnAppearing();

        _carouselItem = CarouselCurrentItem;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            LocationCarousel.CurrentItem = _carouselItem;
            IsCarouselVisible = true;
        });
    }

    public SavedLocation CarouselCurrentItem
    {
        get
        {
            if (App.State.SavedLocations == null || App.State.SavedLocations.Count == 0)
                return null!;

            if (App.State.SelectedSavedLocation.Name == null)
                return App.State.SavedLocations[0];

            var match = App.State.SavedLocations.FirstOrDefault(b =>
                b.Name!.Equals(App.State.SelectedSavedLocation.Name, StringComparison.OrdinalIgnoreCase));

            return match ?? App.State.SavedLocations[0];
        }
    }

    async void OnAllBirdsTapped(object? sender, EventArgs e)
    {
        AllBirdsTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
        SpecificBirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        AllBirdsLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        SpecificBirdLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void OnSpecificBirdTapped(object? sender, EventArgs e)
    {
        SpecificBirdTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
        AllBirdsTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        SpecificBirdLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        AllBirdsLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
    }

    async void OnLocationTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
            return;
        if (e.Parameter is SavedLocation location)
        {
            App.State.SelectedSavedLocation = location;
        }
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry || e.OldTextValue == null || e.OldTextValue == e.NewTextValue)
            return;
        string textString = entry?.Text?.Trim()!;
        if (!IsNewLocation)
        {
            App.State.SelectedSavedLocation = new SavedLocation();
            IsNewLocation = true;
        }
        App.State.SelectedSavedLocation.Name = LocationEntry.Text;
    }


    private void Latitude_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry || e.OldTextValue == null || e.OldTextValue == e.NewTextValue)
            return;
        string textString = entry?.Text?.Trim()!;
        double.TryParse(textString, out double lat);
        if (!IsNewLocation)
        {
            App.State.SelectedSavedLocation = new SavedLocation();
            IsNewLocation = true;
        }
        App.State.SelectedSavedLocation.Lat = lat;
    }

    private void Longitude_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry || e.OldTextValue == null || e.OldTextValue == e.NewTextValue)
            return;
        string textString = entry?.Text?.Trim()!;
        double.TryParse(textString, out double lng);
        if (!IsNewLocation)
        {
            App.State.SelectedSavedLocation = new SavedLocation();
            IsNewLocation = true;
        }
        App.State.SelectedSavedLocation.Lng = lng;
    }

    async void OnUseLocationInvoked(object? sender, EventArgs e)
    {
        
        

        // 🔥 Call location logic here
        
    }
}
