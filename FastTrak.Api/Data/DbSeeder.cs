using FastTrak.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FastTrak.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(FastTrakDbContext db)
    {
        // Only seed if database is empty
        if (await db.Restaurants.AnyAsync())
        {
            return;
        }

        // 1. Seed Restaurants
        var restaurants = new List<Restaurant>
        {
            new() { Name = "Wendy's", Slug = "wendys" },
            new() { Name = "Dunkin'", Slug = "dunkin" },
            new() { Name = "Wingstop", Slug = "wingstop" },
            new() { Name = "Culver's", Slug = "culvers" }
        };
        db.Restaurants.AddRange(restaurants);
        await db.SaveChangesAsync();

        // Get IDs after insert
        var wendys = restaurants[0].Id;
        var dunkin = restaurants[1].Id;
        var wingstop = restaurants[2].Id;
        var culvers = restaurants[3].Id;

        // 2. Seed Menu Items
        var menuItems = new List<MenuItem>
        {
            // WENDY'S
            new() { RestaurantId = wendys, Name = "Dave's Single", Calories = 524, Protein = 28.0m, Carbs = 37.0m, Fat = 29.0m, Sodium = 866, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Dave's Double", Calories = 879, Protein = 54.0m, Carbs = 39.0m, Fat = 56.0m, Sodium = 1731, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Dave's Triple", Calories = 1195, Protein = 77.0m, Carbs = 41.0m, Fat = 80.0m, Sodium = 2400, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Baconator", Calories = 1001, Protein = 63.0m, Carbs = 38.0m, Fat = 66.0m, Sodium = 2282, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Curry Bean Burger", Calories = 535, Protein = 17.0m, Carbs = 57.0m, Fat = 25.0m, Sodium = 1102, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Spicy Chicken Sandwich", Calories = 400, Protein = 20.0m, Carbs = 45.0m, Fat = 15.0m, Sodium = 865, Category = "Sandwiches" },
            new() { RestaurantId = wendys, Name = "Classic Chicken Sandwich", Calories = 404, Protein = 20.0m, Carbs = 46.0m, Fat = 15.0m, Sodium = 984, Category = "Sandwiches" },
            new() { RestaurantId = wendys, Name = "Grilled Chicken Sandwich", Calories = 337, Protein = 26.0m, Carbs = 35.0m, Fat = 10.0m, Sodium = 630, Category = "Sandwiches" },
            new() { RestaurantId = wendys, Name = "Avocado Chicken Club", Calories = 583, Protein = 30.0m, Carbs = 47.0m, Fat = 30.0m, Sodium = 1338, Category = "Sandwiches" },
            new() { RestaurantId = wendys, Name = "4 Pc Chicken Nuggets", Calories = 114, Protein = 11.0m, Carbs = 4.9m, Fat = 5.6m, Sodium = 366, Category = "Sides" },
            new() { RestaurantId = wendys, Name = "10 Pc Chicken Nuggets", Calories = 286, Protein = 28.0m, Carbs = 12.0m, Fat = 14.0m, Sodium = 905, Category = "Sides" },
            new() { RestaurantId = wendys, Name = "Cheeseburger Deluxe", Calories = 366, Protein = 17.0m, Carbs = 31.0m, Fat = 19.0m, Sodium = 748, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Bacon Cheeseburger", Calories = 410, Protein = 20.0m, Carbs = 30.0m, Fat = 23.0m, Sodium = 905, Category = "Burgers" },
            new() { RestaurantId = wendys, Name = "Caesar Chicken Salad", Calories = 411, Protein = 28.0m, Carbs = 4.2m, Fat = 31.0m, Sodium = 826, Category = "Salads" },
            new() { RestaurantId = wendys, Name = "Avocado Chicken Salad", Calories = 485, Protein = 32.0m, Carbs = 7.5m, Fat = 36.0m, Sodium = 866, Category = "Salads" },
            new() { RestaurantId = wendys, Name = "Value Fries", Calories = 142, Protein = 1.6m, Carbs = 18.0m, Fat = 6.7m, Sodium = 826, Category = "Sides" },
            new() { RestaurantId = wendys, Name = "Medium Fries", Calories = 176, Protein = 2.0m, Carbs = 22.0m, Fat = 8.5m, Sodium = 826, Category = "Sides" },
            new() { RestaurantId = wendys, Name = "Large Fries", Calories = 239, Protein = 2.7m, Carbs = 31.0m, Fat = 11.0m, Sodium = 944, Category = "Sides" },
            new() { RestaurantId = wendys, Name = "Plain Baked Potato", Calories = 232, Protein = 4.4m, Carbs = 39.0m, Fat = 5.4m, Sodium = 748, Category = "Potato" },
            new() { RestaurantId = wendys, Name = "Chili", Calories = 253, Protein = 19.0m, Carbs = 14.0m, Fat = 13.0m, Sodium = 590, Category = "Sides" },
            new() { RestaurantId = wendys, Name = "Jr. Vanilla Frosty", Calories = 179, Protein = 3.5m, Carbs = 27.0m, Fat = 6.3m, Sodium = 134, Category = "Desserts" },
            new() { RestaurantId = wendys, Name = "Jr. Chocolate Frosty", Calories = 174, Protein = 5.2m, Carbs = 28.0m, Fat = 4.6m, Sodium = 79, Category = "Desserts" },

            // DUNKIN'
            new() { RestaurantId = dunkin, Name = "Hot Coffee - Medium", Calories = 5, Protein = 0.0m, Carbs = 0.0m, Fat = 0.0m, Sodium = 10, Category = "Beverages" },
            new() { RestaurantId = dunkin, Name = "Iced Coffee - Medium", Calories = 5, Protein = 0.0m, Carbs = 0.0m, Fat = 0.0m, Sodium = 15, Category = "Beverages" },
            new() { RestaurantId = dunkin, Name = "Cold Brew - Medium", Calories = 5, Protein = 0.0m, Carbs = 0.0m, Fat = 0.0m, Sodium = 15, Category = "Beverages" },
            new() { RestaurantId = dunkin, Name = "Glazed Donut", Calories = 240, Protein = 4.0m, Carbs = 33.0m, Fat = 11.0m, Sodium = 270, Category = "Donuts" },
            new() { RestaurantId = dunkin, Name = "Chocolate Frosted Donut", Calories = 260, Protein = 4.0m, Carbs = 34.0m, Fat = 11.0m, Sodium = 290, Category = "Donuts" },
            new() { RestaurantId = dunkin, Name = "Old Fashioned Donut", Calories = 310, Protein = 4.0m, Carbs = 30.0m, Fat = 19.0m, Sodium = 320, Category = "Donuts" },
            new() { RestaurantId = dunkin, Name = "Boston Kreme Donut", Calories = 270, Protein = 5.0m, Carbs = 39.0m, Fat = 11.0m, Sodium = 320, Category = "Donuts" },
            new() { RestaurantId = dunkin, Name = "Bacon, Egg and Cheese on Croissant", Calories = 520, Protein = 19.0m, Carbs = 34.0m, Fat = 34.0m, Sodium = 870, Category = "Breakfast" },
            new() { RestaurantId = dunkin, Name = "Sausage, Egg and Cheese on Croissant", Calories = 610, Protein = 22.5m, Carbs = 29.8m, Fat = 44.1m, Sodium = 1100, Category = "Breakfast" },
            new() { RestaurantId = dunkin, Name = "Egg and Cheese on English Muffin", Calories = 340, Protein = 14.0m, Carbs = 38.0m, Fat = 15.0m, Sodium = 650, Category = "Breakfast" },

            // WINGSTOP
            new() { RestaurantId = wingstop, Name = "Mild Classic Wing (1 wing)", Calories = 120, Protein = 10.0m, Carbs = 0.0m, Fat = 8.0m, Sodium = 160, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Original Hot Classic Wing (1 wing)", Calories = 90, Protein = 10.0m, Carbs = 0.0m, Fat = 5.0m, Sodium = 230, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Plain Classic Wing (1 wing)", Calories = 90, Protein = 10.0m, Carbs = 0.0m, Fat = 5.0m, Sodium = 30, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Lemon Pepper Classic Wing (1 wing)", Calories = 120, Protein = 10.0m, Carbs = 0.0m, Fat = 8.0m, Sodium = 210, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Garlic Parmesan Classic Wing (1 wing)", Calories = 120, Protein = 10.0m, Carbs = 1.0m, Fat = 8.0m, Sodium = 75, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Cajun Classic Wing (1 wing)", Calories = 90, Protein = 10.0m, Carbs = 0.0m, Fat = 5.0m, Sodium = 310, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Mild Boneless Wing (1 piece)", Calories = 110, Protein = 4.0m, Carbs = 6.0m, Fat = 7.0m, Sodium = 330, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Original Hot Boneless Wing (1 piece)", Calories = 80, Protein = 4.0m, Carbs = 6.0m, Fat = 4.5m, Sodium = 390, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Plain Boneless Wing (1 piece)", Calories = 80, Protein = 4.0m, Carbs = 6.0m, Fat = 4.5m, Sodium = 230, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Lemon Pepper Boneless Wing (1 piece)", Calories = 110, Protein = 4.0m, Carbs = 6.0m, Fat = 7.0m, Sodium = 290, Category = "Wings" },
            new() { RestaurantId = wingstop, Name = "Seasoned Fries - Regular 10oz", Calories = 500, Protein = 8.0m, Carbs = 69.0m, Fat = 21.0m, Sodium = 620, Category = "Sides" },
            new() { RestaurantId = wingstop, Name = "Seasoned Fries - Large 18oz", Calories = 900, Protein = 14.0m, Carbs = 126.0m, Fat = 37.0m, Sodium = 1060, Category = "Sides" },
            new() { RestaurantId = wingstop, Name = "Ranch Dip - 3.25oz Cup", Calories = 320, Protein = 1.0m, Carbs = 2.0m, Fat = 34.0m, Sodium = 870, Category = "Sauces" },
            new() { RestaurantId = wingstop, Name = "Blue Cheese Dip - 3.25oz Cup", Calories = 330, Protein = 4.0m, Carbs = 4.0m, Fat = 33.0m, Sodium = 570, Category = "Sauces" },
            new() { RestaurantId = wingstop, Name = "Honey Mustard Dip - 3.25oz Cup", Calories = 390, Protein = 0.0m, Carbs = 18.0m, Fat = 33.0m, Sodium = 660, Category = "Sauces" },

            // CULVER'S
            new() { RestaurantId = culvers, Name = "ButterBurger Original - Single", Calories = 390, Protein = 20.0m, Carbs = 38.0m, Fat = 17.0m, Sodium = 480, Category = "Burgers" },
            new() { RestaurantId = culvers, Name = "ButterBurger Original - Double", Calories = 560, Protein = 34.0m, Carbs = 38.0m, Fat = 30.0m, Sodium = 580, Category = "Burgers" },
            new() { RestaurantId = culvers, Name = "ButterBurger Original - Triple", Calories = 730, Protein = 48.0m, Carbs = 38.0m, Fat = 43.0m, Sodium = 680, Category = "Burgers" },
            new() { RestaurantId = culvers, Name = "ButterBurger Cheese - Single", Calories = 460, Protein = 24.0m, Carbs = 39.0m, Fat = 23.0m, Sodium = 700, Category = "Burgers" },
            new() { RestaurantId = culvers, Name = "ButterBurger Cheese - Double", Calories = 700, Protein = 41.0m, Carbs = 40.0m, Fat = 42.0m, Sodium = 1020, Category = "Burgers" },
            new() { RestaurantId = culvers, Name = "Mushroom & Swiss ButterBurger - Single", Calories = 530, Protein = 27.0m, Carbs = 41.0m, Fat = 28.0m, Sodium = 650, Category = "Burgers" },
            new() { RestaurantId = culvers, Name = "Crispy Chicken Sandwich", Calories = 690, Protein = 28.0m, Carbs = 65.0m, Fat = 35.0m, Sodium = 1590, Category = "Sandwiches" },
            new() { RestaurantId = culvers, Name = "Grilled Chicken Sandwich", Calories = 480, Protein = 36.0m, Carbs = 40.0m, Fat = 19.0m, Sodium = 1340, Category = "Sandwiches" },
            new() { RestaurantId = culvers, Name = "Spicy Crispy Chicken Sandwich", Calories = 680, Protein = 32.0m, Carbs = 65.0m, Fat = 33.0m, Sodium = 1680, Category = "Sandwiches" },
            new() { RestaurantId = culvers, Name = "Crinkle Cut Fries - Small", Calories = 220, Protein = 3.0m, Carbs = 32.0m, Fat = 9.0m, Sodium = 410, Category = "Sides" },
            new() { RestaurantId = culvers, Name = "Crinkle Cut Fries - Medium", Calories = 350, Protein = 4.0m, Carbs = 50.0m, Fat = 14.0m, Sodium = 650, Category = "Sides" },
            new() { RestaurantId = culvers, Name = "Crinkle Cut Fries - Large", Calories = 430, Protein = 5.0m, Carbs = 62.0m, Fat = 18.0m, Sodium = 810, Category = "Sides" },
            new() { RestaurantId = culvers, Name = "Wisconsin Cheese Curds - Medium", Calories = 490, Protein = 17.0m, Carbs = 46.0m, Fat = 27.0m, Sodium = 1520, Category = "Sides" }
        };
        db.MenuItems.AddRange(menuItems);
        await db.SaveChangesAsync();

        // 3. Seed Custom Options
        var options = new List<CustomOption>
        {
            // Wendy's Options
            new() { Name = "Lettuce", Category = "Topping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
            new() { Name = "Tomato", Category = "Topping", Calories = 3, Protein = 0, Carbs = 0.8m, Fat = 0, Sodium = 0 },
            new() { Name = "Onion", Category = "Topping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
            new() { Name = "Pickles", Category = "Topping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 87 },
            new() { Name = "American Cheese", Category = "Topping", Calories = 42, Protein = 2, Carbs = 1, Fat = 3, Sodium = 193 },
            new() { Name = "Bacon (2 strips)", Category = "Topping", Calories = 42, Protein = 3, Carbs = 0, Fat = 3, Sodium = 189 },
            new() { Name = "Mayo", Category = "Sauce", Calories = 50, Protein = 0, Carbs = 0, Fat = 5.6m, Sodium = 39 },
            new() { Name = "Ketchup", Category = "Sauce", Calories = 11, Protein = 0, Carbs = 2.8m, Fat = 0, Sodium = 87 },

            // Potato toppings
            new() { Name = "Sour Cream", Category = "Potato", Calories = 52, Protein = 0.7m, Carbs = 1.1m, Fat = 5, Sodium = 16 },
            new() { Name = "Butter", Category = "Potato", Calories = 50, Protein = 0, Carbs = 0, Fat = 6, Sodium = 0 },
            new() { Name = "Chives", Category = "Potato", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
            new() { Name = "Shredded Cheese", Category = "Potato", Calories = 36, Protein = 2, Carbs = 0, Fat = 3, Sodium = 67 },

            // Dunkin Options
            new() { Name = "Whole Milk", Category = "MilkType", Calories = 30, Protein = 2, Carbs = 2, Fat = 1.5m, Sodium = 25 },
            new() { Name = "Skim Milk", Category = "MilkType", Calories = 20, Protein = 2, Carbs = 2, Fat = 0, Sodium = 25 },
            new() { Name = "Almond Milk", Category = "MilkType", Calories = 25, Protein = 1, Carbs = 4, Fat = 0.5m, Sodium = 45 },
            new() { Name = "Oat Milk", Category = "MilkType", Calories = 30, Protein = 1, Carbs = 5, Fat = 1, Sodium = 35 },
            new() { Name = "Cream", Category = "MilkType", Calories = 90, Protein = 2, Carbs = 1, Fat = 9, Sodium = 40 },
            new() { Name = "Vanilla Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
            new() { Name = "Hazelnut Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
            new() { Name = "Toasted Almond Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
            new() { Name = "Caramel Swirl", Category = "Swirl", Calories = 150, Protein = 0, Carbs = 38, Fat = 0, Sodium = 70 },
            new() { Name = "Mocha Swirl", Category = "Swirl", Calories = 140, Protein = 0, Carbs = 36, Fat = 0.5m, Sodium = 35 },
            new() { Name = "French Vanilla Swirl", Category = "Swirl", Calories = 150, Protein = 0, Carbs = 37, Fat = 0, Sodium = 50 },
            new() { Name = "Sugar", Category = "Sweetener", Calories = 15, Protein = 0, Carbs = 4, Fat = 0, Sodium = 0 },
            new() { Name = "Liquid Sugar", Category = "Sweetener", Calories = 50, Protein = 0, Carbs = 13, Fat = 0, Sodium = 0 },

            // Culver's Options
            new() { Name = "Pickles (Culver's)", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 52 },
            new() { Name = "Onions (Culver's)", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
            new() { Name = "Tomato (Culver's)", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
            new() { Name = "Lettuce (Culver's)", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
            new() { Name = "American Cheese (Culver's)", Category = "BurgerTopping", Calories = 70, Protein = 3, Carbs = 1, Fat = 6, Sodium = 125 },
            new() { Name = "Swiss Cheese", Category = "BurgerTopping", Calories = 50, Protein = 4, Carbs = 1, Fat = 4, Sodium = 150 },
            new() { Name = "Cheddar Cheese", Category = "BurgerTopping", Calories = 70, Protein = 4, Carbs = 0, Fat = 6, Sodium = 115 },
            new() { Name = "Bacon (2 slices)", Category = "BurgerTopping", Calories = 100, Protein = 9, Carbs = 0, Fat = 6, Sodium = 380 },
            new() { Name = "Mayo (Culver's)", Category = "BurgerTopping", Calories = 80, Protein = 0, Carbs = 1, Fat = 8, Sodium = 75 },
            new() { Name = "Ketchup (Culver's)", Category = "BurgerTopping", Calories = 30, Protein = 0, Carbs = 8, Fat = 0, Sodium = 250 },
            new() { Name = "Mustard", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 120 }
        };
        db.CustomOptions.AddRange(options);
        await db.SaveChangesAsync();

        // 4. Create Menu Item <-> Option links based on category rules
        var links = new List<MenuItemOption>();

        foreach (var item in menuItems)
        {
            var category = item.Category.ToLower();

            // Beverages get coffee options
            if (category.Contains("beverage"))
            {
                var coffeeOptions = options.Where(o =>
                    o.Category == "MilkType" ||
                    o.Category == "Shot" ||
                    o.Category == "Swirl" ||
                    o.Category == "Sweetener");

                foreach (var opt in coffeeOptions)
                    links.Add(new MenuItemOption { MenuItemId = item.Id, CustomOptionId = opt.Id });
            }

            // Burgers and sandwiches get toppings/sauces
            if (category.Contains("burger") || category.Contains("sandwich"))
            {
                var burgerOptions = options.Where(o =>
                    o.Category == "BurgerTopping" ||
                    o.Category == "Topping" ||
                    o.Category == "Sauce");

                foreach (var opt in burgerOptions)
                    links.Add(new MenuItemOption { MenuItemId = item.Id, CustomOptionId = opt.Id });
            }

            // Potatoes get potato toppings
            if (category.Contains("potato"))
            {
                var potatoOptions = options.Where(o => o.Category == "Potato");

                foreach (var opt in potatoOptions)
                    links.Add(new MenuItemOption { MenuItemId = item.Id, CustomOptionId = opt.Id });
            }
        }

        db.MenuItemOptions.AddRange(links);
        await db.SaveChangesAsync();
    }
}
