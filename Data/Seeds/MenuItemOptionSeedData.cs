using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastTrak.Models;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.Data.Seeds
{
    public class MenuItemOptionSeedData
    {
        public static List<MenuItemOption> CreateLinks(
            List<MenuItem> menuItems,
            List<CustomOption> options)
        {
            var menuLinks = new List<MenuItemOption>();

            foreach (var item in menuItems)
            {
                // Normalize category (lowercase)
                var category = item.Category?.ToLower() ?? "";

                // ==============================
                // Apply rules based on category
                // ==============================

                // --- Coffee ---
                if (category.Contains("coffee") || category.Contains("beverage"))
                {
                    var coffeeOptions = options.Where(o =>
                        o.Category == "MilkType" ||
                        o.Category == "Shot" ||
                        o.Category == "Swirl" ||
                        o.Category == "Sweetener");

                    foreach (var opt in coffeeOptions)
                        menuLinks.Add(new MenuItemOption
                        {
                            MenuItemId = item.Id,
                            CustomOptionId = opt.Id
                        });
                }

                // --- Wings ---
                if (category.Contains("wing"))
                {
                    var wingOptions = options.Where(o => o.Category == "WingFlavor");

                    foreach (var opt in wingOptions)
                        menuLinks.Add(new MenuItemOption
                        {
                            MenuItemId = item.Id,
                            CustomOptionId = opt.Id
                        });
                }

                // --- Burgers / Sandwiches ---
                if (category.Contains("burger") || category.Contains("sandwich"))
                {
                    var burgerOptions = options.Where(o =>
                        o.Category == "BurgerTopping" ||
                        o.Category == "Topping" ||
                        o.Category == "Sauce");

                    foreach (var opt in burgerOptions)
                        menuLinks.Add(new MenuItemOption
                        {
                            MenuItemId = item.Id,
                            CustomOptionId = opt.Id
                        });
                }

                // --- Baked Potatoes ---
                if (category.Contains("potato"))
                {
                    var potatoOptions = options.Where(o =>
                        o.Category == "Potato");

                    foreach (var opt in potatoOptions)
                        menuLinks.Add(new MenuItemOption
                        {
                            MenuItemId = item.Id,
                            CustomOptionId = opt.Id
                        });
                }

                // --- Frosty / Ice Cream ---
                if (category.Contains("dessert"))
                {
                    var frostyOptions = options.Where(o =>
                        o.Category == "Frosty");

                    foreach (var opt in frostyOptions)
                        menuLinks.Add(new MenuItemOption
                        {
                            MenuItemId = item.Id,
                            CustomOptionId = opt.Id
                        });
                }
            }

            return menuLinks;
        }
    }
}
