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

    private void OnMenuRefreshTapped(object sender, TappedEventArgs e)
    {

    }
    void OnMenuSettingsTapped(object sender, TappedEventArgs e)
    {
        SettingsClicked?.Invoke(this, EventArgs.Empty);
    }
}