using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdProfilePage : BasePage
{
    private readonly SummaryService _summaryService;
    public BirdProfilePage(SummaryService summaryService)
    {
        InitializeComponent();
        _summaryService = summaryService;
        BindingContext = App.State;
        App.State.LeftSelected = false;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LocationTab.Style = (Style)Application.Current!.Resources["SegmentUnselectedStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;
    }
    async void LocationTapped(object? sender, EventArgs e)
    {
        if (!App.State.HasLocation)
            return;
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationProfilePage));
    }

    
}