using BirdBrain.Services;
using BirdBrain.Models;
using System.Diagnostics;
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
        try
        {
            App.State.TotalSightings = await _summaryService.GetTotalLocationCountAsync(App.State.Lat, App.State.Lng);
            if (App.State.TotalSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoLocationDataFound);
            }
            App.State.TotalTypeOfBird = await _summaryService.GetTotalLocationBirdCountAsync(App.State.Lat, App.State.Lng);
            App.State.TopBirds = await _summaryService.GetTop5BirdCountAsync(App.State.Lat, App.State.Lng);
            App.State.LocationDailyObs = await _summaryService.GetLocationDailyObsAsync(App.State.Lat, App.State.Lng);
            App.State.BuildChart(App.State.LocationDailyObs);

        }
        catch (Exception ex) 
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex}");
        }
    }

    async void LocationTapped(object sender, EventArgs e)
    {
        LocationTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        BirdTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        LocationTabLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        BirdTabLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];

        await Shell.Current.GoToAsync(nameof(LocationSightingPage));

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

        //Navigate using Shell
        await Shell.Current.GoToAsync(nameof(BirdSightingPage));     
    }
}