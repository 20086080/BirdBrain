namespace BirdBrain.Views;

public partial class Animation : ContentPage
{
    string[] images = { "animate1a.png", "animate2a.png", "animate3a.png", "animate4a.png", "animate5a.png" };
    public Animation()
	{
        InitializeComponent();
        RunIntroAnimation();
    }

    protected override async void OnAppearing()
    {
        //base.OnAppearing();
        await RunIntroAnimation();
    }

    private async Task RunIntroAnimation()
    {
        AnimatedImage.Opacity = 0;
        foreach (var img in images)
        {
            //await AnimatedImage.FadeToAsync(0, 100);
            AnimatedImage.Source = img;
            await AnimatedImage.FadeToAsync(1, 100);
            await Task.Delay(1000);
        }
        await Task.Delay(50);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current.Windows[0].Page = new AppShell();
        });
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
    }

    
}