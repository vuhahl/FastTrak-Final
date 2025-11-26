using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastTrak.Models;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.Data.Seeds
{
    public static class MenuItemSeedData
    {
            public static List<MenuItem> CreateMenuItems()
            {
                return new List<MenuItem>
            {
                // ============================
                //          WENDY'S (1)
                // ============================
                new MenuItem { RestaurantId = 1, Name = "Dave's Single", Calories = 524, Protein = 28.0, Carbs = 37.0, Fat = 29.0, Sodium = 2200, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Dave's Double", Calories = 879, Protein = 54.0, Carbs = 39.0, Fat = 56.0, Sodium = 4400, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Dave's Triple", Calories = 1195, Protein = 77.0, Carbs = 41.0, Fat = 80.0, Sodium = 6100, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Baconator", Calories = 1001, Protein = 63.0, Carbs = 38.0, Fat = 66.0, Sodium = 5800, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Curry Bean Burger", Calories = 535, Protein = 17.0, Carbs = 57.0, Fat = 25.0, Sodium = 2800, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Spicy Chicken Sandwich", Calories = 400, Protein = 20.0, Carbs = 45.0, Fat = 15.0, Sodium = 2200, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "Classic Chicken Sandwich", Calories = 404, Protein = 20.0, Carbs = 46.0, Fat = 15.0, Sodium = 2500, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "Grilled Chicken Sandwich", Calories = 337, Protein = 26.0, Carbs = 35.0, Fat = 10.0, Sodium = 1600, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "Avocado Chicken Club", Calories = 583, Protein = 30.0, Carbs = 47.0, Fat = 30.0, Sodium = 3400, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "4 Pc Chicken Nuggets", Calories = 114, Protein = 11.0, Carbs = 4.9, Fat = 5.6, Sodium = 930, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "10 Pc Chicken Nuggets", Calories = 286, Protein = 28.0, Carbs = 12.0, Fat = 14.0, Sodium = 2300, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Cheeseburger Deluxe", Calories = 366, Protein = 17.0, Carbs = 31.0, Fat = 19.0, Sodium = 1900, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Bacon Cheeseburger", Calories = 410, Protein = 20.0, Carbs = 30.0, Fat = 23.0, Sodium = 2300, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Caesar Chicken Salad", Calories = 411, Protein = 28.0, Carbs = 4.2, Fat = 31.0, Sodium = 2000, Category = "Salads" },
                new MenuItem { RestaurantId = 1, Name = "Avocado Chicken Salad", Calories = 485, Protein = 32.0, Carbs = 7.5, Fat = 36.0, Sodium = 2200, Category = "Salads" },
                new MenuItem { RestaurantId = 1, Name = "Value Fries", Calories = 142, Protein = 1.6, Carbs = 18.0, Fat = 6.7, Sodium = 2100, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Medium Fries", Calories = 176, Protein = 2.0, Carbs = 22.0, Fat = 8.5, Sodium = 2100, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Baked Potato (Plain)", Calories = 232, Protein = 4.4, Carbs = 39.0, Fat = 5.4, Sodium = 1900, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Chili", Calories = 253, Protein = 19.0, Carbs = 14.0, Fat = 13.0, Sodium = 1500, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Jr. Vanilla Frosty", Calories = 179, Protein = 3.5, Carbs = 27.0, Fat = 6.3, Sodium = 340, Category = "Desserts" },
                new MenuItem { RestaurantId = 1, Name = "Jr. Chocolate Frosty", Calories = 174, Protein = 5.2, Carbs = 28.0, Fat = 4.6, Sodium = 200, Category = "Desserts" },

                // ============================
                //           DUNKIN (2)
                // ============================
                new MenuItem { RestaurantId = 2, Name = "Iced Coffee with Cream & Sugar - Small", Calories = 130, Protein = 1.0, Carbs = 18.0, Fat = 6.0, Sodium = 0, Category = "Beverages" },
                new MenuItem { RestaurantId = 2, Name = "Iced Coffee with Cream & Sugar - Medium", Calories = 190, Protein = 2.0, Carbs = 27.0, Fat = 9.0, Sodium = 0, Category = "Beverages" },
                new MenuItem { RestaurantId = 2, Name = "Iced Coffee with Cream & Sugar - Large", Calories = 260, Protein = 2.0, Carbs = 37.0, Fat = 12.0, Sodium = 0, Category = "Beverages" },
                new MenuItem { RestaurantId = 2, Name = "Iced Coffee with Cream - Medium", Calories = 90, Protein = 2.0, Carbs = 1.0, Fat = 9.0, Sodium = 0, Category = "Beverages" },
                new MenuItem { RestaurantId = 2, Name = "Old Fashioned Donut", Calories = 310, Protein = 4.0, Carbs = 30.0, Fat = 19.0, Sodium = 0, Category = "Donuts" },
                new MenuItem { RestaurantId = 2, Name = "Chocolate Frosted Donut", Calories = 280, Protein = 3.0, Carbs = 31.0, Fat = 15.0, Sodium = 0, Category = "Donuts" },
                new MenuItem { RestaurantId = 2, Name = "Sausage Egg & Cheese on Croissant", Calories = 610, Protein = 22.5, Carbs = 29.8, Fat = 44.1, Sodium = 0, Category = "Breakfast" },
                new MenuItem { RestaurantId = 2, Name = "Sausage Egg & Cheese on Bagel", Calories = 680, Protein = 26.0, Carbs = 68.0, Fat = 34.0, Sodium = 1500, Category = "Breakfast" },

                // ============================
                //          WINGSTOP (3)
                // ============================
                new MenuItem { RestaurantId = 3, Name = "Mild Classic Wing (1 wing)", Calories = 110, Protein = 4.0, Carbs = 6.0, Fat = 7.0, Sodium = 330, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Original Hot Classic Wing (1 wing)", Calories = 80, Protein = 4.0, Carbs = 6.0, Fat = 4.5, Sodium = 390, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Plain Classic Wing (1 wing)", Calories = 80, Protein = 4.0, Carbs = 6.0, Fat = 4.5, Sodium = 230, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Spicy Korean Q Classic Wing (1 wing)", Calories = 90, Protein = 5.0, Carbs = 8.0, Fat = 4.5, Sodium = 320, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Seasoned Fries - Regular 10oz", Calories = 500, Protein = 8.0, Carbs = 69.0, Fat = 21.0, Sodium = 620, Category = "Sides" },
                new MenuItem { RestaurantId = 3, Name = "Seasoned Fries - Large 18oz", Calories = 900, Protein = 14.0, Carbs = 126.0, Fat = 37.0, Sodium = 1060, Category = "Sides" },
                new MenuItem { RestaurantId = 3, Name = "Cajun Fried Corn - Regular", Calories = 300, Protein = 5.0, Carbs = 34.0, Fat = 17.0, Sodium = 660, Category = "Sides" },
                new MenuItem { RestaurantId = 3, Name = "Ranch Dip - 3.25oz Cup", Calories = 320, Protein = 1.0, Carbs = 2.0, Fat = 34.0, Sodium = 870, Category = "Sauces" },
                new MenuItem { RestaurantId = 3, Name = "Blue Cheese Dip - 3.25oz Cup", Calories = 330, Protein = 4.0, Carbs = 4.0, Fat = 33.0, Sodium = 570, Category = "Sauces" },
                new MenuItem { RestaurantId = 3, Name = "Honey Mustard Dip - 3.25oz Cup", Calories = 390, Protein = 0.0, Carbs = 18.0, Fat = 33.0, Sodium = 660, Category = "Sauces" },
                new MenuItem { RestaurantId = 3, Name = "Cheddar Cheese Dip - 3.25oz Cup", Calories = 120, Protein = 2.0, Carbs = 8.0, Fat = 9.0, Sodium = 850, Category = "Sauces" },
                new MenuItem { RestaurantId = 3, Name = "Boneless Tender (1 piece)", Calories = 140, Protein = 10.0, Carbs = 10.0, Fat = 7.0, Sodium = 470, Category = "Wings" },

                // ============================
                //          CULVER'S (4)
                // ============================
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Original - Single", Calories = 390, Protein = 20.0, Carbs = 38.0, Fat = 17.0, Sodium = 480, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Original - Double", Calories = 560, Protein = 34.0, Carbs = 38.0, Fat = 30.0, Sodium = 580, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Original - Triple", Calories = 730, Protein = 48.0, Carbs = 38.0, Fat = 43.0, Sodium = 680, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Cheese - Single", Calories = 460, Protein = 23.0, Carbs = 39.0, Fat = 23.0, Sodium = 605, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Cheese - Double", Calories = 700, Protein = 40.0, Carbs = 40.0, Fat = 42.0, Sodium = 830, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "Cheddar ButterBurger - Single", Calories = 470, Protein = 25.0, Carbs = 38.0, Fat = 24.0, Sodium = 620, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "Cheddar ButterBurger with Bacon - Single", Calories = 510, Protein = 28.0, Carbs = 38.0, Fat = 26.0, Sodium = 775, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "Mushroom & Swiss ButterBurger - Single", Calories = 500, Protein = 26.0, Carbs = 40.0, Fat = 26.0, Sodium = 582, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "Crispy Chicken Sandwich", Calories = 460, Protein = 25.0, Carbs = 57.0, Fat = 14.0, Sodium = 1105, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 4, Name = "Grilled Chicken Sandwich", Calories = 390, Protein = 41.0, Carbs = 40.0, Fat = 8.0, Sodium = 1070, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 4, Name = "Spicy Crispy Chicken Sandwich", Calories = 490, Protein = 25.0, Carbs = 57.0, Fat = 17.0, Sodium = 1470, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 4, Name = "Veggie Burger Sandwich", Calories = 580, Protein = 13.0, Carbs = 65.0, Fat = 30.0, Sodium = 1150, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 4, Name = "Crinkle Cut Fries - Small", Calories = 240, Protein = 4.0, Carbs = 35.0, Fat = 9.0, Sodium = 350, Category = "Sides" },
                new MenuItem { RestaurantId = 4, Name = "Crinkle Cut Fries - Regular", Calories = 360, Protein = 6.0, Carbs = 53.0, Fat = 14.0, Sodium = 530, Category = "Sides" },
                new MenuItem { RestaurantId = 4, Name = "Wisconsin Cheese Curds", Calories = 510, Protein = 20.0, Carbs = 51.0, Fat = 25.0, Sodium = 1230, Category = "Sides" }
            };
            }
        }
    }

