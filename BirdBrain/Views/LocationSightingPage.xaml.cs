using BirdBrain.Models;
using BirdBrain.Services;
using LiveChartsCore;
using System.ComponentModel;
using System.Diagnostics;
using LiveChartsCore.SkiaSharpView;

namespace BirdBrain.Views;

public partial class LocationSightingPage : BasePage, INotifyPropertyChanged
{
    public AppState State => App.State;

    public int TotalSightings { get; set; }
    public int TodayObs { get; set; }
    public int AverageObs { get; set; }
    public int NewBirds { get; set; }
    public string? CutoffDate { get; set; }
    public int TotalTypeOfBird { get; set; }

    public List<TopBirds> TopBirds { get; set; } = new();

    public List<LocationDailyObs> LocationDailyObs { get; set; } = new();

    // SERIES
    private ISeries[] _series = Array.Empty<ISeries>();
    public ISeries[] Series
    {
        get => _series;
        set
        {
            _series = value ?? Array.Empty<ISeries>();
            OnPropertyChanged(nameof(Series));
        }
    }

    // LABELS
    private List<string> _labels = new();
    public List<string> Labels
    {
        get => _labels;
        set
        {
            _labels = value ?? new List<string>();
            OnPropertyChanged(nameof(Labels));
        }
    }

    // X AXES
    private Axis[] _xAxes =
    {
    new Axis { LabelsRotation = 20 }
};

    public Axis[] XAxes
    {
        get => _xAxes;
        set
        {
            _xAxes = value ?? new Axis[] { new Axis { LabelsRotation = 20 } };
            OnPropertyChanged(nameof(XAxes));
        }
    }

    // Y AXES
    private Axis[] _yAxes =
    {
    new Axis { LabelsRotation = 20 }
};

    public Axis[] YAxes
    {
        get => _yAxes;
        set
        {
            _yAxes = value ?? new Axis[] { new Axis { LabelsRotation = 20 } };
            OnPropertyChanged(nameof(YAxes));
        }
    }

    public LocationSightingPage()
    {
        InitializeComponent();
        
        if (App.State == null)
            throw new Exception("App.State is NULL");
        BindingContext = this;
        App.State.LeftSelected = true;
    }
    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            var chartService = new ChartService();

            CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
            TotalSightings = await SummaryService.GetTotalLocationCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate);
            if (TotalSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoLocationDataFound);
            }
            string DateToday = await SummaryService.LatestDateStmpAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, 0);

            TotalTypeOfBird = await SummaryService.GetTotalLocationBirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate);
            TopBirds = await SummaryService.GetTop5BirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate, DateToday);
            
            
            App.State.LeftSelected = true;
            TodayObs = TopBirds?.Sum(b => b.StatsToday) ?? 0;
            int NumberDays = await SummaryService.DateStampTotalAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate);
            AverageObs = TotalSightings / NumberDays;
            NewBirds = 0;

            if (NumberDays > 1)
            {
                string previousDate = await SummaryService.LatestDateStmpAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, 1);
                int previousCount = await SummaryService.BirdCountForDateAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, previousDate);
                NewBirds = TodayObs - previousCount; 
            }

            LocationDailyObs = await SummaryService.GetLocationDailyObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate);
            
            var result = chartService.BuildLocationChart(LocationDailyObs);
            Series = result.Series;
            Labels = result.Labels;
            XAxes = result.XAxes;
            YAxes = result.YAxes;
            OnPropertyChanged(null);
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

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}