using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSightingPage : BasePage
{
    private readonly SummaryService _summaryService;
    public BirdSightingPage(SummaryService summaryService)
	{
        InitializeComponent();
        _summaryService = summaryService;

        BindingContext = App.State;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        App.State.TotalBirdSightings = await _summaryService.GetTotalBirdCountAsync(App.State.Lat, App.State.Lng, App.State.SelectedBirdCommonName);
        App.State.TotalSightings = await _summaryService.GetTotalLocationCountAsync(App.State.Lat, App.State.Lng);
        App.State.PercTotalBirdSightings = 100 * ((double)App.State.TotalBirdSightings / App.State.TotalSightings) ; 
        App.State.BirdDailyObs = await _summaryService.GetBirdDailyObsAsync(App.State.Lat, App.State.Lng, App.State.SelectedBirdCommonName);

        App.State.BuildChartBird(App.State.BirdDailyObs);

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

        // Navigate using Shell
        await Shell.Current.GoToAsync(nameof(BirdSightingPage));    
    }
}