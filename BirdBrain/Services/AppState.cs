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
        private string _selectedLocationName = "";
        private int _days = 30;
        private double _radius = 5;

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

        public double Radius
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
