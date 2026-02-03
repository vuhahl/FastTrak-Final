using FastTrak.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTrak.Api.Endpoints;

public static class MenuItemEndpoints
{
    public static void MapMenuItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/menu-items")
            .WithTags("Menu Items");

        // GET /api/v1/menu-items/{id} - Get single menu item
        group.MapGet("/{id:int}", async (int id, FastTrakDbContext db) =>
        {
            var menuItem = await db.MenuItems
                .Where(mi => mi.Id == id)
                .Select(mi => new
                {
                    mi.Id,
                    mi.RestaurantId,
                    RestaurantName = mi.Restaurant.Name,
                    mi.Name,
                    mi.Category,
                    mi.Calories,
                    mi.Protein,
                    mi.Carbs,
                    mi.Fat,
                    mi.Sodium,
                    mi.IsDirectAdd,
                    OptionCount = mi.MenuItemOptions.Count
                })
                .FirstOrDefaultAsync();

            return menuItem is null
                ? Results.NotFound(new { error = $"Menu item with ID {id} not found" })
                : Results.Ok(new { data = menuItem });
        })
        .WithName("GetMenuItem")
        .WithSummary("Get a menu item by ID")
        .WithDescription("Returns detailed information about a single menu item including nutrition data.");

        // GET /api/v1/menu-items/{id}/options - Get customization options for menu item
        group.MapGet("/{id:int}/options", async (int id, FastTrakDbContext db) =>
        {
            // Validate menu item exists
            var menuItemExists = await db.MenuItems.AnyAsync(mi => mi.Id == id);
            if (!menuItemExists)
            {
                return Results.NotFound(new { error = $"Menu item with ID {id} not found" });
            }

            var options = await db.MenuItemOptions
                .Where(mio => mio.MenuItemId == id)
                .Select(mio => new
                {
                    mio.CustomOption.Id,
                    mio.CustomOption.Name,
                    mio.CustomOption.Category,
                    mio.CustomOption.Calories,
                    mio.CustomOption.Protein,
                    mio.CustomOption.Carbs,
                    mio.CustomOption.Fat,
                    mio.CustomOption.Sodium
                })
                .ToListAsync();

            // Group count by category for metadata
            var categoryGroups = options
                .GroupBy(o => o.Category)
                .ToDictionary(g => g.Key, g => g.Count());

            return Results.Ok(new
            {
                data = options,
                meta = new
                {
                    totalCount = options.Count,
                    groupedByCategory = categoryGroups
                }
            });
        })
        .WithName("GetMenuItemOptions")
        .WithSummary("Get customization options for a menu item")
        .WithDescription("Returns available customization options (toppings, sauces, etc.) for the menu item.");
    }
}
