using BirdBrain.Models;
using BirdBrain.Resources.Themes;
using BirdBrain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BirdBrain
{
    public partial class App : Application
    {
        public static AppState State { get; set; }
        public App()
        {
            InitializeComponent();
            ThemeManager.LoadSavedTheme(typeof(SunsetCoralNavyDark));
            //State = Application.Current?.Handler?.MauiContext?.Services.GetService<AppState>();
            State = new AppState();
            State.Database = new DatabaseService();
            LoadCachedData();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new BirdBrain.Views.Animation());
        }

        private async void LoadCachedData()
        {
            await State.Database.InitAsync();

            var cached =
                await State.Database.GetLatestObservationsAsync();

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