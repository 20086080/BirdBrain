namespace BirdBrain.Views;

public partial class BirdSighting : BasePage
{
	public BirdSighting()
	{
        InitializeComponent();
	}

    void LocationTapped(object sender, EventArgs e)
    {
        LocationTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        BirdTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        LocationTabLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        BirdTabLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
    }

    async void BirdTapped(object sender, EventArgs e)
    {
        BirdTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        LocationTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        BirdTabLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        LocationTabLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];

        // Navigate using Shell
        //await Shell.Current.GoToAsync(nameof(BirdSelectionPage));     //Call Location Selection
    }
}