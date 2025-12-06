using FastTrak.Data;
using FastTrak.Services;
using FastTrak.ViewModels;
using FastTrak.Views;
using Microsoft.Extensions.Logging;
using MenuItem = FastTrak.Models.MenuItem;

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
            // Repository (register ONCE)
            builder.Services.AddSingleton<NutritionRepository>();

            // ViewModels
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<RestaurantsViewModel>();
            builder.Services.AddTransient<MenuItemsViewModel>();
            builder.Services.AddTransient<CustomizationViewModel>();
            builder.Services.AddTransient<CalculatorViewModel>();
            builder.Services.AddTransient<FatSecretSearchViewModel>();

            // Pages
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<RestaurantsPage>();
            builder.Services.AddTransient<MenuItemsPage>();
            builder.Services.AddTransient<CustomizationPage>();

            builder.Services.AddSingleton<FatSecretService>();
            builder.Services.AddTransient<FatSecretSearchPage>();
            builder.Services.AddTransient<CalculatorPage>();


            return builder.Build();
        }
    }
}
