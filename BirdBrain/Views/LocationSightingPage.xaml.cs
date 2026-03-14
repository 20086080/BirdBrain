using BirdBrain.Services;
using BirdBrain.Models;
namespace BirdBrain.Views;

public partial class LocationSightingPage : BasePage
{
    private readonly SummaryService _summaryService;

    public LocationSightingPage(SummaryService summaryService)
    {
        InitializeComponent();
        _summaryService = summaryService;

        BindingContext = App.State;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        App.State.TotalSightings = await _summaryService.GetTotalLocationCountAsync(App.State.Lat,App.State.Lng);
        App.State.TotalTypeOfBird = await _summaryService.GetTotalLocationBirdCountAsync(App.State.Lat, App.State.Lng);
        App.State.TopBirds = await _summaryService.GetTop5BirdCountAsync(App.State.Lat, App.State.Lng);
        var data = await _summaryService.GetLocationDailyObsAsync(App.State.Lat, App.State.Lng);
        AppState.BuildChart(data);
    }

    void LocationTapped(object sender, EventArgs e)
    {
        LocationTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        BirdTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        LocationTabLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        BirdTabLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
    }

    async void BirdTapped(object sender, EventArgs e)
    {
        BirdTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        LocationTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        BirdTabLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        LocationTabLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];

        // Navigate using Shell
        //await Shell.Current.GoToAsync(nameof(BirdSightingPage));     //Call Location Selection
    }
}