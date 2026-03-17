using BirdBrain.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Kernel;
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
        //private double _lat;
        //private double _lng;

        private double _lat = -31.9617;
        private double _lng = 115.8420;
        private string _selectedLocationName = "Kings Park";
        private string _city_ascii = "Perth";
        private string _country = "Australia";
        private string _selectedBirdCommonName = "Yellow-billed Spoonbill";
        private string _selectedBirdThumbnail = "currawong_t.png";
        private string _selectedLocationThumbnail = "kings_park_t.png";
        private string _selectedLocationProfileImage = "kings_park.png";
        private string _selectedBirdProfileImage = "currawong.png";

        private int _days = 30;
        private int _radius = 10;
        private bool _leftSelected = true;
        private int _totalSightings;
        private int _totalTypeOfBird;
        private int _totalBirdSightings;
        private double _PercTotalSightings;
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
        public List<BirdDailyObs> BirdDailyObs { get; set; } = new();

        public List<BirdTimeObs> BirdTimeObs { get; set; } = new();
        public List<BirdObservation> Observations { get; set; } = new();

        public DatabaseService Database { get; set; }

        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        private List<string> _labels;
        public List<string> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        private Axis[] _xAxes =
        {
            new Axis
            {
                LabelsRotation = 20
            }
        };

        public Axis[] XAxes
        {
            get => _xAxes;
            set
            {
                _xAxes = value;
                OnPropertyChanged();
            }
        }
        private Axis[] _yAxes =
        {
            new Axis
            {
                LabelsRotation = 20
            }
        };
        public Axis[] YAxes
        {
            get => _yAxes;
            set
            {
                _yAxes = value;
                OnPropertyChanged();
            }
        }

        public void BuildChart(List<LocationDailyObs> data)
        {
            var color = (Color)Application.Current.Resources["TextPrimary"];
            var skColor = new SKColor(
                (byte)(color.Red * 255),
                (byte)(color.Green * 255),
                (byte)(color.Blue * 255),
                (byte)(color.Alpha * 255));
            var maxSightings = data.Any() ? data.Max(x => x.Sightings) : 0;
            Series = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = data.Select(x => x.Sightings).ToList(),
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(skColor)
                    {
                        StrokeThickness = 3
                    },
                    Fill = null
                }
            };
            Labels = data
                .Select(x => DateTime.Parse(x.ObsDt).ToString("dd/M"))
                .ToList();
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = Labels,
                    LabelsRotation = 20,
                    MinStep = 1,
                    SeparatorsPaint = null,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor)

                }
            };
            YAxes = new Axis[]
            {
                new Axis
                {
                    SeparatorsPaint = null,
                    MinStep = 5,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor),
                    MaxLimit = maxSightings + 5
                }
            };
        }

        private ISeries[] _Birdseries;
        public ISeries[] BirdSeries
        {
            get => _Birdseries;
            set
            {
                _Birdseries = value;
                OnPropertyChanged();
            }
        }

        private List<string> _Birdlabels;
        public List<string> BirdLabels
        {
            get => _Birdlabels;
            set
            {
                _Birdlabels = value;
                OnPropertyChanged();
            }
        }

        private Axis[] _BirdxAxes =
        {
            new Axis
            {
                LabelsRotation = 20
            }
        };

        public Axis[] BirdXAxes
        {
            get => _BirdxAxes;
            set
            {
                _BirdxAxes = value;
                OnPropertyChanged();
            }
        }
        private Axis[] _BirdyAxes =
        {
            new Axis
            {
                LabelsRotation = 20
            }
        };
        public Axis[] BirdYAxes
        {
            get => _BirdyAxes;
            set
            {
                _BirdyAxes = value;
                OnPropertyChanged();
            }
        }

        public void BuildChartBird(List<BirdDailyObs> data)
        {
            var color = (Color)Application.Current.Resources["TextPrimary"];
            var skColor = new SKColor(
                (byte)(color.Red * 255),
                (byte)(color.Green * 255),
                (byte)(color.Blue * 255),
                (byte)(color.Alpha * 255));
            var maxSightings = data.Max(x => x.Sightings);
            BirdSeries = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = data.Select(x => x.Sightings).ToList(),
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(skColor)
                    {
                        StrokeThickness = 3
                    },
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
                    Labels = Labels,
                    LabelsRotation = 20,
                    MinStep = 1,
                    SeparatorsPaint = null,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor)

                }
            };
            BirdYAxes = new Axis[]
            {
                new Axis
                {
                    SeparatorsPaint = null,
                    MinStep = 5,
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
                (byte)(color.Red * 255),
                (byte)(color.Green * 255),
                (byte)(color.Blue * 255),
                (byte)(color.Alpha * 255));
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

        public bool LeftSelected
        {
            get => _leftSelected;
            set
            {
                if (_leftSelected != value)
                {
                    _leftSelected = value;
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

        public int TotalBirdSightings
        {
            get => _totalBirdSightings;
            set
            {
                if (_totalBirdSightings != value)
                {
                    _totalBirdSightings = value;
                    OnPropertyChanged();
                }
            }
        }

        public double PercTotalBirdSightings
        {
            get => _PercTotalSightings;
            set
            {
                if (_PercTotalSightings != value)
                {
                    _PercTotalSightings = value;
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
