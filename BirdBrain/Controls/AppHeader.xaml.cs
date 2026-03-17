using BirdBrain.Models;
using BirdBrain.Services;
using BirdBrain.Views;
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
        await ErrorService.Show(ErrorType.GettingInformation);
        var refresh = new RefreshData();
        bool refreshed =
            await refresh.RefreshAsync(App.State.Lat, App.State.Lng);
        
        if (!refreshed)
        {
            await ErrorService.Show(ErrorType.ApiPleaseWait);
        }
        else
        {
            await ErrorService.Show(ErrorType.RefreshSuccessful);
        }
    }
    void OnMenuSettingsTapped(object sender, TappedEventArgs e)
    {
        SettingsClicked?.Invoke(this, EventArgs.Empty);
    }
}