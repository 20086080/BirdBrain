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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

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

        public ObservableCollection<LatLng> GlobalLocations { get; set; } = new();

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

        private bool _isLoadingGlobalLocations;

        public async Task LoadGlobalLocationsAsync()
        {
            if (_isLoadingGlobalLocations) return;

            _isLoadingGlobalLocations = true;

            using var stream = await FileSystem.OpenAppPackageFileAsync("LatLngSeedData.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            int count = 0;

            await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<LatLng>(stream, options))
            {
                if (item == null) continue;

                // First item immediately (fast UI response)
                if (count == 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                        GlobalLocations.Add(item));
                }
                else
                {
                    // Throttle UI updates slightly (every 20 items)
                    if (count % 20 == 0)
                    {
                        await Task.Yield(); // give UI breathing room
                    }

                    MainThread.BeginInvokeOnMainThread(() =>
                        GlobalLocations.Add(item));
                }

                count++;
            }
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            _initialized = true;

            var birdsTask = _jsonReader.ReadListAsync<Bird>("BirdsSeedData.json");
            var locationsTask = _jsonReader.ReadListAsync<SavedLocation>("LocationSeedData.json");
            //var globalTask = _jsonReader.ReadListAsync<LatLng>("LatLngSeedData.json");

            await Task.WhenAll(birdsTask, locationsTask);

            SavedBirds = await birdsTask;
            SavedLocations = await locationsTask;
            //GlobalLocations = await globalTask;
            _ = LoadGlobalLocationsAsync();
            SelectedSavedBird = SavedBirds.Count > 0 ? SavedBirds[0] : null;
            SelectedSavedLocation = SavedLocations.Count > 0 ? SavedLocations[0] : null;

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

        

        public ISeries[] PieSeries { get; set; }

        

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
