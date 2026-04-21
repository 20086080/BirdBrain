using BirdBrain.Resources.Themes;
namespace BirdBrain.Services;

using System.Reflection;
using BirdBrain.Models;
using System.Text.RegularExpressions;

public static class ThemeManager
{
    private const string ThemePrefKey = "SelectedTheme";
    public static void ApplyTheme(Type themeDictionaryType, bool persist = true)
    {
        if (!typeof(ResourceDictionary).IsAssignableFrom(themeDictionaryType))
            throw new ArgumentException("Theme must be a ResourceDictionary");

        var dictionaries = Application.Current!.Resources.MergedDictionaries;

        dictionaries.Clear();
        dictionaries.Add((ResourceDictionary)Activator.CreateInstance(themeDictionaryType)!);

        if (persist)
            Preferences.Set(ThemePrefKey, themeDictionaryType.AssemblyQualifiedName);
    }

    public static void LoadSavedTheme(Type fallbackTheme)
    {
        var saved = Preferences.Get(ThemePrefKey, null);

        var themeType = saved != null
            ? Type.GetType(saved)
            : fallbackTheme;

        ApplyTheme(themeType ?? fallbackTheme, persist: false);
    }

    public static string FormatThemeName(Type themeType)
    {
        return Regex.Replace(themeType.Name, "(\\B[A-Z])", " $1");
    }

    public static IEnumerable<Type> GetAvailableThemes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.IsSubclassOf(typeof(ResourceDictionary)) &&
                t.GetCustomAttribute<SelectableThemeAttribute>() != null);
    }

    public static AppThemeOption CreateThemeOption(Type themeType)
    {
        var theme = (ResourceDictionary)Activator.CreateInstance(themeType)!;

        Color GetColor(string key)
        {
            if (theme.TryGetValue(key, out var value) && value is Color color)
                return color;

            return Colors.Transparent;
        }

        return new AppThemeOption
        {
            Name = FormatThemeName(themeType),
            ThemeType = themeType,

            PreviewPrimary = GetColor("Primary"),
            PreviewAccent = GetColor("Accent"),
            PreviewBackground = GetColor("Background"),
            PreviewSurface = GetColor("Surface"),
            PreviewTextPrimary = GetColor("TextPrimary"),
            PreviewTextOnPrimary = GetColor("TextOnPrimary")
        };
    }
}