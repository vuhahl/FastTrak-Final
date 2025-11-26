namespace FastTrak
    using FastTrak.Views;
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Needed for navigation from Restaurants → Menu Items
            Routing.RegisterRoute(nameof(MenuItemsPage), typeof(MenuItemsPage));
        }
    }
}
