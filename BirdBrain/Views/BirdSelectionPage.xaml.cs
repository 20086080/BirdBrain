
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
        AppState.LeftSelected = false;
        BindingContext = AppState;
        _jsonReader = jsonReader;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (AppState.SavedBirds == null || AppState.SavedBirds.Count == 0)
        {
            AppState.SavedBirds = await _jsonReader
                .ReadListAsync<Bird>("BirdsSeedData.json");
        }

        BirdCarousel.ItemsSource = AppState.SavedBirds;

        if (AppState.SavedBirds != null && AppState.SavedBirds.Count > 0)
        {
            if (!AppState.SavedBirds.Contains(AppState.SelectedSavedBird))
            {
                AppState.SelectedSavedBird = AppState.SavedBirds[0];
            }
        }
    }

    async void Bird_Completed(object sender, EventArgs e)
    {
        
        Bird.Unfocus();
        await KeyboardHelper.DismissAsync();
        
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        });
    }

    async void OnBirdTapped(object sender, TappedEventArgs e)
    {
        
        if (e.Parameter is Bird bird)
        {
            AppState.SelectedBirdCommonName = bird.CommonName;
            AppState.SelectedSavedBird = bird;
            
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        }
    }
}