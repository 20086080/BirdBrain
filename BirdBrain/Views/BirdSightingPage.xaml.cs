using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSightingPage : BasePage
{
    private readonly SummaryService _summaryService;
    public BirdSightingPage(SummaryService summaryService)
	{
        InitializeComponent();
        _summaryService = summaryService;
        AppState.LeftSelected = false;
        BindingContext = AppState;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            AppState.TotalBirdSightings = await _summaryService.GetTotalBirdCountAsync(AppState.Lat, AppState.Lng, AppState.SelectedBirdCommonName);
            if (AppState.TotalBirdSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoBirdsFound);
            }
            AppState.TotalSightings = await _summaryService.GetTotalLocationCountAsync(AppState.Lat, AppState.Lng);
            AppState.PercTotalBirdSightings = 100 * ((double)AppState.TotalBirdSightings / AppState.TotalSightings) ; 
            AppState.BirdDailyObs = await _summaryService.GetBirdDailyObsAsync(AppState.Lat, AppState.Lng, AppState.SelectedBirdCommonName);
            AppState.BirdTimeObs = await _summaryService.GetBirdTimeObsAsync(AppState.Lat, AppState.Lng, AppState.SelectedBirdCommonName);
            AppState.BuildChartBird(AppState.BirdDailyObs);
            AppState.BuildChartBirdTime(AppState.BirdTimeObs);
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
        AppState.LeftSelected = true;
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
        AppState.LeftSelected = false;
        // Navigate using Shell
        await Shell.Current.GoToAsync(nameof(BirdSightingPage));    
    }
}