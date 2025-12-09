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
            // Local buckets — O(1) append operations
            var mains = new List<MenuItem>();
            var sides = new List<MenuItem>();
            var beverages = new List<MenuItem>();
            var sauces = new List<MenuItem>();
            var bakery = new List<MenuItem>();
            var desserts = new List<MenuItem>();

            foreach (var item in items)
            {
                var cat = item.Category?.ToLowerInvariant() ?? "";

                // MAIN ENTREES
                if (cat.Contains("wing") ||
                    cat.Contains("burger") ||
                    cat.Contains("sandwich") ||
                    cat.Contains("salad") ||
                    cat.Contains("breakfast"))
                {
                    mains.Add(item);
                    continue;
                }

                // SIDES
                if (cat.Contains("side") ||
                    cat.Contains("fries") ||
                    cat.Contains("corn"))
                {
                    sides.Add(item);
                    continue;
                }

                // BEVERAGES
                if (cat.Contains("beverage") ||
                    cat.Contains("drink") ||
                    cat.Contains("coffee") ||
                    cat.Contains("tea"))
                {
                    beverages.Add(item);
                    continue;
                }

                // SAUCES / DIPS
                if (cat.Contains("sauce") ||
                    cat.Contains("dip"))
                {
                    sauces.Add(item);
                    continue;
                }

                // BAKERY (DONUTS)
                if (cat.Contains("donut") ||
                    cat.Contains("bakery") ||
                    cat.Contains("pastry"))
                {
                    bakery.Add(item);
                    continue;
                }

                // DESSERTS
                if (cat.Contains("dessert"))
                {
                    desserts.Add(item);
                    continue;
                }
            }

            // Build groups ONLY for non-empty buckets
            var groups = new ObservableCollection<MenuItemGroup>();

            if (mains.Any()) groups.Add(new MenuItemGroup("Mains", mains));
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

