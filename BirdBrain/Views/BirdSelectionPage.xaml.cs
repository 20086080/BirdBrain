
using BirdBrain.Helpers;
using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
    private readonly JsonFileReader _jsonReader;
    public int BirdIndex = 0;
    public List<Bird> savedBirds = new List<Bird>();
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

        savedBirds = await _jsonReader
            .ReadListAsync<Bird>("BirdsSeedData.json");

        BirdCarousel.ItemsSource = savedBirds;
        if (AppState.SelectedBirdCommonName != null)
        {
            BirdIndex = savedBirds.FindIndex(x => x.CommonName == AppState.SelectedBirdCommonName);
        }
        if (BirdIndex >= 0)
        {
            BirdCarousel.Position = BirdIndex;
        }
        else
        {
            BirdCarousel.Position = 0;
        }
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

    async void OnBirdTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter == null)
            return;
        if (e.Parameter is Bird bird)
        {
            AppState.SelectedBirdCommonName = bird.CommonName;
            AppState.SelectedSavedBird = bird;
            BirdIndex = savedBirds.FindIndex(x => x.CommonName == AppState.SelectedBirdCommonName);
            if (BirdIndex >= 0)
            {
                BirdCarousel.Position = BirdIndex;
                //AppState.SelectedBirdThumbnail = bird.Thumbnail;
                //AppState.SelectedBirdProfileImage = bird.ProfileImage;
                //AppState.SelectedBirdScName = bird.ScientificName;
                //AppState.SelectedBirdDesc = bird.Description;
            }
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        }
    }
}