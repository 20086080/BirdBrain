using BirdBrain.Models;
using BirdBrain.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                //if (_selectedTheme != value)
                //{
                    _selectedTheme = value;
                    OnPropertyChanged(nameof(SelectedTheme));

                    if (value != null)
                    {
                        var resolvedTheme = ResolveThemeByMode(value.ThemeType!);
                        ThemeManager.ApplyTheme(resolvedTheme);
                    }
                //}
            }
        }

        public ThemeViewModel()
        {
            Themes = new ObservableCollection<AppThemeOption>(
                ThemeManager.GetAvailableThemes()
                .Select(t => ThemeManager.CreateThemeOption(t)));
            var currentTheme = ThemeManager.CurrentThemeType;

            SelectedTheme = Themes.FirstOrDefault(t => t.ThemeType == currentTheme)!;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public Type ResolveThemeByMode(Type baseThemeType)
        {
            var fullName = baseThemeType.FullName!;

            // Remove existing suffix if present
            var baseName = fullName
                .Replace("Dark", "")
                .Replace("Light", "");

            string finalName;

            if (App.State.DarkLightMode)
                finalName = baseName + "Dark";
            else
                finalName = baseName + "Light";

            var assemblyName = baseThemeType.Assembly.FullName;
            var qualifiedName = $"{finalName}, {assemblyName}";

            return Type.GetType(qualifiedName)!;
        }

        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    
    }
}