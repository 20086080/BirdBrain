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
        App.State.LeftSelected = false;
        BindingContext = App.State;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
    }
    async void LocationTapped(object sender, EventArgs e)
    {
        LocationTab.Style = (Style)Application.Current.Resources["SegmentSelectedStyle"];
        BirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = true;
        await Shell.Current.GoToAsync(nameof(LocationProfilePage));
    }

    async void BirdTapped(object sender, EventArgs e)
    {
        BirdTab.Style = (Style)Application.Current.Resources["SegmentSelectedStyle"];
        LocationTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
        BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
        LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
        App.State.LeftSelected = false;
        await Shell.Current.GoToAsync(nameof(BirdProfilePage));
    }
}