
using BirdBrain.Helpers;
using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
    private readonly JsonFileReader _jsonReader;
    public BirdSelectionPage(JsonFileReader jsonReader)
	{
		InitializeComponent();
        App.State.LeftSelected = false;
        BindingContext = App.State;
        _jsonReader = jsonReader;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var savedLocations = await _jsonReader
            .ReadListAsync<Bird>("BirdsSeedData.json");

        BirdCarousel.ItemsSource = savedLocations;
    }

    async void Bird_Completed(object sender, EventArgs e)
    {
        // Navigate to BirdSelection Page
        //await DisplayAlertAsync("debug","ok","OK");
        Bird.Unfocus();
        await KeyboardHelper.DismissAsync();
        // optional short delay
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        });
    }
}