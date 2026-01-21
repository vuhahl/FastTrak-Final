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
    public static class MauiProgram

    /// <summary>
    /// defines What services exist
    /// What pages exist
    /// What ViewModels exist
    ///  dependency injection works
    /// </summary>
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


            // Repository (one instance used everywhere to prevent database conflicts)
            builder.Services.AddSingleton<NutritionRepository>();

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
