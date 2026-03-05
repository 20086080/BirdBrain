using BirdBrain.Models;
using BirdBrain.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace BirdBrain.ViewModels
{
    public class ThemeViewModel
    {
        public ObservableCollection<AppThemeOption> Themes { get; }

        private AppThemeOption _selectedTheme;
        public AppThemeOption SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;

                    if (value != null)
                        ThemeManager.ApplyTheme(value.ThemeType);
                }
            }
        }

        public ThemeViewModel()
        {
            Themes = new ObservableCollection<AppThemeOption>(
                ThemeManager.GetAvailableThemes()
                    .Select(t => new AppThemeOption
                    {
                        Name = ThemeManager.FormatThemeName(t),
                        ThemeType = t
                    }));
        }
    }
}