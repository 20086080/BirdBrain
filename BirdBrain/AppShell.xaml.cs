
using BirdBrain.Views;
namespace BirdBrain
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(IntroPage), typeof(IntroPage));
            Routing.RegisterRoute(nameof(BirdSelectionPage), typeof(BirdSelectionPage));
            Routing.RegisterRoute(nameof(BirdSightingPage), typeof(BirdSightingPage));
            Routing.RegisterRoute(nameof(LocationSightingPage), typeof(LocationSightingPage));
        }
    }
}
