
using BirdBrain.Views;
namespace BirdBrain
{
    public partial class AppShell : Shell
    {
        private bool _isNavigating;
        public AppShell()
        {
            InitializeComponent();
            Navigating += OnNavigating;
            Navigated += OnNavigated;
            Routing.RegisterRoute(nameof(IntroPage), typeof(IntroPage));
            Routing.RegisterRoute(nameof(BirdSelectionPage), typeof(BirdSelectionPage));
            Routing.RegisterRoute(nameof(BirdSightingPage), typeof(BirdSightingPage));
            Routing.RegisterRoute(nameof(LocationSightingPage), typeof(LocationSightingPage));
            Routing.RegisterRoute(nameof(LocationProfilePage), typeof(LocationProfilePage));          
            Routing.RegisterRoute(nameof(BirdProfilePage), typeof(BirdProfilePage));         
        }

        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if (e.Source != ShellNavigationSource.Push &&
                e.Source != ShellNavigationSource.ShellItemChanged &&
                e.Source != ShellNavigationSource.ShellSectionChanged)
                return;

            if (_isNavigating)
            {
                e.Cancel();
                return;
            }


            _isNavigating = true;
            _ = Task.Delay(1000).ContinueWith(_ =>
            {
                _isNavigating = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            _isNavigating = false;      // navigation finished
        }

    }
    
}
