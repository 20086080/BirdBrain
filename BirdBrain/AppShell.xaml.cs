
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
            Routing.RegisterRoute(nameof(LocationSightingPage), typeof(LocationProfilePage));
            Routing.RegisterRoute(nameof(LocationSightingPage), typeof(LocationInsightsPage));
            Routing.RegisterRoute(nameof(LocationSightingPage), typeof(BirdProfilePage));
            Routing.RegisterRoute(nameof(LocationSightingPage), typeof(BirdInsightsPage));
        }
    }
    
}
