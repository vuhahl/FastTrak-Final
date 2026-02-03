using FastTrak.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTrak.Api.Endpoints;

public static class RestaurantEndpoints
{
    public static void MapRestaurantEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/restaurants")
            .WithTags("Restaurants");

        // GET /api/v1/restaurants - List all restaurants
        group.MapGet("/", async (FastTrakDbContext db) =>
        {
            var restaurants = await db.Restaurants
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Slug,
                    MenuItemCount = r.MenuItems.Count
                })
                .ToListAsync();

            return Results.Ok(new { data = restaurants });
        })
        .WithName("GetRestaurants")
        .WithSummary("Get all restaurants")
        .WithDescription("Returns a list of all restaurants with their menu item counts.");

        // GET /api/v1/restaurants/{id} - Get single restaurant
        group.MapGet("/{id:int}", async (int id, FastTrakDbContext db) =>
        {
            var restaurant = await db.Restaurants
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Slug,
                    MenuItemCount = r.MenuItems.Count
                })
                .FirstOrDefaultAsync();

            return restaurant is null
                ? Results.NotFound(new { error = $"Restaurant with ID {id} not found" })
                : Results.Ok(new { data = restaurant });
        })
        .WithName("GetRestaurant")
        .WithSummary("Get a restaurant by ID");

        // GET /api/v1/restaurants/{id}/menu-items - Get menu items for restaurant
        group.MapGet("/{id:int}/menu-items", async (
            int id,
            FastTrakDbContext db,
            int page = 1,
            int pageSize = 20,
            string? category = null) =>
        {
            // Validate restaurant exists
            var restaurantExists = await db.Restaurants.AnyAsync(r => r.Id == id);
            if (!restaurantExists)
            {
                return Results.NotFound(new { error = $"Restaurant with ID {id} not found" });
            }

            // Clamp page size
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = db.MenuItems
                .Where(mi => mi.RestaurantId == id);

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(mi => mi.Category == category);
            }

            var totalCount = await query.CountAsync();

            var menuItems = await query
                .OrderBy(mi => mi.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(mi => new
                {
                    mi.Id,
                    mi.RestaurantId,
                    mi.Name,
                    mi.Category,
                    mi.Calories,
                    mi.Protein,
                    mi.Carbs,
                    mi.Fat,
                    mi.Sodium,
                    mi.IsDirectAdd
                })
                .ToListAsync();

            return Results.Ok(new
            {
                data = menuItems,
                meta = new
                {
                    page,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            });
        })
        .WithName("GetRestaurantMenuItems")
        .WithSummary("Get menu items for a restaurant")
        .WithDescription("Returns paginated menu items. Supports filtering by category.");
    }
}
