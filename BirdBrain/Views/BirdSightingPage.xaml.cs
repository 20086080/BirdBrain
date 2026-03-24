using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSightingPage : BasePage
{
    //private readonly SummaryService _summaryService;
    public BirdSightingPage()
	{
        InitializeComponent();
        if (App.State == null)
            throw new Exception("App.State is NULL");
        //_summaryService = summaryService;
        App.State.LeftSelected = false;
        BindingContext = App.State;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LocationTab.Style = (Style)Application.Current!.Resources["SegmentUnselectedStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        try
        {
            App.State.TotalBirdSightings = await SummaryService.GetTotalBirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.SelectedSavedBird.CommonName!);
            if (App.State.TotalBirdSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoBirdsFound);
            }
            App.State.TotalSightings = await SummaryService.GetTotalLocationCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);
            App.State.PercTotalBirdSightings = 100 * ((double)App.State.TotalBirdSightings / App.State.TotalSightings) ; 
            App.State.BirdDailyObs = await SummaryService.GetBirdDailyObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.SelectedSavedBird.CommonName!);
            App.State.BirdTimeObs = await SummaryService.GetBirdTimeObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.SelectedSavedBird.CommonName!);
            App.State.BuildChartBird(App.State.BirdDailyObs);
            App.State.BuildChartBirdTime(App.State.BirdTimeObs);
            App.State.LeftSelected = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex}");
            await ErrorService.Show(ErrorType.ErrorFound);
        }
    }
    async void LocationTapped(object? sender, EventArgs e)
    {
        LocationTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
        BirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void BirdTapped(object? sender, EventArgs e)
    {
        BirdTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
        LocationTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;
        await Shell.Current.GoToAsync(nameof(BirdSightingPage));    
    }
}