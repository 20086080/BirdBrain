using BirdBrain.Services;
using BirdBrain.Models;
using System.Diagnostics;
namespace BirdBrain.Views;

public partial class LocationSightingPage : BasePage
{
    private readonly SummaryService _summaryService;
   // public LocationSightingPage(SummaryService summaryService)
    public LocationSightingPage()
    {
        InitializeComponent();
        
        //TODO Clean -up 
        if (AppState == null)
            throw new Exception("AppState is NULL");

       // _summaryService = summaryService;
             _summaryService = App.Current?.Handler?.MauiContext?.Services
                .GetRequiredService<SummaryService>();
        if (_summaryService == null)
            throw new Exception("SummaryService is NULL");
        BindingContext = AppState;
        AppState.LeftSelected = true;
    }
    protected override async void OnAppearing()
    {
        
        try
        {
            base.OnAppearing();
            AppState.TotalSightings = await _summaryService.GetTotalLocationCountAsync(AppState.Lat, AppState.Lng);
            if (AppState.TotalSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoLocationDataFound);
            }
            AppState.TotalTypeOfBird = await _summaryService.GetTotalLocationBirdCountAsync(AppState.Lat, AppState.Lng);
            AppState.TopBirds = await _summaryService.GetTop5BirdCountAsync(AppState.Lat, AppState.Lng);
            AppState.LocationDailyObs = await _summaryService.GetLocationDailyObsAsync(AppState.Lat, AppState.Lng);
            AppState.BuildChart(AppState.LocationDailyObs);

        }
        catch (Exception ex) 
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex}");
            await ErrorService.Show(ErrorType.ErrorFound);
        }
    }

    async void LocationTapped(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
    }

    async void BirdTapped(object sender, EventArgs e)
    {
        try
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
            //Navigate using Shell
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
    }
}