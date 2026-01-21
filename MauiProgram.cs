using CommunityToolkit.Mvvm;
using FastTrak.Data;
using FastTrak.Services;
using FastTrak.ViewModels;
using FastTrak.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Plugin.MauiMTAdmob;


namespace FastTrak
{
    /// <summary>
    /// Application entry point and Dependency Injection (DI) configuration.
    ///
    /// WHY DEPENDENCY INJECTION?
    /// DI allows us to swap implementations without changing ViewModels.
    /// Example: ViewModels request IRestaurantDataService, not NutritionRepository.
    /// Today that resolves to SQLite. Tomorrow it resolves to an API client.
    ///
    /// REGISTRATION PATTERNS:
    /// - Singleton: One instance shared app-wide (database connections, HTTP clients)
    /// - Transient: New instance every time (ViewModels, Pages)
    /// </summary>
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiMTAdmob()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentIcons");
                });


            // ===========================================
            // DATA SERVICES - Singleton for shared state
            // ===========================================

            // NutritionRepository: Single SQLite connection prevents database conflicts.
            // Currently implements BOTH interfaces. After API migration, it will only
            // implement IUserLogRepository, and a new RestaurantApiService handles the rest.
            builder.Services.AddSingleton<NutritionRepository>();

            // Interface registrations - ViewModels depend on these, not concrete classes.
            // This is the "seam" that allows swapping SQLite for API later.
            builder.Services.AddSingleton<IUserLogRepository>(sp => sp.GetRequiredService<NutritionRepository>());
            builder.Services.AddSingleton<IRestaurantDataService>(sp => sp.GetRequiredService<NutritionRepository>());

            // ViewModels - new instances crated when needed, every page gets a viewmodel
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<RestaurantsViewModel>();
            builder.Services.AddTransient<MenuItemsViewModel>();
            builder.Services.AddTransient<CustomizationViewModel>();
            builder.Services.AddTransient<CalculatorViewModel>();
            builder.Services.AddTransient<FatSecretSearchViewModel>();

            // Pages
            builder.Services.AddTransient<LoadingPage>();
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
