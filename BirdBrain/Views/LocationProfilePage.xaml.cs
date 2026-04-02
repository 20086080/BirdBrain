using BirdBrain.Services;
using BirdBrain.Models;
using System.Diagnostics;
namespace BirdBrain.Views;

public partial class LocationProfilePage : BasePage
{
    private readonly SummaryService _summaryService;
    public LocationProfilePage(SummaryService summaryService)
	{
		InitializeComponent();
        _summaryService = summaryService;
        App.State.LeftSelected = true;
        BindingContext = App.State;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        App.State.LeftSelected = true;
    }

    async void BirdTapped(object? sender, EventArgs e)
    {
        try
        {
            if (!App.State.HasBird)
                return;
            App.State.LeftSelected = false;
            await Shell.Current.GoToAsync(nameof(BirdProfilePage));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
    }
}