using Microsoft.Extensions.DependencyInjection;
using BirdBrain.Services;

namespace BirdBrain
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme("CoralNavy");
            
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}