namespace BirdBrain.Views;

public partial class IntroPage : ContentPage
{
    string[] images = { "animate1a.png", "animate2a.png", "animate3a.png", "animate4a.png", "animate5a.png" };

    public IntroPage()
    {
        InitializeComponent();

        LocationCarousel.ItemsSource = new List<string>
        {
        "1 Location",
        "2 Location",
        "3 Location",
        "4 Location",
        "5 Location"
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RunIntroAnimation();
    }

    private async Task RunIntroAnimation()
    {
        foreach (var img in images)
        {
            AnimatedImage.Source = img;

            await AnimatedImage.FadeTo(1, 250);  // Fade in
            await Task.Delay(350);               // Hold
            await AnimatedImage.FadeTo(0, 250);   // Fade out
        }

        // Slide entire image layer up
        await ImageLayer.TranslateTo(0, -this.Height, 200, Easing.CubicInOut);

        // Remove it completely (optional but cleaner)
        ImageLayer.IsVisible = false;

        //await Task.Delay(600);
        // Enable menu
        MenuPanel.IsEnabled = true;
        MenuPanel.InputTransparent = false;

        // 🔥 Slide menu from bottom to TOP
        MenuPanel.Opacity = 100;
        await MenuPanel.TranslateTo(0, 0, 100, Easing.CubicOut);

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
        var primary = (Color)Application.Current.Resources["Primary"];
        var surface = (Color)Application.Current.Resources["Surface"];
        var textOnPrimary = (Color)Application.Current.Resources["TextOnPrimary"];
        var textPrimary = (Color)Application.Current.Resources["TextPrimary"];

        AllBirdsTab.BackgroundColor = primary;
        SpecificBirdTab.BackgroundColor = surface;

        ((Label)AllBirdsTab.Content).TextColor = textOnPrimary;
        ((Label)SpecificBirdTab.Content).TextColor = textPrimary;
    }

    void OnSpecificBirdTapped(object sender, EventArgs e)
    {
        var primary = (Color)Application.Current.Resources["Primary"];
        var surface = (Color)Application.Current.Resources["Surface"];
        var textOnPrimary = (Color)Application.Current.Resources["TextOnPrimary"];
        var textPrimary = (Color)Application.Current.Resources["TextPrimary"];

        SpecificBirdTab.BackgroundColor = primary;
        AllBirdsTab.BackgroundColor = surface;

        ((Label)SpecificBirdTab.Content).TextColor = textOnPrimary;
        ((Label)AllBirdsTab.Content).TextColor = textPrimary;
    }

    async void OnUseLocationInvoked(object sender, EventArgs e)
    {
        // Optional micro animation
        await this.ScaleTo(0.98, 70);
        await this.ScaleTo(1, 70);

        // 🔥 Call your location logic here
        // await GetCurrentLocation();
    }
}
