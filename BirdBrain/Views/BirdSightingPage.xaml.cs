using BirdBrain.Models;
using BirdBrain.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.ComponentModel;
using System.Diagnostics;

namespace BirdBrain.Views;

public partial class BirdSightingPage : BasePage, INotifyPropertyChanged
{
    public AppState State => App.State;

    int BirdSightings { get; set; }
    int TotalSightings { get; set; }
    public double PercBirdSightings { get; set; }
    public string? CutoffDate { get; set; }

    public List<BirdDailyObs> BirdDailyObs { get; set; } = new();
    public List<BirdTimeObs> BirdTimeObs { get; set; } = new();
    public List<BirdObservation> Observations { get; set; } = new();

    //private readonly SummaryService _summaryService;

    // BIRD SERIES
    private ISeries[] _birdSeries = Array.Empty<ISeries>();
    public ISeries[] BirdSeries
    {
        get => _birdSeries;
        set
        {
            _birdSeries = value ?? Array.Empty<ISeries>();
            OnPropertyChanged(nameof(BirdSeries));
        }
    }

    // BIRD LABELS
    private List<string> _birdLabels = new();
    public List<string> BirdLabels
    {
        get => _birdLabels;
        set
        {
            _birdLabels = value ?? new List<string>();
            OnPropertyChanged(nameof(BirdLabels));
        }
    }

    // BIRD X AXES
    private Axis[] _birdXAxes =
    {
    new Axis { LabelsRotation = 20 }
};

    public Axis[] BirdXAxes
    {
        get => _birdXAxes;
        set
        {
            _birdXAxes = value ?? new Axis[] { new Axis { LabelsRotation = 20 } };
            OnPropertyChanged(nameof(BirdXAxes));
        }
    }

    // BIRD Y AXES
    private Axis[] _birdYAxes =
    {
    new Axis { LabelsRotation = 20 }
};

    public Axis[] BirdYAxes
    {
        get => _birdYAxes;
        set
        {
            _birdYAxes = value ?? new Axis[] { new Axis { LabelsRotation = 20 } };
            OnPropertyChanged(nameof(BirdYAxes));
        }
    }

    // PIE SERIES (IMPORTANT: make it bindable)
    private ISeries[] _pieSeries = Array.Empty<ISeries>();
    public ISeries[] PieSeries
    {
        get => _pieSeries;
        set
        {
            _pieSeries = value ?? Array.Empty<ISeries>();
            OnPropertyChanged(nameof(PieSeries));
        }
    }
    public BirdSightingPage()
	{
        InitializeComponent();
        if (App.State == null)
            throw new Exception("App.State is NULL");
        //_summaryService = summaryService;
        App.State.LeftSelected = false;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _ = LoadDataAsync();

    }

    private async Task LoadDataAsync()
    {
        LocationTab.Style = (Style)Application.Current!.Resources["SegmentUnselectedStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;

        try
        {
            Double Lat = App.State.SelectedSavedLocation.Lat;
            Double Lng = App.State.SelectedSavedLocation.Lng;
            string commonName = App.State.SelectedSavedBird.CommonName!;

            CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
            var (DateToday, previousDate) = await SummaryService.GetLatestTwoDatesAsync(Lat, Lng);

            var resultSummary = await SummaryService.GetTotalBirdCountAsync(Lat, Lng, commonName, CutoffDate);

            BirdSightings = resultSummary.FirstOrDefault()?.BirdSightings ?? 0;
            TotalSightings = resultSummary.FirstOrDefault()?.TotalSightings ?? 0;

            if (BirdSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoBirdsFound);
            }
            int NumberDays = resultSummary.FirstOrDefault()?.TotalDays ?? 0;
            BirdDailyObs = await SummaryService.GetBirdDailyObsAsync(Lat, Lng, commonName, CutoffDate);
            BirdTimeObs = await SummaryService.GetBirdTimeObsAsync(Lat, Lng, commonName, CutoffDate);
            int PercBirdSightings = TotalSightings == 0 ? 0
                                    : (int)(100.0 * BirdSightings / TotalSightings);

            var chartService = new ChartService();
            var result = chartService.BuildBirdChart(BirdDailyObs);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BirdSeries = result.Series;
                BirdLabels = result.Labels;
                BirdXAxes = result.XAxes;
                BirdYAxes = result.YAxes;
            });
            OnPropertyChanged(string.Empty);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex}");
            await ErrorService.Show(ErrorType.ErrorFound);
        }
    }

    async void LocationTapped(object? sender, EventArgs e)
    {
        //LocationTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
        //BirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        //LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        //BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationSightingPage));
    }

    async void BirdTapped(object? sender, EventArgs e)
    {
        //BirdTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
        //LocationTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        //BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        //LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        //App.State.LeftSelected = false;
        //await Shell.Current.GoToAsync(nameof(BirdSightingPage));    
    }
}