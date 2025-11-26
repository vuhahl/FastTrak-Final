using FastTrak.Views;

namespace FastTrak
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Register navigation routes for non-tab pages
            Routing.RegisterRoute(nameof(MenuItemsPage), typeof(MenuItemsPage));
            Routing.RegisterRoute(nameof(CustomizationPage), typeof(CustomizationPage));
            Routing.RegisterRoute(nameof(NutritionixSearchPage), typeof(NutritionixSearchPage));
        }
    }
}

