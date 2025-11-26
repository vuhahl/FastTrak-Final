using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastTrak.Models;
using SQLite;
using FastTrak.Data.Seeds;
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

            // Seed base restaurant list
            await SeedRestaurantsAsync();

            // Seed menu items from MenuItemSeedData.cs
            await SeedMenuItemsAsync();
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

        public Task<int> InsertLoggedItemAsync(LoggedItem item)
        {
            return _db.InsertAsync(item);
        }

        public Task<List<LoggedItem>> GetLoggedItemsForTodayAsync()
        {
            var today = DateTime.Today;

            return _db.Table<LoggedItem>()
                      .Where(x => x.LoggedAt >= today)
                      .OrderBy(x => x.LoggedAt)
                      .ToListAsync();
        }

        public async Task ClearTodayAsync()
        {
            var today = DateTime.Today;

            var items = await _db.Table<LoggedItem>()
                                 .Where(x => x.LoggedAt >= today)
                                 .ToListAsync();

            foreach (var item in items)
                await _db.DeleteAsync(item);
        }
    }
}

