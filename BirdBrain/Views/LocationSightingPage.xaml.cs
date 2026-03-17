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
        if (App.State == null)
            throw new Exception("App.State is NULL");

       // _summaryService = summaryService;
             _summaryService = App.Current?.Handler?.MauiContext?.Services
                .GetRequiredService<SummaryService>();
        if (_summaryService == null)
            throw new Exception("SummaryService is NULL");
        BindingContext = App.State;
        App.State.LeftSelected = true;
    }
    protected override async void OnAppearing()
    {
        
        try
        {
            base.OnAppearing();
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
            App.State.LeftSelected = true;
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
            App.State.LeftSelected = false;
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