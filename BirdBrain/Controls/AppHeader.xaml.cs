namespace BirdBrain.Controls;

public partial class AppHeader : ContentView
{
    public event EventHandler HamburgerClicked;
    public AppHeader()
	{
		InitializeComponent();
	}
    async void OnMenuTapped(object sender, EventArgs e)
    {
        HamburgerClicked?.Invoke(this, EventArgs.Empty);
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