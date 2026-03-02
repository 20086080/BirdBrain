
using BirdBrain.Views;
namespace BirdBrain
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(BirdSelectionPage), typeof(BirdSelectionPage));
            Routing.RegisterRoute(nameof(BirdSighting), typeof(BirdSighting));
        }
    }
}
