
using BirdBrain.Models;

namespace BirdBrain.Views;

public partial class IntroPage : ContentPage
{
    string[] images = { "animate1a.png", "animate2a.png", "animate3a.png", "animate4a.png", "animate5a.png" };

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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Shell.SetNavBarIsVisible(this, true);
        await RunIntroAnimation();
    }

    private async Task RunIntroAnimation()
    {
        AnimatedImage.Opacity = 0;
        foreach (var img in images)
        {
            await AnimatedImage.FadeToAsync(0, 150);
            AnimatedImage.Source = img;
            await AnimatedImage.FadeToAsync(1, 150);
            await Task.Delay(1200);

            //await AnimatedImage.FadeToAsync(1, 150);    // Fade in
            //await Task.Delay(850);                      // Hold image
            //await AnimatedImage.FadeToAsync(0, 150);   // Fade out
        }

        // Slide entire image layer up
        await ImageLayer.TranslateToAsync(0, -this.Height, 200, Easing.CubicInOut);

        // Remove it completely (optional but cleaner)
        ImageLayer.IsVisible = false;

        // Enable menu
        MenuPanel.IsEnabled = true;
        MenuPanel.InputTransparent = false;

        // 🔥 Slide menu from bottom to TOP
        MenuPanel.Opacity = 100;
        await MenuPanel.TranslateToAsync(0, 0, 100, Easing.CubicOut);

    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (MenuPanel.TranslationY == 0) // prevent resetting
        {
            MenuPanel.TranslationY = height;
        }
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
