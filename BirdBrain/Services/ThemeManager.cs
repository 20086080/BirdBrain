using BirdBrain.Resources.Themes;
namespace BirdBrain.Services;

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
}