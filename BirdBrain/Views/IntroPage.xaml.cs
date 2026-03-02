
using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class IntroPage : BasePage
{
    
    public IntroPage()
    {
        InitializeComponent();

        LocationCarousel.ItemsSource = new List<SavedLocation>
        {
          
            new SavedLocation { Name = "Location 1", Image = "parrot.png" },
            new SavedLocation { Name = "Location 2", Image = "sparrow.png" },
            new SavedLocation { Name = "Location 3", Image = "parrot.png" },
            new SavedLocation { Name = "Location 4", Image = "sparrow.png" }
        };
    }

    

    private void OnTextChanged(object sender, EventArgs e)
    {
        this.AppState.SelectedLocationName = LocationEntry.Text;
    }

    

    void OnAllBirdsTapped(object sender, EventArgs e)
    {
        AllBirdsTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        SpecificBirdTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        AllBirdsLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        SpecificBirdLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
    }

    async void OnSpecificBirdTapped(object sender, EventArgs e)
    {
        SpecificBirdTab.Style =
            (Style)Application.Current.Resources["SegmentSelectedStyle"];

        AllBirdsTab.Style =
            (Style)Application.Current.Resources["SegmentUnselectedStyle"];

        SpecificBirdLabel.Style =
            (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];

        AllBirdsLabel.Style =
            (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];

        // Navigate using Shell
        await Shell.Current.GoToAsync(nameof(BirdSelectionPage));
    }

    async void OnUseLocationInvoked(object sender, EventArgs e)
    {
        // Optional micro animation
        await this.ScaleToAsync(0.98, 70);
        await this.ScaleToAsync(1, 70);

        // 🔥 Call your location logic here
        // await GetCurrentLocation();
    }
}
