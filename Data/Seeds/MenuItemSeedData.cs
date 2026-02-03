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
                // Note: Wendy's guide shows salt in grams. Sodium (mg) = Salt (g) × 393.4
                new MenuItem { RestaurantId = 1, Name = "Dave's Single", Calories = 524, Protein = 28.0M, Carbs = 37.0M, Fat = 29.0M, Sodium = 866, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Dave's Double", Calories = 879, Protein = 54.0M, Carbs = 39.0M, Fat = 56.0M, Sodium = 1731, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Dave's Triple", Calories = 1195, Protein = 77.0M, Carbs = 41.0M, Fat = 80.0M, Sodium = 2400, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Baconator", Calories = 1001, Protein = 63.0M, Carbs = 38.0M, Fat = 66.0M, Sodium = 2282, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Curry Bean Burger", Calories = 535, Protein = 17.0M, Carbs = 57.0M, Fat = 25.0M, Sodium = 1102, Category = "Burgers" },

                new MenuItem { RestaurantId = 1, Name = "Spicy Chicken Sandwich", Calories = 400, Protein = 20.0M, Carbs = 45.0M, Fat = 15.0M, Sodium = 865, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "Classic Chicken Sandwich", Calories = 404, Protein = 20.0M, Carbs = 46.0M, Fat = 15.0M, Sodium = 984, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "Grilled Chicken Sandwich", Calories = 337, Protein = 26.0M, Carbs = 35.0M, Fat = 10.0M, Sodium = 630, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 1, Name = "Avocado Chicken Club", Calories = 583, Protein = 30.0M, Carbs = 47.0M, Fat = 30.0M, Sodium = 1338, Category = "Sandwiches" },

                new MenuItem { RestaurantId = 1, Name = "4 Pc Chicken Nuggets", Calories = 114, Protein = 11.0M, Carbs = 4.9M, Fat = 5.6M, Sodium = 366, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "10 Pc Chicken Nuggets", Calories = 286, Protein = 28.0M, Carbs = 12.0M, Fat = 14.0M, Sodium = 905, Category = "Sides" },

                new MenuItem { RestaurantId = 1, Name = "Cheeseburger Deluxe", Calories = 366, Protein = 17.0M, Carbs = 31.0M, Fat = 19.0M, Sodium = 748, Category = "Burgers" },
                new MenuItem { RestaurantId = 1, Name = "Bacon Cheeseburger", Calories = 410, Protein = 20.0M, Carbs = 30.0M, Fat = 23.0M, Sodium = 905, Category = "Burgers" },

                new MenuItem { RestaurantId = 1, Name = "Caesar Chicken Salad", Calories = 411, Protein = 28.0M, Carbs = 4.2M, Fat = 31.0M, Sodium = 826, Category = "Salads" },
                new MenuItem { RestaurantId = 1, Name = "Avocado Chicken Salad", Calories = 485, Protein = 32.0M, Carbs = 7.5M, Fat = 36.0M, Sodium = 866, Category = "Salads" },

                new MenuItem { RestaurantId = 1, Name = "Value Fries", Calories = 142, Protein = 1.6M, Carbs = 18.0M, Fat = 6.7M, Sodium = 826, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Medium Fries", Calories = 176, Protein = 2.0M, Carbs = 22.0M, Fat = 8.5M, Sodium = 826, Category = "Sides" },
                new MenuItem { RestaurantId = 1, Name = "Large Fries", Calories = 239, Protein = 2.7M, Carbs = 31.0M, Fat = 11.0M, Sodium = 944, Category = "Sides" },

                new MenuItem { RestaurantId = 1, Name = "Plain Baked Potato", Calories = 232, Protein = 4.4M, Carbs = 39.0M, Fat = 5.4M, Sodium = 748, Category = "Potato" },
                new MenuItem { RestaurantId = 1, Name = "Chili", Calories = 253, Protein = 19.0M, Carbs = 14.0M, Fat = 13.0M, Sodium = 590, Category = "Sides" },

                new MenuItem { RestaurantId = 1, Name = "Jr. Vanilla Frosty", Calories = 179, Protein = 3.5M, Carbs = 27.0M, Fat = 6.3M, Sodium = 134, Category = "Desserts" },
                new MenuItem { RestaurantId = 1, Name = "Jr. Chocolate Frosty", Calories = 174, Protein = 5.2M, Carbs = 28.0M, Fat = 4.6M, Sodium = 79, Category = "Desserts" },

                // ============================
                //           DUNKIN (2)
                // ============================
                // Base coffee items - users will add milk/sugar/shots as customizations
                new MenuItem { RestaurantId = 2, Name = "Hot Coffee - Medium", Calories = 5, Protein = 0.0M, Carbs = 0.0M, Fat = 0.0M, Sodium = 10, Category = "Beverages" },
                new MenuItem { RestaurantId = 2, Name = "Iced Coffee - Medium", Calories = 5, Protein = 0.0M, Carbs = 0.0M, Fat = 0.0M, Sodium = 15, Category = "Beverages" },
                new MenuItem { RestaurantId = 2, Name = "Cold Brew - Medium", Calories = 5, Protein = 0.0M, Carbs = 0.0M, Fat = 0.0M, Sodium = 15, Category = "Beverages" },

                new MenuItem { RestaurantId = 2, Name = "Glazed Donut", Calories = 240, Protein = 4.0M, Carbs = 33.0M, Fat = 11.0M, Sodium = 270, Category = "Donuts" },
                new MenuItem { RestaurantId = 2, Name = "Chocolate Frosted Donut", Calories = 260, Protein = 4.0M, Carbs = 34.0M, Fat = 11.0M, Sodium = 290, Category = "Donuts" },
                new MenuItem { RestaurantId = 2, Name = "Old Fashioned Donut", Calories = 310, Protein = 4.0M, Carbs = 30.0M, Fat = 19.0M, Sodium = 320, Category = "Donuts" },
                new MenuItem { RestaurantId = 2, Name = "Boston Kreme Donut", Calories = 270, Protein = 5.0M, Carbs = 39.0M, Fat = 11.0M, Sodium = 320, Category = "Donuts" },

                new MenuItem { RestaurantId = 2, Name = "Bacon, Egg and Cheese on Croissant", Calories = 520, Protein = 19.0M, Carbs = 34.0M, Fat = 34.0M, Sodium = 870, Category = "Breakfast" },
                new MenuItem { RestaurantId = 2, Name = "Sausage, Egg and Cheese on Croissant", Calories = 610, Protein = 22.5M, Carbs = 29.8M, Fat = 44.1M, Sodium = 1100, Category = "Breakfast" },
                new MenuItem { RestaurantId = 2, Name = "Egg and Cheese on English Muffin", Calories = 340, Protein = 14.0M, Carbs = 38.0M, Fat = 15.0M, Sodium = 650, Category = "Breakfast" },

                // ============================
                //          WINGSTOP (3)
                // ============================
                // Classic Wings - THESE ALREADY INCLUDE THE FLAVOR
                new MenuItem { RestaurantId = 3, Name = "Mild Classic Wing (1 wing)", Calories = 120, Protein = 10.0M, Carbs = 0.0M, Fat = 8.0M, Sodium = 160, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Original Hot Classic Wing (1 wing)", Calories = 90, Protein = 10.0M, Carbs = 0.0M, Fat = 5.0M, Sodium = 230, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Plain Classic Wing (1 wing)", Calories = 90, Protein = 10.0M, Carbs = 0.0M, Fat = 5.0M, Sodium = 30, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Lemon Pepper Classic Wing (1 wing)", Calories = 120, Protein = 10.0M, Carbs = 0.0M, Fat = 8.0M, Sodium = 210, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Garlic Parmesan Classic Wing (1 wing)", Calories = 120, Protein = 10.0M, Carbs = 1.0M, Fat = 8.0M, Sodium = 75, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Cajun Classic Wing (1 wing)", Calories = 90, Protein = 10.0M, Carbs = 0.0M, Fat = 5.0M, Sodium = 310, Category = "Wings" },
                
                // Boneless Wings - THESE ALREADY INCLUDE THE FLAVOR
                new MenuItem { RestaurantId = 3, Name = "Mild Boneless Wing (1 piece)", Calories = 110, Protein = 4.0M, Carbs = 6.0M, Fat = 7.0M, Sodium = 330, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Original Hot Boneless Wing (1 piece)", Calories = 80, Protein = 4.0M, Carbs = 6.0M, Fat = 4.5M, Sodium = 390, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Plain Boneless Wing (1 piece)", Calories = 80, Protein = 4.0M, Carbs = 6.0M, Fat = 4.5M, Sodium = 230, Category = "Wings" },
                new MenuItem { RestaurantId = 3, Name = "Lemon Pepper Boneless Wing (1 piece)", Calories = 110, Protein = 4.0M, Carbs = 6.0M, Fat = 7.0M, Sodium = 290, Category = "Wings" },

                new MenuItem { RestaurantId = 3, Name = "Seasoned Fries - Regular 10oz", Calories = 500, Protein = 8.0M, Carbs = 69.0M, Fat = 21.0M, Sodium = 620, Category = "Sides" },
                new MenuItem { RestaurantId = 3, Name = "Seasoned Fries - Large 18oz", Calories = 900, Protein = 14.0M, Carbs = 126.0M, Fat = 37.0M, Sodium = 1060, Category = "Sides" },

                new MenuItem { RestaurantId = 3, Name = "Ranch Dip - 3.25oz Cup", Calories = 320, Protein = 1.0M, Carbs = 2.0M, Fat = 34.0M, Sodium = 870, Category = "Sauces" },
                new MenuItem { RestaurantId = 3, Name = "Blue Cheese Dip - 3.25oz Cup", Calories = 330, Protein = 4.0M, Carbs = 4.0M, Fat = 33.0M, Sodium = 570, Category = "Sauces" },
                new MenuItem { RestaurantId = 3, Name = "Honey Mustard Dip - 3.25oz Cup", Calories = 390, Protein = 0.0M, Carbs = 18.0M, Fat = 33.0M, Sodium = 660, Category = "Sauces" },

                // ============================
                //          CULVER'S (4)
                // ============================
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Original - Single", Calories = 390, Protein = 20.0M, Carbs = 38.0M, Fat = 17.0M, Sodium = 480, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Original - Double", Calories = 560, Protein = 34.0M, Carbs = 38.0M, Fat = 30.0M, Sodium = 580, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Original - Triple", Calories = 730, Protein = 48.0M, Carbs = 38.0M, Fat = 43.0M, Sodium = 680, Category = "Burgers" },

                new MenuItem { RestaurantId = 4, Name = "ButterBurger Cheese - Single", Calories = 460, Protein = 24.0M, Carbs = 39.0M, Fat = 23.0M, Sodium = 700, Category = "Burgers" },
                new MenuItem { RestaurantId = 4, Name = "ButterBurger Cheese - Double", Calories = 700, Protein = 41.0M, Carbs = 40.0M, Fat = 42.0M, Sodium = 1020, Category = "Burgers" },

                new MenuItem { RestaurantId = 4, Name = "Mushroom & Swiss ButterBurger - Single", Calories = 530, Protein = 27.0M, Carbs = 41.0M, Fat = 28.0M, Sodium = 650, Category = "Burgers" },

                new MenuItem { RestaurantId = 4, Name = "Crispy Chicken Sandwich", Calories = 690, Protein = 28.0M, Carbs = 65.0M, Fat = 35.0M, Sodium = 1590, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 4, Name = "Grilled Chicken Sandwich", Calories = 480, Protein = 36.0M, Carbs = 40.0M, Fat = 19.0M, Sodium = 1340, Category = "Sandwiches" },
                new MenuItem { RestaurantId = 4, Name = "Spicy Crispy Chicken Sandwich", Calories = 680, Protein = 32.0M, Carbs = 65.0M, Fat = 33.0M, Sodium = 1680, Category = "Sandwiches" },

                new MenuItem { RestaurantId = 4, Name = "Crinkle Cut Fries - Small", Calories = 220, Protein = 3.0M, Carbs = 32.0M, Fat = 9.0M, Sodium = 410, Category = "Sides" },
                new MenuItem { RestaurantId = 4, Name = "Crinkle Cut Fries - Medium", Calories = 350, Protein = 4.0M, Carbs = 50.0M, Fat = 14.0M, Sodium = 650, Category = "Sides" },
                new MenuItem { RestaurantId = 4, Name = "Crinkle Cut Fries - Large", Calories = 430, Protein = 5.0M, Carbs = 62.0M, Fat = 18.0M, Sodium = 810, Category = "Sides" },

                new MenuItem { RestaurantId = 4, Name = "Wisconsin Cheese Curds - Medium", Calories = 490, Protein = 17.0M, Carbs = 46.0M, Fat = 27.0M, Sodium = 1520, Category = "Sides" }
            };
        }
    }
    }

