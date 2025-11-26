using FastTrak.Data;
using Microsoft.Extensions.Logging;

namespace FastTrak
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Repository
            builder.Services.AddSingleton<NutritionRepository>();

            // ViewModels
            builder.Services.AddTransient<RestaurantsViewModel>();

            // Pages
            builder.Services.AddTransient<RestaurantsPage>();

            return builder.Build();
        }
}
