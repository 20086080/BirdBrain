using BirdBrain.Models;
using BirdBrain.Services;
using System.Diagnostics;
//using static AndroidX.Core.Text.Util.LocalePreferences.FirstDayOfWeek;

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
            
            App.State.CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
            App.State.TotalSightings = await SummaryService.GetTotalLocationCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.CutoffDate);
            if (App.State.TotalSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoLocationDataFound);
            }
            string DateToday = await SummaryService.LatestDateStmpAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, 0);

            App.State.TotalTypeOfBird = await SummaryService.GetTotalLocationBirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.CutoffDate);
            App.State.TopBirds = await SummaryService.GetTop5BirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.CutoffDate, DateToday);
            App.State.LocationDailyObs = await SummaryService.GetLocationDailyObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.CutoffDate);
            App.State.BuildChart(App.State.LocationDailyObs);
            App.State.LeftSelected = true;
            App.State.TodayObs = App.State.TopBirds?.Sum(b => b.StatsToday) ?? 0;
            int NumberDays = await SummaryService.DateStampTotalAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.CutoffDate);
            App.State.AverageObs = App.State.TotalSightings / NumberDays;
            App.State.NewBirds = 0;
            if (NumberDays > 1)
            {
                string previousDate = await SummaryService.LatestDateStmpAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, 1);
                int previousCount = await SummaryService.BirdCountForDateAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, previousDate);
                App.State.NewBirds = App.State.TodayObs - previousCount; 
            }
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