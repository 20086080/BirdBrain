
using BirdBrain.Models;
using BirdBrain.Services;
using BirdBrain.Helpers;
using System.ComponentModel.Design;
namespace BirdBrain.Views;

public partial class IntroPage : BasePage
{
    private readonly JsonFileReader _jsonReader;
    public int LocationIndex = 0;
    public List<SavedLocation> savedLocations = new List<SavedLocation>();
    public IntroPage(JsonFileReader jsonReader)
    {
        InitializeComponent();
        AppState.LeftSelected = true;
        _jsonReader = jsonReader;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        savedLocations = await _jsonReader
            .ReadListAsync<SavedLocation>("LocationSeedData.json");

        LocationCarousel.ItemsSource = savedLocations;
        if (AppState.SelectedLocationName != null)
        {
            LocationIndex = savedLocations.FindIndex(x => x.Name == AppState.SelectedLocationName);
        }
        else
        {
            LocationCarousel.Position = 0;
        }
        if (LocationIndex >= 0)
        {
            LocationCarousel.Position = LocationIndex;
        }
        else
        {
            LocationCarousel.Position = 0;
        }
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        AppState.SelectedLocationName = LocationEntry.Text;
        
    }

    async void OnAllBirdsTapped(object sender, EventArgs e)
    {
        AllBirdsTab.Style = (Style)Application.Current.Resources["SegmentSelectedStyle"];
        SpecificBirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        AllBirdsLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        SpecificBirdLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        AppState.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void OnSpecificBirdTapped(object sender, EventArgs e)
    {
        SpecificBirdTab.Style = (Style)Application.Current.Resources["SegmentSelectedStyle"];
        AllBirdsTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        SpecificBirdLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        AllBirdsLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        AppState.LeftSelected = false;
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
    }

    async void OnLocationTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
            return;
        if (e.Parameter is SavedLocation location)
        {
            AppState.SelectedSavedLocation = location;
            AppState.SelectedLocationName = location.Name;
            LocationIndex = savedLocations.FindIndex(x => x.Name == AppState.SelectedLocationName);
            if (LocationIndex >= 0)
            {
                LocationCarousel.Position = LocationIndex;
                AppState.Lat = location.Lat;
                AppState.Lng = location.Lng;
                
            }
            else
            {
                LocationCarousel.Position = 0;
            }
        }
    }

    async void OnUseLocationInvoked(object sender, EventArgs e)
    {
        // Optional micro animation
        await this.ScaleToAsync(0.98, 70);
        await this.ScaleToAsync(1, 70);

        // 🔥 Call your location logic here
        // await GetCurrentLocation();
    }
}
