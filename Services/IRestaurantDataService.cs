using FastTrak.Models;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.Services
{
    /// <summary>
    /// Service interface for restaurant reference data.
    ///
    /// WHY THIS INTERFACE EXISTS:
    /// This abstracts WHERE the data comes from. Today it's SQLite (via NutritionRepository).
    /// Tomorrow it will be a REST API. The ViewModels don't need to know or care.
    ///
    /// MIGRATION PATH:
    /// Phase 1 (now):  NutritionRepository implements this interface (SQLite)
    /// Phase 2 (API):  RestaurantApiService implements this interface (HTTP)
    /// Phase 3 (hybrid): Can swap implementations or use fallback pattern
    ///
    /// WHAT THIS COVERS:
    /// - Restaurants (list of available restaurants)
    /// - MenuItems (food items for each restaurant)
    /// - CustomOptions (toppings, milk types, sauces linked to menu items)
    ///
    /// WHAT THIS DOES NOT COVER:
    /// - User's logged items (see IUserLogRepository)
    /// - External food search (see FatSecretService)
    /// </summary>
    public interface IRestaurantDataService
    {
        /// <summary>
        /// Gets all available restaurants, ordered by name.
        /// </summary>
        Task<List<Restaurant>> GetRestaurantsAsync();

        /// <summary>
        /// Gets all menu items for a specific restaurant.
        /// Ordered by name.
        /// </summary>
        Task<List<MenuItem>> GetMenuItemsForRestaurantAsync(int restaurantId);

        /// <summary>
        /// Gets a single menu item by ID.
        /// Used by CustomizationPage to load base nutrition values.
        /// </summary>
        Task<MenuItem> GetMenuItemAsync(int id);

        /// <summary>
        /// Gets available customization options for a menu item.
        /// Uses the MenuItemOption junction table to determine which
        /// CustomOptions are valid for this item.
        /// </summary>
        Task<List<CustomOption>> GetCustomOptionsForMenuItemAsync(int menuItemId);
    }
}
