using BirdBrain.Models;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Text.Json;

namespace BirdBrain.Services
{
    public class AppState : INotifyPropertyChanged
    {
        public readonly int MaxDays = 30;
        public readonly int MinDays = 1;
        public readonly int MaxRadius = 50;
        public readonly int MinRadius = 1;

        private JsonFileReader _jsonReader = new JsonFileReader();
        private SavedLocation _selectedSavedLocation { get; set; } = new();
        private Bird _selectedSavedBird { get; set; } = new();

        private int _days = 30;
        private int _radius = 50;

        private bool _leftSelected = true;
        private bool _initialized;

        public DatabaseService Database { get; set; }

        public List<Bird> SavedBirds { get; set; } = new List<Bird>();
        public List<SavedLocation> SavedLocations { get; set; } = new List<SavedLocation>();

        public List<BirdObservation> Observations { get; set; } = new();
        public ObservableCollection<LatLng> GlobalLocations { get; set; } = new();
        
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

            await Task.WhenAll(birdsTask, locationsTask);

            SavedBirds = await birdsTask;
            SavedLocations = await locationsTask;
            
            _ = LoadGlobalLocationsAsync();
            SelectedSavedBird = SavedBirds.Count > 0 ? SavedBirds[0] : null;
            SelectedSavedLocation = SavedLocations.Count > 0 ? SavedLocations[0] : null;
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

        public int Days
        {
            get => _days;
            set
            {
                if (_days != value)
                { 
                    _days = value; 
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DaysSlider));
                }
            }
        }

        public double DaysSlider
        {
            get => Days;  // slider reads from Days

            set
            {
                int newValue = (int)Math.Round(value);

                if (Days != newValue)
                {
                    Days = newValue; // update real value
                }
            }
        }

        public double RadiusSlider
        {
            get => Radius;  // slider reads from Radius

            set
            {
                int newValue = (int)Math.Round(value);

                if (Radius != newValue)
                {
                    Radius = newValue; // update real value
                }
            }
        }

        public int Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                { 
                    _radius = value; 
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(RadiusSlider));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
