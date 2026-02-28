namespace BirdBrain.Controls;

public partial class AppHeader : ContentView
{
	public AppHeader()
	{
		InitializeComponent();
	}
    async void OnMenuTapped(object sender, EventArgs e)
    {
        // If using Shell Flyout
        Shell.Current.FlyoutIsPresented = true;
    }

    async void OnCloseTapped(object sender, EventArgs e)
    {
        // Go back one page
        await Shell.Current.GoToAsync("..");
    }

    private void OnMenuFavouriteTapped(object sender, TappedEventArgs e)
    {

    }

    private void OnMenuRefreshTapped(object sender, TappedEventArgs e)
    {

    }
    private void OnMenuSettingsTapped(object sender, TappedEventArgs e)
    {

    }
}