
using BirdBrain.Models;
using BirdBrain.Services;
using BirdBrain.Helpers;
namespace BirdBrain.Views;

public partial class IntroPage : BasePage
{
    private readonly JsonFileReader _jsonReader;

    public IntroPage(JsonFileReader jsonReader)
    {
        InitializeComponent();
        _jsonReader = jsonReader;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var savedLocations = await _jsonReader
            .ReadListAsync<SavedLocation>("LocationSeedData.json");

        LocationCarousel.ItemsSource = savedLocations;
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

        // Navigate to BirdSelectionPage
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
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
