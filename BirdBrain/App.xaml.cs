using Microsoft.Extensions.DependencyInjection;
using BirdBrain.Resources.Themes;
using BirdBrain.Services;

namespace BirdBrain
{
    public partial class App : Application
    {
        public static AppState State { get; private set; }
        public App()
        {
            InitializeComponent();
            State = Application.Current?.Handler?.MauiContext?.Services.GetService<AppState>();

            ThemeManager.LoadSavedTheme(typeof(SunsetCoralNavyDark));
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new BirdBrain.Views.Animation());
        }
    }
}