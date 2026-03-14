using BirdBrain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BirdBrain.Services
{
    public class AppState : INotifyPropertyChanged
    {
        private int _selectedBirdId;
        private int _selectedLocationId;

        //TODO remove below values once testing is finished
        //private string _selectedLocationName = "";
        //private string _city_ascii = "";
        //private string _country = "";
        //private string _selectedBirdCommonName = "";
        //private string _selectedBirdThumbnail = "";
        //private string _selectedLocationThumbnail = "";
        //private string _selectedLocationProfileImage = "";
        //private string _selectedBirdProfileImage = "";
        private string _selectedLocationName = "Kings Park";
        private string _city_ascii = "Perth";
        private string _country = "Australia";
        private string _selectedBirdCommonName = "Pied Stilt";
        private string _selectedBirdThumbnail = "currawong_t.png";
        private string _selectedLocationThumbnail = "kings_park_t.png";
        private string _selectedLocationProfileImage = "kings_park.png";
        private string _selectedBirdProfileImage = "currawong.png";

        private int _days = 30;
        private int _radius = 50;

        private int _totalSightings;
        private int _totalTypeOfBird;
        private List<TopBirds> _topBirds = new();
        public List<TopBirds> TopBirds
        {
            get => _topBirds;
            set
            {
                _topBirds = value;
                OnPropertyChanged();
            }
        }
        public List<LocationDailyObs> LocationDailyObs { get; set; } = new();

        //private double _lat;
        //private double _lng;

        //TODO remove below 2 values once testing is finished 
        private double _lat = -31.9617;
        private double _lng = 115.8420;

        public List<BirdObservation> Observations { get; set; } = new();

        public DatabaseService Database { get; set; }
        public int SelectedBirdId
        {
            get => _selectedBirdId;
            set
            {
                if (_selectedBirdId != value)
                {
                    _selectedBirdId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedLocationId
        {
            get => _selectedLocationId;
            set
            {
                if (_selectedLocationId != value)
                {
                    _selectedLocationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedLocationName
        {
            get => _selectedLocationName;
            set
            {
                if (_selectedLocationName != value)
                {
                    _selectedLocationName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string City_Ascii
        {
            get => _city_ascii;
            set
            {
                if (_city_ascii != value)
                {
                    _city_ascii = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedBirdCommonName
        {
            get => _selectedBirdCommonName;
            set
            {
                if (_selectedBirdCommonName != value)
                {
                    _selectedBirdCommonName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedBirdThumbnail
        {
            get => _selectedBirdThumbnail;
            set
            {
                if (_selectedBirdThumbnail != value)
                {
                    _selectedBirdThumbnail = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedLocationThumbnail
        {
            get => _selectedLocationThumbnail;
            set
            {
                if (_selectedLocationThumbnail != value)
                {
                    _selectedLocationThumbnail = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedLocationProfileImage
        {
            get => _selectedLocationProfileImage;
            set
            {
                if (_selectedLocationProfileImage != value)
                {
                    _selectedLocationProfileImage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedBirdProfileImage
        {
            get => _selectedBirdProfileImage;
            set
            {
                if (_selectedBirdProfileImage != value)
                {
                    _selectedBirdProfileImage = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalSightings
        {
            get => _totalSightings;
            set
            {
                if (_totalSightings != value)
                {
                    _totalSightings = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalTypeOfBird
        {
            get => _totalTypeOfBird;
            set
            {
                if (_totalTypeOfBird != value)
                {
                    _totalTypeOfBird = value;
                    OnPropertyChanged();
                }
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
                }
            }
        }

        public double Lng
        {
            get => _lng;
            set
            {
                if (_lng != value)
                {
                    _lng = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Lat
        {
            get => _lat;
            set
            {
                if (_lat != value)
                {
                    _lat = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
