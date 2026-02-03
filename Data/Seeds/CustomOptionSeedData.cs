using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastTrak.Models;

namespace FastTrak.Data.Seeds
{
    public class CustomOptionSeedData
    {
        public static List<CustomOption> CreateOptions()
        {
            return new List<CustomOption>
            {
                // ======================
                // Wendy's Options
                // ======================
                new CustomOption { Name = "Lettuce", Category = "Topping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Tomato", Category = "Topping", Calories = 3, Protein = 0, Carbs = 0.8M, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Onion", Category = "Topping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Pickles", Category = "Topping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 87 },
                new CustomOption { Name = "American Cheese", Category = "Topping", Calories = 42, Protein = 2, Carbs = 1, Fat = 3, Sodium = 193 },
                new CustomOption { Name = "Bacon (2 strips)", Category = "Topping", Calories = 42, Protein = 3, Carbs = 0, Fat = 3, Sodium = 189 },
                new CustomOption { Name = "Mayo", Category = "Sauce", Calories = 50, Protein = 0, Carbs = 0, Fat = 5.6M, Sodium = 39 },
                new CustomOption { Name = "Ketchup", Category = "Sauce", Calories = 11, Protein = 0, Carbs = 2.8M, Fat = 0, Sodium = 87 },
                
                // Potato toppings
                new CustomOption { Name = "Sour Cream", Category = "Potato", Calories = 52, Protein = 0.7M, Carbs = 1.1M, Fat = 5, Sodium = 16 },
                new CustomOption { Name = "Butter", Category = "Potato", Calories = 50, Protein = 0, Carbs = 0, Fat = 6, Sodium = 0 },
                new CustomOption { Name = "Chives", Category = "Potato", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Shredded Cheese", Category = "Potato", Calories = 36, Protein = 2, Carbs = 0, Fat = 3, Sodium = 67 },

                // ======================
                // Dunkin Options
                // ======================
                // Milk options (per medium serving)
                new CustomOption { Name = "Whole Milk", Category = "MilkType", Calories = 30, Protein = 2, Carbs = 2, Fat = 1.5M, Sodium = 25 },
                new CustomOption { Name = "Skim Milk", Category = "MilkType", Calories = 20, Protein = 2, Carbs = 2, Fat = 0, Sodium = 25 },
                new CustomOption { Name = "Almond Milk", Category = "MilkType", Calories = 25, Protein = 1, Carbs = 4, Fat = 0.5M, Sodium = 45 },
                new CustomOption { Name = "Oat Milk", Category = "MilkType", Calories = 30, Protein = 1, Carbs = 5, Fat = 1, Sodium = 35 },
                new CustomOption { Name = "Cream", Category = "MilkType", Calories = 90, Protein = 2, Carbs = 1, Fat = 9, Sodium = 40 },
                
                // Flavor shots (unsweetened)
                new CustomOption { Name = "Vanilla Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Hazelnut Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Toasted Almond Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
                
                // Flavor swirls (sweetened, medium serving)
                new CustomOption { Name = "Caramel Swirl", Category = "Swirl", Calories = 150, Protein = 0, Carbs = 38, Fat = 0, Sodium = 70 },
                new CustomOption { Name = "Mocha Swirl", Category = "Swirl", Calories = 140, Protein = 0, Carbs = 36, Fat = 0.5M, Sodium = 35 },
                new CustomOption { Name = "French Vanilla Swirl", Category = "Swirl", Calories = 150, Protein = 0, Carbs = 37, Fat = 0, Sodium = 50 },
                
                // Sweeteners
                new CustomOption { Name = "Sugar", Category = "Sweetener", Calories = 15, Protein = 0, Carbs = 4, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Liquid Sugar", Category = "Sweetener", Calories = 50, Protein = 0, Carbs = 13, Fat = 0, Sodium = 0 },

                // ======================
                // Culver's Options
                // ======================
                new CustomOption { Name = "Pickles", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 52 },
                new CustomOption { Name = "Onions", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Tomato", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Lettuce", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "American Cheese", Category = "BurgerTopping", Calories = 70, Protein = 3, Carbs = 1, Fat = 6, Sodium = 125 },
                new CustomOption { Name = "Swiss Cheese", Category = "BurgerTopping", Calories = 50, Protein = 4, Carbs = 1, Fat = 4, Sodium = 150 },
                new CustomOption { Name = "Cheddar Cheese", Category = "BurgerTopping", Calories = 70, Protein = 4, Carbs = 0, Fat = 6, Sodium = 115 },
                new CustomOption { Name = "Bacon (2 slices)", Category = "BurgerTopping", Calories = 100, Protein = 9, Carbs = 0, Fat = 6, Sodium = 380 },
                new CustomOption { Name = "Mayo", Category = "BurgerTopping", Calories = 80, Protein = 0, Carbs = 1, Fat = 8, Sodium = 75 },
                new CustomOption { Name = "Ketchup", Category = "BurgerTopping", Calories = 30, Protein = 0, Carbs = 8, Fat = 0, Sodium = 250 },
                new CustomOption { Name = "Mustard", Category = "BurgerTopping", Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sodium = 120 }
            };
        }
    }
}
