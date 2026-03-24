using BirdBrain.Services;
using BirdBrain.Models;
using System.Diagnostics;

namespace BirdBrain.Views;

public partial class LocationSightingPage : BasePage
{
    
    public LocationSightingPage()
    {
        InitializeComponent();
        if (App.State == null)
            throw new Exception("App.State is NULL");
        BindingContext = App.State;
        App.State.LeftSelected = true;
    }
    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            App.State.TotalSightings = await SummaryService.GetTotalLocationCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);
            if (App.State.TotalSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoLocationDataFound);
            }
            App.State.TotalTypeOfBird = await SummaryService.GetTotalLocationBirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);
            App.State.TopBirds = await SummaryService.GetTop5BirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);
            App.State.LocationDailyObs = await SummaryService.GetLocationDailyObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);
            App.State.BuildChart(App.State.LocationDailyObs);
            App.State.LeftSelected = true;

        }
        catch (Exception ex) 
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex}");
            await ErrorService.Show(ErrorType.ErrorFound);
        }
    }

    async void LocationTapped(object? sender, EventArgs e)
    {
        try
        {
            LocationTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
            BirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
            LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
            BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
            App.State.LeftSelected = true;
            await Shell.Current.GoToAsync(nameof(LocationSightingPage));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
    }

    async void BirdTapped(object? sender, EventArgs e)
    {
        try
        {
            BirdTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
            LocationTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
            BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
            LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
            App.State.LeftSelected = false;
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
    }
}