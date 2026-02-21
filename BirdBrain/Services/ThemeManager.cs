using BirdBrain.Resources.Themes;

namespace BirdBrain.Services;

public static class ThemeManager
{
    public static void ApplyTheme(string themeName)
    {
        var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        mergedDictionaries.Clear();

        switch (themeName)
        {
            case "CoralNavy":
                mergedDictionaries.Add(new CoralNavyDark());
                break;

            case "BlueTeal":
                mergedDictionaries.Add(new BlueTealDark());
                break;
        }
    }
}
