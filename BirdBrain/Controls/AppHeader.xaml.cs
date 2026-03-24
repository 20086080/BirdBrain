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
    void OnMenuTapped(object? sender, EventArgs e)                       // Open Menu 
    {
        HamburgerClicked?.Invoke(this, EventArgs.Empty);
    }

    async void OnCloseTapped(object? sender, EventArgs e)                // Exit the app
    {
        //TODO - Save SavedLocations and SavedBirds to json files before quitting
        Application.Current.Quit();   
    }

    async void OnMenuFavouriteTapped(object? sender, TappedEventArgs e)
    {
        if (App.State.LeftSelected)                                     // Location option is selected 
        {
            if (App.State.SelectedSavedLocation == null)
                return;
            if (App.State.SavedLocations.Contains(App.State.SelectedSavedLocation))
            {
                await ErrorService.Show(ErrorType.ReadytoGoLocationAlreadySaved);
            }
            else
            {
                App.State.SavedLocations.Add(App.State.SelectedSavedLocation);
                await ErrorService.Show(ErrorType.ReadytoGoSavedLocation);
            }
        }
        else
        {                                                           // Bird option is selected
            if (App.State.SelectedSavedBird == null)
                return;
            if (App.State.SavedBirds.Contains(App.State.SelectedSavedBird))
            {
                await ErrorService.Show(ErrorType.ReadytoGoBirdAlreadySaved);
            }
            else
            { 
                App.State.SavedBirds.Add(App.State.SelectedSavedBird);
                await ErrorService.Show(ErrorType.ReadytoGoSavedBird);
            }
        }
    }

    async void OnMenuRefreshTapped(object? sender, TappedEventArgs e)    //Get API Data 
    {
        await ErrorService.Show(ErrorType.GettingInformation);
        var refresh = new RefreshData();
        bool refreshed = await refresh.RefreshAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);
        
        if (!refreshed)
        {
            await ErrorService.Show(ErrorType.ApiPleaseWait);
        }
        else
        {
            await ErrorService.Show(ErrorType.RefreshSuccessful);
        }
    }
    void OnMenuSettingsTapped(object? sender, TappedEventArgs e)         // Call Settings Menu 
    {
        SettingsClicked?.Invoke(this, EventArgs.Empty);
    }
}