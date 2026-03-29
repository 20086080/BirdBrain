using BirdBrain.Models;
using BirdBrain.Resources.Themes;
using BirdBrain.Services;
using BirdBrain.Views;

namespace BirdBrain
{
    public partial class App : Application
    {
        //public static AppState State { get; set; } = new AppState();
        public static AppState State { get; private set; }
        public App()
        {
            InitializeComponent();

            // Initialize AppState
            State = new AppState();
            State.Database = new DatabaseService();
            //State = Handler.MauiContext.Services.GetRequiredService<AppState>();

            // Show animation page first
            MainPage = new BirdBrain.Views.Animation();

            // Let UI render FIRST
            Dispatcher.Dispatch(async () =>
            {
                await Task.Yield(); 

                ThemeManager.LoadSavedTheme(typeof(SunsetCoralNavyDark));

                await InitializeApp();
            });

            // Start initialization
            InitializeApp();
        }

        private async Task InitializeApp()
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

        
    }
}