using Microsoft.Extensions.DependencyInjection;
using BirdBrain.Resources.Themes;
using BirdBrain.Services;

namespace BirdBrain
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ThemeManager.LoadSavedTheme(typeof(BlueTealDark));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}