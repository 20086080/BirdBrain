using BirdBrain.Models;
using CommunityToolkit.Maui.Views;

namespace BirdBrain.Controls;

public partial class ErrorPopup : Popup
{
    public ErrorPopup(AppError error)
    {
        InitializeComponent();
        BindingContext = error;
        StartAutoClose();
    }

    private async void StartAutoClose()
    {
        await Task.Delay(8000); // 8 seconds
        Close();
    }
}
