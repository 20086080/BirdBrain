
using BirdBrain.Controls;
using BirdBrain.Services;
using System.Windows.Markup;
namespace BirdBrain.Views;

public partial class BasePage : ContentPage
{
    public BasePage()
    {
        InitializeComponent();        
    }

    private async void OnRightSwiped(object sender, SwipedEventArgs e)
    {
        //await RightDrawer.OpenAsync();
    }


    protected AppState AppState =>
    Application.Current
        .Handler
        .MauiContext
        .Services
        .GetRequiredService<AppState>();
}