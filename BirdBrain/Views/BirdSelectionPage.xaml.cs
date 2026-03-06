
using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
    private readonly JsonFileReader _jsonReader;
    public BirdSelectionPage(JsonFileReader jsonReader)
	{
		InitializeComponent();
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
        Bird.Unfocus();   // release keyboard / focus
        await Task.Delay(100);   // allow UI to settle
        await Shell.Current.GoToAsync(nameof(BirdSighting));
    }
}