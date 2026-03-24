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
    async void LocationTapped(object? sender, EventArgs e)
    {
        try
        {
            LocationTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
            BirdTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
            LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
            BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
            App.State.LeftSelected = true;
            await Shell.Current.GoToAsync(nameof(LocationProfilePage));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
    }

    async void BirdTapped(object? sender, EventArgs e)
    {
        try
        {
            BirdTab.Style = (Style)Application.Current!.Resources["SegmentSelectedStyle"];
            LocationTab.Style = (Style)Application.Current.Resources["SegmentUnselectedStyle"];
            BirdTabLabel.Style = (Style)Application.Current.Resources["SegmentSelectedLabelStyle"];
            LocationTabLabel.Style = (Style)Application.Current.Resources["SegmentUnselectedLabelStyle"];
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