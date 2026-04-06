
using BirdBrain.Helpers;
using BirdBrain.Models;
using BirdBrain.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
namespace BirdBrain.Views;

public partial class IntroPage : BasePage
{
    public bool IsNewLocation = false;
    public bool IsCarouselVisible { get; set; }

    public ObservableCollection<LatLng> FilteredLocations { get; set; } = new();

    private SavedLocation _carouselItem;

    public AppState State => App.State;

    public IntroPage()
    {
        InitializeComponent();
        App.State.LeftSelected = true;
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        App.State.LeftSelected = true;
        _carouselItem = CarouselCurrentItem;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            LocationCarousel.CurrentItem = _carouselItem;
            IsCarouselVisible = true;
        });
        FilteredLocations.Clear();
        DropdownBorder.IsVisible = false;
    }

    public SavedLocation CarouselCurrentItem
    {
        get
        {
            if (App.State.SavedLocations == null || App.State.SavedLocations.Count == 0)
            {
                
                return null!;
            }
            
            if (App.State.SelectedSavedLocation.Name == null)
                return App.State.SavedLocations[0];

            var match = App.State.SavedLocations.FirstOrDefault(b =>
                b.Name!.Equals(App.State.SelectedSavedLocation.Name, StringComparison.OrdinalIgnoreCase));

            return match ?? App.State.SavedLocations[0];
        }
    }

    async void OnAllBirdsTapped(object? sender, EventArgs e)
    {
        if (!App.State.HasLocation)
            return;
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void OnSpecificBirdTapped(object? sender, EventArgs e)
    {
        if (!App.State.HasLocation)
            return;
        App.State.LeftSelected = false;
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
    }

    async void OnLocationTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
        {
            
            return;
        }
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
        App.State.SelectedSavedLocation.Name = SearchBox.Text;
        
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

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;

        FilteredLocations.Clear();

        if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 2)
        {
            DropdownBorder.IsVisible = false;
            return;
        }

        var results = App.State.GlobalLocations
            .Where(x =>
                (x.city_ascii?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.country?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false))
            .Take(20);

        foreach (var item in results)
            FilteredLocations.Add(item);

        DropdownBorder.IsVisible = FilteredLocations.Any();
    }

    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as LatLng;

        if (selected == null)
            return;

        
        SearchBox.Text = $"{selected.city_ascii}, {selected.country}";

       
        App.State.SelectedSavedLocation = new SavedLocation
        {
            Name = selected.city_ascii,
            Lat = selected.lat,
            Lng = selected.lng,
            Country = selected.country,
            CountryCode = selected.iso2
        };

        
        ((CollectionView)sender).SelectedItem = null;

        
        DropdownBorder.IsVisible = false;
        FilteredLocations.Clear();

        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SearchBox.Unfocus();
        });
    }
}
