using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastTrak.Models;
using SQLite;

namespace FastTrak.Data
{
    internal class NutritionRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public NutritionRepository()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "fasttrak.db3");
            _db = new SQLiteAsyncConnection(path);
        }

        /// <summary>
        /// Initializes all tables and seeds default data.
        /// Must be called on app startup.
        /// </summary>
        public async Task InitializeAsync()
        {
            await _db.CreateTableAsync<Restaurant>();
            await SeedRestaurantsAsync();
        }

        private async Task SeedRestaurantsAsync()
        {
            if (await _db.Table<Restaurant>().CountAsync() > 0)
                return;

            var defaults = new List<Restaurant>
            {
                new Restaurant { Name = "Wendy's", Slug = "wendys" },
                new Restaurant { Name = "Dunkin'", Slug = "dunkin" },
                new Restaurant { Name = "Wingstop", Slug = "wingstop" },
                new Restaurant { Name = "Culver's",  Slug = "culvers" }
            };

            await _db.InsertAllAsync(defaults);
        }

        /// <summary>
        /// Pull all restaurants from SQLite in alphabetical order.
        /// Used by RestaurantsViewModel.
        /// </summary>
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
    }
}
}
