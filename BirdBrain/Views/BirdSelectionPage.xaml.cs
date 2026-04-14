
//using AndroidX.ConstraintLayout.Helper.Widget;
using BirdBrain.Helpers;
using BirdBrain.Models;
using BirdBrain.Services;
using System.ComponentModel.Design;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
    public string? CutoffDate;
    public BirdSelectionPage()
	{
		InitializeComponent();
        App.State.LeftSelected = false;
        BindingContext = App.State;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        BirdCarousel.CurrentItem = null;   
        BirdCarousel.CurrentItem = CarouselCurrentItem;
        App.State.LeftSelected = false;
    }

    void Carousel_CurrentItemChanged(object? sender, CurrentItemChangedEventArgs e)
    {
        if (e.CurrentItem is Bird bird)
        {
            App.State.SelectedSavedBird = bird;
        }
    }

    public Bird CarouselCurrentItem
    {
        get
        {
            if (App.State.SavedBirds == null || App.State.SavedBirds.Count == 0)
                return null!;

            if (App.State.SelectedSavedBird == null)
                return App.State.SavedBirds[0];

            var match = App.State.SavedBirds.FirstOrDefault(b =>
                b.CommonName!.Equals(App.State.SelectedSavedBird.CommonName, StringComparison.OrdinalIgnoreCase));

            return match ?? App.State.SavedBirds[0];
        }
    }

    async void Bird_Completed(object? sender, EventArgs e)
    {
        if (sender is not Entry entry)
            return;

        string textString = entry?.Text?.Trim()!;

        if (!ValidationHelper.IsValidString(textString!))
        {
            await ErrorService.Show(ErrorType.InvalidBird);
            return;
        }
        var selected = App.State.SavedBirds
            .FirstOrDefault(b => b.CommonName!.Equals(textString, StringComparison.OrdinalIgnoreCase));

        if (selected != null)
        {
            App.State.SelectedSavedBird = selected;
        }
        else
        {
            App.State.SelectedSavedBird = new Bird();
            App.State.SelectedSavedBird.CommonName = textString;
        }

        bool checkBird = await Check_Bird();
        if (!checkBird)
            return;

        Bird.Unfocus();
        await KeyboardHelper.DismissAsync();
        
        Application.Current!.Dispatcher.Dispatch(async () =>
        {
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        });
    }

    async Task<bool> Check_Bird()
    {
        //if (!App.State.HasBird)
        //    return false;
        CutoffDate = DateTime.UtcNow.AddDays(-App.State.Days).ToString("yyyy-MM-dd");
        string? comName = App.State.SelectedSavedBird.CommonName;
        var result = await SummaryService.BirdHasDataAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng, CutoffDate, comName);
        if (!result)                                            // Bird has no data for this Location and date range 
        {
            await ErrorService.Show(ErrorType.NoBirdsFound);
            return false;
        }
        return true;
    }

    async void OnBirdTapped(object? sender, TappedEventArgs e)       //Selection from Saved Bird List
    {
        if (e.Parameter is Bird bird)
        {
            App.State.SelectedSavedBird = bird;
            bool checkBird = await Check_Bird();
            if (!checkBird)
                return;
            
            await Shell.Current.GoToAsync(nameof(BirdSightingPage));
        }
    }
}