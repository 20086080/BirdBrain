using BirdBrain.Models;
using BirdBrain.Resources.Themes;
using BirdBrain.Services;
using BirdBrain.Views;

namespace BirdBrain
{
    public partial class App : Application
    {
        public static AppState State { get; set; }

        public App()
        {
            InitializeComponent();

            // Load theme
            ThemeManager.LoadSavedTheme(typeof(SunsetCoralNavyDark));

            // Initialize AppState
            State = new AppState();
            State.Database = new DatabaseService();

            // Load cached DB data (non-blocking)
            LoadCachedData();

            // Show animation page first
            MainPage = new BirdBrain.Views.Animation();

            // Start initialization
            InitializeApp();
        }

        private async void InitializeApp()
        {
            try
            {
                await State.InitializeAsync();   
            }
            catch (Exception ex)
            {
                // Basic error fallback
                MainPage = new ContentPage
                {
                    Content = new Label
                    {
                        Text = $"Startup error: {ex.Message}",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    }
                };
            }
        }

        private async void LoadCachedData()
        {
            await State.Database.InitAsync();

            var cached = await State.Database.GetLatestObservationsAsync();

            if (cached != null && cached.Count > 0)
            {
                State.Observations = cached.Select(x => new BirdObservation
                {
                    speciesCode = x.SpeciesCode,
                    comName = x.ComName,
                    sciName = x.SciName,
                    locId = x.LocId,
                    locName = x.LocName,
                    obsDt = x.ObsDt,
                    howMany = x.HowMany,
                    lat = x.Lat,
                    lng = x.Lng,
                    obsValid = x.ObsValid
                }).ToList();
            }
        }
    }
}