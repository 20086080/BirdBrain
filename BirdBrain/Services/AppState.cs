//using AndroidX.Startup;
using BirdBrain.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BirdBrain.Services
{
    public class AppState : INotifyPropertyChanged
    {
        
        private JsonFileReader _jsonReader = new JsonFileReader();

        private SavedLocation _selectedSavedLocation { get; set; } = new();

        private Bird _selectedSavedBird { get; set; } = new();

        private int _maxDays = 30;
        private int _minDays = 1;
        private int _maxRadius = 50;
        private int _minRadius = 1;

        private int _days = 30;
        private int _radius = 50;
        private bool _leftSelected = true;

        private int _totalSightings;
        private int _todayObs;
        private int _averageObs;
        private int _newBirds;
        private string? _cutoffDate;

        private int _totalTypeOfBird;
        private int _totalBirdSightings;
        private double _PercTotalSightings;

        private bool _initialized;
        public List<LocationDailyObs> LocationDailyObs { get; set; } = new();
        public List<BirdDailyObs> BirdDailyObs { get; set; } = new();
        public List<BirdTimeObs> BirdTimeObs { get; set; } = new();
        public List<BirdObservation> Observations { get; set; } = new();

        public DatabaseService Database { get; set; }

        public List<Bird> SavedBirds { get; set; } = new List<Bird>();
        public List<SavedLocation> SavedLocations { get; set; } = new List<SavedLocation>();

        public List<LatLng> GlobalLocations { get; set; } = new List<LatLng>();

        private List<TopBirds> _topBirds = new();


        public List<TopBirds> TopBirds
        {
            get => _topBirds;
            set { _topBirds = value; OnPropertyChanged(); }
        }

        public SavedLocation SelectedSavedLocation
        {
            get => _selectedSavedLocation;
            set
            {
                if (_selectedSavedLocation != value)
                {
                    _selectedSavedLocation = value; OnPropertyChanged();
                }
            }
        }

        public Bird SelectedSavedBird
        {
            get => _selectedSavedBird;
            set
            {
                if (_selectedSavedBird != value)
                {
                    _selectedSavedBird = value; OnPropertyChanged();
                }
            }
        }

        public void SetJsonReader(JsonFileReader reader)
        {
            _jsonReader = reader;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            _initialized = true;

            var birdsTask = _jsonReader.ReadListAsync<Bird>("BirdsSeedData.json");
            var locationsTask = _jsonReader.ReadListAsync<SavedLocation>("LocationSeedData.json");
            var globalTask = _jsonReader.ReadListAsync<LatLng>("LatLngSeedData.json");

            await Task.WhenAll(birdsTask, locationsTask, globalTask);

            SavedBirds = await birdsTask;
            SavedLocations = await locationsTask;
            GlobalLocations = await globalTask;

            // Now set selected values
            SelectedSavedBird = SavedBirds.Count > 0 ? SavedBirds[0] : null;
            SelectedSavedLocation = SavedLocations.Count > 0 ? SavedLocations[0] : null;

            //Get Saved (Favourite) Birds List
            //if (SavedBirds == null || SavedBirds.Count == 0)
            //{
            //    SavedBirds = await _jsonReader.ReadListAsync<Bird>("BirdsSeedData.json");

            //    SelectedSavedBird = SavedBirds.Count > 0 ? SavedBirds[0] : null;
            //}

            //Get Saved (Favourite) Location List
            //if (SavedLocations == null || SavedLocations.Count == 0)
            //{
            //    SavedLocations = await _jsonReader.ReadListAsync<SavedLocation>("LocationSeedData.json");

            //    SelectedSavedLocation = SavedLocations.Count > 0 ? SavedLocations[0] : null;
            //}


            //Get Global Lat Lng List
            //if (GlobalLocations == null || GlobalLocations.Count == 0)
            //{
            //    GlobalLocations = await _jsonReader.ReadListAsync<LatLng>("LatLngSeedData.json");
            //}
        }

        private ISeries[] _series = Array.Empty<ISeries>();
        public ISeries[] Series
        {
            get => _series;
            set { _series = value ?? Array.Empty<ISeries>(); OnPropertyChanged(); }
        }

        private List<string> _labels;
        public List<string> Labels
        {
            get => _labels;
            set {_labels = value; OnPropertyChanged(); }
        }

        private Axis[] _xAxes =
        {
            new Axis
                { LabelsRotation = 20 }
        };

        public Axis[] XAxes
        {
            get => _xAxes;
            set { _xAxes = value; OnPropertyChanged(); }
        }

        private Axis[] _yAxes =
        {
            new Axis
                { LabelsRotation = 20 }
        };
        public Axis[] YAxes
        {
            get => _yAxes;
            set { _yAxes = value; OnPropertyChanged(); }
        }

        public void BuildChart(List<LocationDailyObs> data)
        {
            if (data == null || !data.Any())
            {
                Series = Array.Empty<ISeries>();
                Labels = new List<string>();
                return;
            }
            var color = (Color)Application.Current!.Resources["TextPrimary"];
            var skColor = new SKColor(
                (byte)(color.Red * 255), (byte)(color.Green * 255),
                (byte)(color.Blue * 255), (byte)(color.Alpha * 255));
            var maxSightings = data.Any() ? data.Max(x => x.Sightings) : 0;
            Series = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = data.Select(x => x.Sightings).ToList(),
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(skColor)
                        { StrokeThickness = 3 },
                        Fill = null
                }
            };
            Labels = data
                .Select(x => DateTime.TryParse(x.ObsDt, out var d)
                ? d.ToString("dd/M")
                : "")
                .ToList();
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = Labels, LabelsRotation = 20,
                    MinStep = 1, SeparatorsPaint = null,
                    TextSize = 11, 
                    LabelsPaint = new SolidColorPaint(skColor)
                }
            };
            YAxes = new Axis[]
            {
                new Axis
                {
                    SeparatorsPaint = null, MinStep = 5,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor),
                    MaxLimit = maxSightings + 5
                }
            };
        }

        private ISeries[] _Birdseries = Array.Empty<ISeries>();
        public ISeries[] BirdSeries
        {
            get => _Birdseries;
            set { _Birdseries = value ?? Array.Empty<ISeries>(); OnPropertyChanged(); }
        }

        private List<string> _Birdlabels;
        public List<string> BirdLabels
        {
            get => _Birdlabels;
            set { _Birdlabels = value; OnPropertyChanged(); }
        }

        private Axis[] _BirdxAxes =
        {
            new Axis
                { LabelsRotation = 20 }
        };

        public Axis[] BirdXAxes
        {
            get => _BirdxAxes;
            set { _BirdxAxes = value; OnPropertyChanged(); }
        }

        private Axis[] _BirdyAxes =
        {
            new Axis
                { LabelsRotation = 20 }
        };
        public Axis[] BirdYAxes
        {
            get => _BirdyAxes;
            set { _BirdyAxes = value; OnPropertyChanged(); }
        }

        public void BuildChartBird(List<BirdDailyObs> data)
        {
            if (data == null || !data.Any())
            {
                BirdSeries = Array.Empty<ISeries>();
                BirdLabels = new List<string>();
                return;
            }
            var color = (Color)Application.Current!.Resources["TextPrimary"];
            var skColor = new SKColor(
                (byte)(color.Red * 255), (byte)(color.Green * 255),
                (byte)(color.Blue * 255), (byte)(color.Alpha * 255));
            var maxSightings = data.Max(x => x.Sightings);
            BirdSeries = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = data.Select(x => x.Sightings).ToList(),
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(skColor)
                        { StrokeThickness = 3 },
                        Fill = null
                }
            };
            BirdLabels = data
                .Select(x => DateTime.Parse(x.ObsDt).ToString("dd/M"))
                .ToList();
            BirdXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = Labels, LabelsRotation = 20,
                    MinStep = 1, SeparatorsPaint = null,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor)
                }
            };
            BirdYAxes = new Axis[]
            {
                new Axis
                {
                    SeparatorsPaint = null, MinStep = 5,
                    TextSize = 11, 
                    LabelsPaint = new SolidColorPaint(skColor),
                    MaxLimit = maxSightings + 5
                }
            };
        }

        public ISeries[] PieSeries { get; set; }

        public void BuildChartBirdTime(List<BirdTimeObs> data)
        {
            var color = (Color)Application.Current.Resources["TextPrimary"];
            var skColor = new SKColor(
                (byte)(color.Red * 255), (byte)(color.Green * 255),
                (byte)(color.Blue * 255), (byte)(color.Alpha * 255));
            if (data == null || !data.Any())
            {
                PieSeries = Array.Empty<ISeries>();
                return;
            }
            var topTimes = data
                .GroupBy(x => x.ObsDt)
                .Select(g => new
                {
                    Time = g.Key,
                    Total = g.Sum(x => x.Sightings)
            })
            .OrderByDescending(x => x.Total)
            .Take(6)
            .ToList();

            PieSeries = topTimes
                .Select(x => new PieSeries<double>
                {
                    Values = new double[] { x.Total },
                    Name = DateTime.Parse(x.Time).ToString("HH:mm"),
                    InnerRadius = 40,
                    DataLabelsSize = 14,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => ((double)point.Model!).ToString("N0"),
                    Stroke = new SolidColorPaint(skColor, 1)
                })
                .Cast<ISeries>()
                .ToArray();
            OnPropertyChanged(nameof(PieSeries));
        }

        public bool LeftSelected
        {
            get => _leftSelected;
            set
            {
                if (_leftSelected != value)
                    { _leftSelected = value; OnPropertyChanged(); }
            }
        }

        public int TotalSightings
        {
            get => _totalSightings;
            set
            {
                if (_totalSightings != value)
                    { _totalSightings = value; OnPropertyChanged(); }
            }
        }

        public int TodayObs
        {
            get => _todayObs;
            set
            {
                if (_todayObs != value)
                { _todayObs = value; OnPropertyChanged(); }
            }
        }

        public int AverageObs
        {
            get => _averageObs;
            set
            {
                if (_averageObs != value)
                { _averageObs = value; OnPropertyChanged(); }
            }
        }

        public int NewBirds
        {
            get => _newBirds;
            set
            {
                if (_newBirds != value)
                { _newBirds = value; OnPropertyChanged(); }
            }
        }

        public int MaxRadius
        {
            get => _maxRadius;
            set
            {
                if (_maxRadius != value)
                { _maxRadius = value; OnPropertyChanged(); }
            }
        }

        public int MinRadius
        {
            get => _minRadius;
            set
            {
                if (_minRadius != value)
                { _minRadius = value; OnPropertyChanged(); }
            }
        }


        public int MaxDays
        {
            get => _maxDays;
            set
            {
                if (_maxDays != value)
                { _maxDays = value; OnPropertyChanged(); }
            }
        }

        public int MinDays
        {
            get => _minDays;
            set
            {
                if (_minDays != value)
                { _minDays = value; OnPropertyChanged(); }
            }
        }

        public int TotalBirdSightings
        {
            get => _totalBirdSightings;
            set
            {
                if (_totalBirdSightings != value)
                    { _totalBirdSightings = value; OnPropertyChanged(); }
            }
        }

        public double PercTotalBirdSightings
        {
            get => _PercTotalSightings;
            set
            {
                if (_PercTotalSightings != value)
                    { _PercTotalSightings = value; OnPropertyChanged(); }
            }
        }

        public int TotalTypeOfBird
        {
            get => _totalTypeOfBird;
            set
            {
                if (_totalTypeOfBird != value)
                    { _totalTypeOfBird = value; OnPropertyChanged(); }
            }
        }

        public int Days
        {
            get => _days;
            set
            {
                if (_days != value)
                    { _days = value; OnPropertyChanged(); }
            }
        }

        public string CutoffDate
        {
            get => _cutoffDate;
            set
            {
                if (_cutoffDate != value)
                { _cutoffDate = value; OnPropertyChanged(); }
            }
        }
        public int Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                    { _radius = value; OnPropertyChanged(); }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
