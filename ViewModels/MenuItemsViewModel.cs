using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Views;
using FastTrak.Models;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.ViewModels
{
    public partial class MenuItemsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly NutritionRepository _repository;

        [ObservableProperty]
        private string restaurantName = string.Empty;

        [ObservableProperty]
        private int restaurantId;

        public ObservableCollection<MenuItem> MenuItems { get; } = new();

        public MenuItemsViewModel(NutritionRepository repository)
        {
            _repository = repository;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RestaurantId = (int)query["RestaurantId"];
            RestaurantName = (string)query["RestaurantName"];
        }
        public class MenuItemGroup : ObservableCollection<MenuItem>
        {
            public string Title { get; }

            public MenuItemGroup(string title, IEnumerable<MenuItem> items) : base(items)
            {
                Title = title;
            }
        }

        [ObservableProperty]
        private ObservableCollection<MenuItemGroup> groupedItems;

        // Good: Asynchronous data loading
        public async Task LoadAsync()
        {
            MenuItems.Clear();

            var items = await _repository.GetMenuItemsForRestaurantAsync(RestaurantId);

            foreach (var m in items)
                MenuItems.Add(m);

            // Process grouping OFF the UI thread
            var groups = await Task.Run(() => CreateGroups(items));

            // Back to UI thread
            GroupedItems = groups;
        }


        private ObservableCollection<MenuItemGroup> CreateGroups(IEnumerable<MenuItem> items)
{
            // Group buckets — each category from seed data properly mapped
            var mains = new List<MenuItem>();
            var sides = new List<MenuItem>();
            var beverages = new List<MenuItem>();
            var sauces = new List<MenuItem>();
            var bakery = new List<MenuItem>();
            var desserts = new List<MenuItem>();
            var breakfast = new List<MenuItem>();   // FIX: Add explicit bucket for Dunkin breakfast

            foreach (var item in items)
            {
                // Normalize category
                var cat = item.Category?.ToLowerInvariant() ?? "";

                // ==========================
                // MAINS (Burgers, Sandwiches, Salads, Wings)
                // ==========================
                if (cat == "burgers" ||
                    cat == "sandwiches" ||
                    cat == "salads" ||
                    cat == "wings")
                {
                    mains.Add(item);
                    continue;
                }

                // ==========================
                // BREAKFAST (Fix for Dunkin)
                // ==========================
                // Previously grouped under "Mains" → but no mains exist for Dunkin → lost entirely
                if (cat == "breakfast")
                {
                    breakfast.Add(item);
                    continue;
                }

                // ==========================
                // SIDES (including potato category)
                // ==========================
                if (cat == "sides" ||
                    cat == "potato")
                {
                    sides.Add(item);
                    continue;
                }

                // ==========================
                // BEVERAGES (Coffee / Drinks)
                // ==========================
                if (cat == "coffee" ||
                    cat == "drink" ||
                    cat == "beverage")
                {
                    beverages.Add(item);
                    continue;
                }

                // ==========================
                // SAUCES (Wingstop dips)
                // ==========================
                if (cat == "sauces" ||
                    cat == "sauce")
                {
                    sauces.Add(item);
                    continue;
                }

                // ==========================
                // DONUTS / BAKERY
                // ==========================
                if (cat == "donuts")
                {
                    bakery.Add(item);
                    continue;
                }

                // ==========================
                // DESSERTS
                // ==========================
                if (cat == "desserts")
                {
                    desserts.Add(item);
                    continue;
                }

                // ==========================
                // FALLBACK — no items disappear
                // ==========================
                mains.Add(item); // Safe default
            }

            // Build groups — only non-empty buckets
            var groups = new ObservableCollection<MenuItemGroup>();

            if (mains.Any()) groups.Add(new MenuItemGroup("Mains", mains));
            if (breakfast.Any()) groups.Add(new MenuItemGroup("Breakfast", breakfast)); // FIXED FOR DUNKIN
            if (sides.Any()) groups.Add(new MenuItemGroup("Sides", sides));
            if (beverages.Any()) groups.Add(new MenuItemGroup("Beverages", beverages));
            if (sauces.Any()) groups.Add(new MenuItemGroup("Sauces", sauces));
            if (bakery.Any()) groups.Add(new MenuItemGroup("Bakery", bakery));
            if (desserts.Any()) groups.Add(new MenuItemGroup("Desserts", desserts));

            return groups;
        }


        // Used for categories that REQUIRE customization (Burgers, Sandwiches, Beverages, Salads, Wings, etc.)
        [RelayCommand]
        private async Task SelectMenuItemAsync(MenuItem item)
        {
            if (item == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "MenuItemId", item.Id }
            };

            await Shell.Current.GoToAsync(nameof(CustomizationPage), parameters);
        }

        // Used for simple items: Sauces, Sides, Donuts (direct add, no customization page)
        [RelayCommand]
        private async Task AddToLogDirectAsync(MenuItem item)
        {
            if (item == null)
                return;

            var loggedItem = new LoggedItem
            {
                MenuItemId = item.Id,
                LoggedAt = DateTime.Now,
                Quantity = 1,
                NameOverride = item.Name,
                CaloriesOverride = item.Calories,
                ProteinOverride = (decimal)item.Protein,
                CarbsOverride = (decimal)item.Carbs,
                FatOverride = (decimal)item.Fat,
                SodiumOverride = item.Sodium
            };

            await _repository.InsertLoggedItemAsync(loggedItem);

            await Shell.Current.DisplayAlert(
                "Added",
                $"{item.Name} added to today's log.",
                "OK");
        }

        
    }
}

