namespace BirdBrain.Views;

public partial class LocationInsightsPage : BasePage
{
	public LocationInsightsPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        App.State.LeftSelected = true;
    }
}