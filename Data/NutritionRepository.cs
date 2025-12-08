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
    {
        private readonly SQLiteAsyncConnection _db;

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

            await SeedRestaurantsAsync();
            await SeedMenuItemsAsync();
            await SeedCustomOptionsAsync();
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

        public Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return _db.Table<Restaurant>()
                      .OrderBy(r => r.Name)
                      .ToListAsync();
        }

        public Task<int> GetTodaySelectionCountAsync()
        {
            var today = DateTime.Today;

            return _db.Table<LoggedItem>()
                      .Where(x => x.LoggedAt >= today)
                      .CountAsync();
        }

        public Task<List<MenuItem>> GetMenuItemsForRestaurantAsync(int restaurantId)
        {
            return _db.Table<MenuItem>()
                      .Where(m => m.RestaurantId == restaurantId)
                      .OrderBy(m => m.Name)
                      .ToListAsync();
        }
        public Task<MenuItem> GetMenuItemAsync(int id) =>
    _db.Table<MenuItem>().FirstAsync(m => m.Id == id);

        public Task<int> InsertLoggedItemAsync(LoggedItem item)
        {
            return _db.InsertAsync(item);
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

        public Task<List<LoggedItem>> GetLoggedItemsForTodayAsync()
        {
            var today = DateTime.Today;

            return _db.Table<LoggedItem>()
                      .Where(x => x.LoggedAt >= today)
                      .OrderBy(x => x.LoggedAt)
                      .ToListAsync();
        }

        // get the custom options for a given menu item
        public async Task<List<CustomOption>> GetCustomOptionsForMenuItemAsync(int menuItemId)
        {
            var links = await _db.Table<MenuItemOption>()
                                 .Where(l => l.MenuItemId == menuItemId)
                                 .ToListAsync();

            var optionIds = links.Select(l => l.CustomOptionId).ToList();

            return await _db.Table<CustomOption>()
                            .Where(o => optionIds.Contains(o.Id))
                            .ToListAsync();
        }

        //Insert option logs
        public Task<int> InsertLoggedItemOptionAsync(int loggedItemId, int optionId) =>
    _db.InsertAsync(new LoggedItemOption
    {
        LoggedItemId = loggedItemId,
        CustomOptionId = optionId
    });



        public Task<int> ClearLoggedItemsForTodayAsync()
        {
            var today = DateTime.Today;

            return _db.Table<LoggedItem>()
                      .Where(x => x.LoggedAt >= today)
                      .DeleteAsync();
        }
    }
}

