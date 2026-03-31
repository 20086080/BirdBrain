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

    public int TotalBirdSightings { get; set; }
    public double PercTotalSightings { get; set; }
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
        LocationTab.Style = (Style)Application.Current!.Resources["SegmentUnselectedStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
        try
        {
            TotalBirdSightings = await SummaryService.GetTotalBirdCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.SelectedSavedBird.CommonName!, DateTime.UtcNow.AddDays(-App.State.Days));
            if (TotalBirdSightings <= 0)
            {
                await ErrorService.Show(ErrorType.NoBirdsFound);
            }

          //  App.State.TotalSightings = await SummaryService.GetTotalLocationCountAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.CutoffDate);
          //  App.State.PercTotalBirdSightings = 100 * ((double)App.State.TotalBirdSightings / App.State.TotalSightings) ; 
            BirdDailyObs = await SummaryService.GetBirdDailyObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.SelectedSavedBird.CommonName!, DateTime.UtcNow.AddDays(-App.State.Days));
            BirdTimeObs = await SummaryService.GetBirdTimeObsAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, App.State.SelectedSavedBird.CommonName!, DateTime.UtcNow.AddDays(-App.State.Days));
            //App.State.BuildChartBird(App.State.BirdDailyObs);
            //App.State.BuildChartBirdTime(App.State.BirdTimeObs);
            App.State.LeftSelected = false;
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