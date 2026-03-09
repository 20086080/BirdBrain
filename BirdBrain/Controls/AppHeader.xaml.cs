using BirdBrain.Views;
using BirdBrain.Services;
namespace BirdBrain.Controls;

public partial class AppHeader : ContentView
{
    public event EventHandler HamburgerClicked;
    public event EventHandler SettingsClicked;
    public AppHeader()
	{
		InitializeComponent();
	}
    void OnMenuTapped(object sender, EventArgs e)
    {
        HamburgerClicked?.Invoke(this, EventArgs.Empty);
    }

    async void OnCloseTapped(object sender, EventArgs e)
    {
            // Exit the app
            Application.Current.Quit();
        
    }

    private void OnMenuFavouriteTapped(object sender, TappedEventArgs e)
    {

    }

    async void OnMenuRefreshTapped(object sender, TappedEventArgs e)
    {
        var refresh = new RefreshData();
        App.State.Lat = -31.9617;
        App.State.Lng = 115.8420;
        bool refreshed =
            await refresh.RefreshAsync(App.State.Lat, App.State.Lng);

        if (!refreshed)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Please Wait",
                "You must wait 60 minutes before refreshing again.",
                "OK");
        }
    }
    void OnMenuSettingsTapped(object sender, TappedEventArgs e)
    {
        SettingsClicked?.Invoke(this, EventArgs.Empty);
    }
}