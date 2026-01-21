
using FastTrak.Data.Seeds;
using FastTrak.Models;
using Microsoft.Maui.Graphics;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.Data
{
    public class NutritionRepository

    /// <summary>
    /// Creates tables
    /// Inserts data, Retrieves data, Updates data, Deletes data
    /// </summary>
    {
        private readonly SQLiteAsyncConnection _db; //all  db actions go through this

        public NutritionRepository()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "fasttrak.db3");
            _db = new SQLiteAsyncConnection(path);

        }

        /// <summary>
        /// Initializes all database tables and seeds data.
        /// This runs ONCE when the app starts.
        /// </summary>
        public async Task InitializeAsync()
        {
            // Create all tables used by the app
            await _db.CreateTableAsync<Restaurant>();
            await _db.CreateTableAsync<MenuItem>();
            await _db.CreateTableAsync<LoggedItem>();
            await _db.CreateTableAsync<CustomOption>();
            await _db.CreateTableAsync<LoggedItemOption>();
            await _db.CreateTableAsync<MenuItemOption>();

            // Seed base restaurant list
            await SeedRestaurantsAsync();

            // Seed menu items from MenuItemSeedData.cs
            await SeedMenuItemsAsync();

            // Seed customization options (milk types, toppings, etc.)
            await SeedCustomOptionsAsync();

            // Seed junction table linking menu items to their available options
            // NOTE: This must run AFTER MenuItems and CustomOptions are seeded
            // because it queries the database to get the actual auto-generated IDs
            await SeedMenuItemOptionsAsync();
        }

        /// <summary>
        /// Seeds restaurants only if table is empty.
        /// </summary>
        private async Task SeedRestaurantsAsync()
        {
            if (await _db.Table<Restaurant>().CountAsync() > 0)
                return;

            var defaults = new List<Restaurant>
            {
                new Restaurant { Name = "Wendy's",  Slug = "wendys" },
                new Restaurant { Name = "Dunkin'",  Slug = "dunkin" },
                new Restaurant { Name = "Wingstop", Slug = "wingstop" },
                new Restaurant { Name = "Culver's", Slug = "culvers" }
            };

            await _db.InsertAllAsync(defaults);
        }

        /// <summary>
        /// Seeds menu items only if the MenuItem table is empty.
        /// Uses MenuItemSeedData.cs which contains all items.
        /// </summary>
        private async Task SeedMenuItemsAsync()
        {
            if (await _db.Table<MenuItem>().CountAsync() > 0)
                return;

            var items = MenuItemSeedData.CreateMenuItems();
            await _db.InsertAllAsync(items);
        }

        private async Task SeedCustomOptionsAsync()
        {
            if (await _db.Table<CustomOption>().CountAsync() > 0)
                return;

            var options = CustomOptionSeedData.CreateOptions();
            await _db.InsertAllAsync(options);
        }

        private async Task SeedMenuItemOptionsAsync()
        {
            if (await _db.Table<MenuItemOption>().CountAsync() > 0)
                return;

            var items = await _db.Table<MenuItem>().ToListAsync();
            var options = await _db.Table<CustomOption>().ToListAsync();

            var links = MenuItemOptionSeedData.CreateLinks(items, options);

            await _db.InsertAllAsync(links);
        }



        // =============================
        //     DATA-RETRIEVAL METHODS
        // =============================

        public Task<List<Restaurant>> GetRestaurantsAsync() //get restuarants and order by restuarant name, return the list asyncronously
        {
            return _db.Table<Restaurant>()
                      .OrderBy(r => r.Name)
                      .ToListAsync();
        }


        public async Task<List<LoggedItem>> GetLoggedItemsForTodayAsync() //get logged items for today, loads logged items made today, load custom options (calc page gets todays stuff only)
        {
            var today = DateTime.Today;

            var items = await _db.Table<LoggedItem>()
                                 .Where(x => x.LoggedAt >= today)
                                 .ToListAsync();

            
            foreach (var item in items)
            {
                item.Options = await _db.Table<LoggedItemOption>()
                                        .Where(o => o.LoggedItemId == item.Id)
                                        .ToListAsync();
            }

            return items;
        }

        public Task<List<MenuItem>> GetMenuItemsForRestaurantAsync(int restaurantId)
        {
            return _db.Table<MenuItem>() //get only the menu items for the selected restaurant
                      .Where(m => m.RestaurantId == restaurantId)
                      .OrderBy(m => m.Name)
                      .ToListAsync();
        }
        public Task<MenuItem> GetMenuItemAsync(int id) => //get a single menu item by its id, used by customization page
        _db.Table<MenuItem>().FirstAsync(m => m.Id == id);

        public Task<int> InsertLoggedItemAsync(LoggedItem item) //insert a new logged item into the db
        {
            return _db.InsertAsync(item); //insert the iten with foodname, quantty, calories info into the db
        }

        // Update an existing logged item (e.g., after quantity change)
        public Task<int> UpdateLoggedItemAsync(LoggedItem item)
        {
            return _db.UpdateAsync(item);
        }

        // Delete a single logged item
        public Task<int> DeleteLoggedItemAsync(int id)
        {
            return _db.DeleteAsync<LoggedItem>(id);
        }

        

        // get the custom options for a given menu item
        public async Task<List<CustomOption>> GetCustomOptionsForMenuItemAsync(int menuItemId)
        {
            var links = await _db.Table<MenuItemOption>() //bridge. which options are allowed for each menuitem?
                                 .Where(l => l.MenuItemId == menuItemId)
                                 .ToListAsync();

            var optionIds = links.Select(l => l.CustomOptionId).ToList();

            return await _db.Table<CustomOption>()
                            .Where(o => optionIds.Contains(o.Id))  //get the ID'S of the options that are linked to the menu item
                            .ToListAsync();
        }

        //Insert option logs

        //added 12/12 to account for the new LoggedItemOption model and calc customization macros
        public Task InsertLoggedItemOptionAsync(LoggedItemOption option)
        {
            return _db.InsertAsync(option);
        }



        public Task<int> ClearLoggedItemsForTodayAsync()
        {
            var today = DateTime.Today;

            return _db.Table<LoggedItem>()
                      .Where(x => x.LoggedAt >= today)
                      .DeleteAsync();
        }
    }
}

