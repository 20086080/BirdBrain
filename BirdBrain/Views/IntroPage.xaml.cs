
using BirdBrain.Models;
using BirdBrain.Services;
using BirdBrain.Helpers;
using System.ComponentModel.Design;
namespace BirdBrain.Views;

public partial class IntroPage : BasePage
{
    //private readonly JsonFileReader _jsonReader;
    
    //public List<SavedLocation> savedLocations = new List<SavedLocation>();
    //public IntroPage(JsonFileReader jsonReader)
    public IntroPage()
    {
        InitializeComponent();
        App.State.LeftSelected = true;
        //_jsonReader = jsonReader;
        BindingContext = App.State;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        //if (AppState.SavedLocations == null || AppState.SavedLocations.Count == 0)
        //{
        //    AppState.SavedLocations = await _jsonReader
        //        .ReadListAsync<SavedLocation>("LocationSeedData.json");
        //}

        //LocationCarousel.ItemsSource = AppState.SavedLocations;

        //if (AppState.SavedLocations != null && AppState.SavedLocations.Count > 0)
        //{
        //    if (!AppState.SavedLocations.Contains(AppState.SelectedSavedLocation))
        //    {
        //        AppState.SelectedSavedLocation = AppState.SavedLocations[0];
        //    }
        //}
        //else
        //{
        //    AppState.SelectedSavedLocation = null;
        //}
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        App.State.SelectedLocationName = LocationEntry.Text;
    }

    async void OnAllBirdsTapped(object sender, EventArgs e)
    {
        AllBirdsTab.Style = (Style)Application.Current.Resources["SegmentSelectedStyle"];
        SpecificBirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        AllBirdsLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        SpecificBirdLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void OnSpecificBirdTapped(object sender, EventArgs e)
    {
        SpecificBirdTab.Style = (Style)Application.Current.Resources["SegmentSelectedStyle"];
        AllBirdsTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        SpecificBirdLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        AllBirdsLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
    }

    async void OnLocationTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
            return;
        if (e.Parameter is SavedLocation location)
        {
            App.State.SelectedSavedLocation = location;
            App.State.SelectedLocationName = location.Name;
            App.State.Lat = location.Lat;
            App.State.Lng = location.Lng;
        }
    }

    async void OnUseLocationInvoked(object sender, EventArgs e)
    {
        // Optional micro animation
        await this.ScaleToAsync(0.98, 70);
        await this.ScaleToAsync(1, 70);

        // 🔥 Call location logic here
        // await GetCurrentLocation();
    }
}
