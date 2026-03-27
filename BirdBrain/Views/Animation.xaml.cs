using System.Diagnostics;
namespace BirdBrain.Views;

public partial class Animation : ContentPage
{
    string[] images = { "animate1a.png", "animate2a.png", "animate3a.png", "animate4a.png" };
    public Animation()
	{
        InitializeComponent();
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RunIntroAnimation();
        
        Application.Current.MainPage = new AppShell();
    }

    private async Task RunIntroAnimation()
    {
        try
        {
            //AnimatedImage.Opacity = 0;
            foreach (var img in images)
            {
                AnimatedImage.Opacity = 0;
                AnimatedImage.Source = img;
                await AnimatedImage.FadeToAsync(1, 100);
                await Task.Delay(500);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
    }
}