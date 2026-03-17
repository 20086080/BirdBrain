using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdProfilePage : BasePage
{
    private readonly SummaryService _summaryService;
    public BirdProfilePage(SummaryService summaryService)
    {
        InitializeComponent();
        _summaryService = summaryService;
        App.State.LeftSelected = false;
        BindingContext = App.State;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            App.State.TotalBirdSightings = await _summaryService.GetTotalBirdCountAsync(App.State.Lat, App.State.Lng, App.State.SelectedBirdCommonName);
            if (App.State.TotalBirdSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoBirdsFound);
            }
            App.State.TotalSightings = await _summaryService.GetTotalLocationCountAsync(App.State.Lat, App.State.Lng);
            App.State.PercTotalBirdSightings = 100 * ((double)App.State.TotalBirdSightings / App.State.TotalSightings);
            App.State.BirdDailyObs = await _summaryService.GetBirdDailyObsAsync(App.State.Lat, App.State.Lng, App.State.SelectedBirdCommonName);
            App.State.BirdTimeObs = await _summaryService.GetBirdTimeObsAsync(App.State.Lat, App.State.Lng, App.State.SelectedBirdCommonName);
            App.State.BuildChartBird(App.State.BirdDailyObs);
            App.State.BuildChartBirdTime(App.State.BirdTimeObs);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex}");
            await ErrorService.Show(ErrorType.ErrorFound);
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
        App.State.LeftSelected = true;
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
        App.State.LeftSelected = false;
        // Navigate using Shell
        await Shell.Current.GoToAsync(nameof(BirdSightingPage));
    }
}