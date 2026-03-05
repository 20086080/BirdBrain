using BirdBrain.Resources.Themes;
namespace BirdBrain.Services;
using System.Text.RegularExpressions;

public static class ThemeManager
{
    private const string ThemePrefKey = "SelectedTheme";
    public static void ApplyTheme(Type themeDictionaryType, bool persist = true)
    {
        if (!typeof(ResourceDictionary).IsAssignableFrom(themeDictionaryType))
            throw new ArgumentException("Theme must be a ResourceDictionary");

        var dictionaries = Application.Current.Resources.MergedDictionaries;

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

    public static List<Type> GetAvailableThemes()
    {
        return typeof(BlueTealDark).Assembly
            .GetTypes()
            .Where(t =>
                t.IsSubclassOf(typeof(ResourceDictionary)) &&
                t.Namespace == "BirdBrain.Resources.Themes")
            .ToList();
    }
}