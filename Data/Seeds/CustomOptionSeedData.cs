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
                new CustomOption { Name = "Lettuce", Category = "Topping", Calories = 1, Protein = 0, Carbs = 0.1M, Fat = 0, Sodium = 1 },
                new CustomOption { Name = "Tomato", Category = "Topping", Calories = 3, Protein = 0.2M, Carbs = 0.7M, Fat = 0, Sodium = 1 },
                new CustomOption { Name = "Onion", Category = "Topping", Calories = 2, Protein = 0.1M, Carbs = 0.5M, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Pickles", Category = "Topping", Calories = 1, Protein = 0, Carbs = 0, Fat = 0, Sodium = 52 },
                new CustomOption { Name = "Cheese Slice", Category = "Topping", Calories = 50, Protein = 3, Carbs = 1, Fat = 4, Sodium = 180 },
                new CustomOption { Name = "Bacon", Category = "Topping", Calories = 40, Protein = 3, Carbs = 0, Fat = 3, Sodium = 140 },
                new CustomOption { Name = "Mayo", Category = "Sauce", Calories = 60, Protein = 0, Carbs = 0, Fat = 7, Sodium = 70 },
                new CustomOption { Name = "Ranch", Category = "Sauce", Calories = 40, Protein = 0, Carbs = 1, Fat = 4, Sodium = 90 },
                new CustomOption { Name = "Sour Cream", Category = "Potato", Calories = 20, Protein = 1, Carbs = 1, Fat = 1, Sodium = 10 },
                new CustomOption { Name = "Butter", Category = "Potato", Calories = 50, Protein = 0, Carbs = 0, Fat = 6, Sodium = 0 },
                new CustomOption { Name = "Chives", Category = "Potato", Calories = 1, Protein = 0, Carbs = 0, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Chocolate Syrup", Category = "Frosty", Calories = 60, Protein = 1, Carbs = 10, Fat = 1, Sodium = 10 },

                // ======================
                // Dunkin Options
                // ======================
                new CustomOption { Name = "Whole Milk", Category = "MilkType", Calories = 20, Protein = 1, Carbs = 1, Fat = 1, Sodium = 10 },
                new CustomOption { Name = "Skim Milk", Category = "MilkType", Calories = 10, Protein = 1, Carbs = 1, Fat = 0, Sodium = 10 },
                new CustomOption { Name = "Almond Milk", Category = "MilkType", Calories = 15, Protein = 0, Carbs = 1, Fat = 1, Sodium = 30 },
                new CustomOption { Name = "Oat Milk", Category = "MilkType", Calories = 25, Protein = 0, Carbs = 2, Fat = 1, Sodium = 20 },
                new CustomOption { Name = "Vanilla Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Hazelnut Shot", Category = "Shot", Calories = 5, Protein = 0, Carbs = 1, Fat = 0, Sodium = 0 },
                new CustomOption { Name = "Mocha Shot", Category = "Shot", Calories = 10, Protein = 0, Carbs = 2, Fat = 0, Sodium = 2 },
                new CustomOption { Name = "Caramel Swirl", Category = "Swirl", Calories = 40, Protein = 0, Carbs = 10, Fat = 0, Sodium = 5 },
                new CustomOption { Name = "Mocha Swirl", Category = "Swirl", Calories = 45, Protein = 0, Carbs = 11, Fat = 0, Sodium = 5 },
                new CustomOption { Name = "Sugar", Category = "Sweetener", Calories = 15, Protein = 0, Carbs = 4, Fat = 0, Sodium = 0 },

                // ======================
                // Wingstop Options
                // ======================
                new CustomOption { Name = "Mild", Category = "WingFlavor", Calories = 10, Protein = 0, Carbs = 1, Fat = 1, Sodium = 120 },
                new CustomOption { Name = "Original Hot", Category = "WingFlavor", Calories = 10, Protein = 0, Carbs = 1, Fat = 1, Sodium = 150 },
                new CustomOption { Name = "Lemon Pepper", Category = "WingFlavor", Calories = 15, Protein = 0, Carbs = 1, Fat = 1, Sodium = 160 },
                new CustomOption { Name = "Cajun", Category = "WingFlavor", Calories = 15, Protein = 0, Carbs = 1, Fat = 1, Sodium = 180 },
                new CustomOption { Name = "Garlic Parmesan", Category = "WingFlavor", Calories = 30, Protein = 1, Carbs = 1, Fat = 2, Sodium = 210 },
                new CustomOption { Name = "BBQ", Category = "WingFlavor", Calories = 20, Protein = 0, Carbs = 4, Fat = 0, Sodium = 150 },
                new CustomOption { Name = "Spicy Korean Q", Category = "WingFlavor", Calories = 25, Protein = 0, Carbs = 5, Fat = 0, Sodium = 180 },

                // ======================
                // Culver’s Options
                // ======================
                new CustomOption { Name = "Pickles", Category = "BurgerTopping", Calories = 1, Protein = 0, Carbs = 0, Fat = 0, Sodium = 52 },
                new CustomOption { Name = "Onions", Category = "BurgerTopping", Calories = 3, Protein = 0, Carbs = 1, Fat = 0, Sodium = 1 },
                new CustomOption { Name = "Cheddar Cheese", Category = "BurgerTopping", Calories = 60, Protein = 4, Carbs = 0, Fat = 5, Sodium = 180 },
                new CustomOption { Name = "Swiss Cheese", Category = "BurgerTopping", Calories = 50, Protein = 4, Carbs = 1, Fat = 4, Sodium = 150 },
                new CustomOption { Name = "Bacon", Category = "BurgerTopping", Calories = 40, Protein = 3, Carbs = 0, Fat = 3, Sodium = 140 },
                new CustomOption { Name = "Mayo", Category = "BurgerTopping", Calories = 60, Protein = 0, Carbs = 0, Fat = 7, Sodium = 70 }
            };
        }
    }
}
