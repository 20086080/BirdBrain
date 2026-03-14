using BirdBrain;
using BirdBrain.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;

using SkiaSharp.Views.Maui.Controls.Hosting;
namespace BirdBrain
{
    public static class MauiProgram
    {
        
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Roboto-Regular.ttf", "AppRegular");
                    fonts.AddFont("Roboto-Medium.ttf", "AppMedium");

                });
            builder.Services.AddSingleton<JsonFileReader>();
            builder.Services.AddSingleton<SavedLocationService>();
            builder.Services.AddSingleton<SavedBirdService>();
            builder.Services.AddSingleton<AppState>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SummaryService>();


            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseLiveCharts();

            var app = builder.Build();

            App.State = app.Services.GetRequiredService<AppState>();

            return app;

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
