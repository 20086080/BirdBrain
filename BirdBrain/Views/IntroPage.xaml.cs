
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
        App.State.LeftSelected = true;
        _jsonReader = jsonReader;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        savedLocations = await _jsonReader
            .ReadListAsync<SavedLocation>("LocationSeedData.json");

        LocationCarousel.ItemsSource = savedLocations;
        if (App.State.SelectedLocationName != null)
        {
            LocationIndex = savedLocations.FindIndex(x => x.Name == App.State.SelectedLocationName);
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
        this.AppState.SelectedLocationName = LocationEntry.Text;
        
    }

    async void OnAllBirdsTapped(object sender, EventArgs e)
    {
        AllBirdsTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        SpecificBirdTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        AllBirdsLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        SpecificBirdLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void OnSpecificBirdTapped(object sender, EventArgs e)
    {
        SpecificBirdTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        AllBirdsTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        SpecificBirdLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        AllBirdsLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;
        // Navigate to BirdSelectionPage
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
    }

    async void OnLocationTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
            return;
        if (e.Parameter is SavedLocation location)
        {
            App.State.SelectedLocationName = location.Name;
            LocationIndex = savedLocations.FindIndex(x => x.Name == App.State.SelectedLocationName);
            if (LocationIndex >= 0)
            {
                LocationCarousel.Position = LocationIndex;
                App.State.Lat = location.Lat;
                App.State.Lng = location.Lng;
                App.State.City_Ascii = location.Region;
                App.State.Country = location.Country;
                App.State.SelectedLocationDesc = location.Description;
                App.State.SelectedLocationThumbnail = location.Thumbnail;
                App.State.SelectedLocationProfileImage = location.ProfileImage;
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
