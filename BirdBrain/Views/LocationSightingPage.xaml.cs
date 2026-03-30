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

    public List<LocationSightingSummary> SightingsCount { get; set; }
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
        base.OnAppearing();
        _ = LoadDataAsync();

    }

    private async Task LoadDataAsync()
    {
        try
        {
            Double Lat = App.State.SelectedSavedLocation.Lat;
            Double Lng = App.State.SelectedSavedLocation.Lng;
            App.State.LeftSelected = true;
            
            await App.State.Database.InitAsync();

            CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
            var (DateToday, previousDate) = await SummaryService.GetLatestTwoDatesAsync(Lat, Lng);

            var resultSummary = await SummaryService.GetLocationCountsAsync(Lat, Lng, CutoffDate, DateToday, previousDate);

            TotalSightings = resultSummary.FirstOrDefault()?.TotalSightings ?? 0;
            if (TotalSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoLocationDataFound);
                await Shell.Current.GoToAsync("//IntroPage");
                return;
            }
            else
            {
                var TopBirdsTask = SummaryService.GetTop5BirdCountAsync(Lat, Lng, CutoffDate, DateToday);
                var LocationDailyObsTask = SummaryService.GetLocationDailyObsAsync(Lat, Lng, CutoffDate);

                int NumberDays = resultSummary.FirstOrDefault()?.TotalDays ?? 0;
                TotalTypeOfBird = resultSummary.FirstOrDefault()?.TotalBirds ?? 0;
                TodayObs = resultSummary.FirstOrDefault()?.TodayBirds ?? 0;
                int previousCount = resultSummary.FirstOrDefault()?.PreviousDayBirds ?? 0;

                AverageObs = TotalSightings / NumberDays;
                NewBirds = TodayObs - previousCount;

                await Task.WhenAll(TopBirdsTask, LocationDailyObsTask);
                TopBirds = await TopBirdsTask;
                LocationDailyObs = await LocationDailyObsTask;

                var chartService = new ChartService();
                var result = chartService.BuildLocationChart(LocationDailyObs);
                Series = result.Series;
                Labels = result.Labels;
                XAxes = result.XAxes;
                YAxes = result.YAxes;
                OnPropertyChanged(null);
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

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}