using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.ViewModels
{
    public partial class CustomizationViewModel : ObservableObject, IQueryAttributable
    {
        private readonly NutritionRepository _repo;

        // store the MenuItem ID passed from the MenuItemsPage
        [ObservableProperty]
        private int menuItemId;

        // This holds the visible name of the item (e.g., “Classic Chicken Sandwich”)
        [ObservableProperty]
        private string itemName = string.Empty;

        // Base menu item nutrition (the values before customizations)
        public int BaseCalories { get; set; }
        public decimal BaseProtein { get; set; }
        public decimal BaseCarbs { get; set; }
        public decimal BaseFat { get; set; }
        public int BaseSodium { get; set; }

        // Shown under “Nutrition (Base)” on the UI
        public string BaseNutritionText =>
            $"{BaseCalories} cal | {BaseProtein}g protein | " +
            $"{BaseCarbs}g carbs | {BaseFat}g fat | {BaseSodium}mg sodium";

        // I track quantity → affects final macros
        [ObservableProperty]
        private int quantity = 1;

        // All groups of options that apply to this item
        // (e.g., Milk Types, Toppings, Sauces, BurgerToppings, etc.)
        public ObservableCollection<OptionGroup> OptionGroups { get; set; } = new();

        public CustomizationViewModel(NutritionRepository repo)
        {
            _repo = repo;
        }

        // extract navigation parameters (MenuItemId)
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            MenuItemId = (int)query["MenuItemId"];
        }

        // MAIN LOAD METHOD — loads base item and attaches appropriate options
        public async Task LoadAsync()
        {
            // load the base menu item first - figure out what category it belongs to.
            var item = await _repo.GetMenuItemAsync(MenuItemId);

            ItemName = item.Name;

            BaseCalories = item.Calories;
            BaseProtein = (decimal)item.Protein;
            BaseCarbs = (decimal)item.Carbs;
            BaseFat = (decimal)item.Fat;
            BaseSodium = item.Sodium;

            // load ALL CustomOptions mapped to this MenuItemId.
            // This returns CustomOption objects, NOT MenuItemOption join rows.
            var allOptions = await _repo.GetCustomOptionsForMenuItemAsync(MenuItemId);

            //  standardize the category for safe comparison.
            string category = (item.Category ?? "").Trim().ToLower();

            IEnumerable<CustomOption> filteredOptions = allOptions;


            // Sandwiches → only "Topping"
            if (category == "sandwich" || category == "sandwiches")
            {
                filteredOptions = allOptions.Where(o =>
                    (o.Category ?? "").Equals("Topping", StringComparison.OrdinalIgnoreCase));
            }
            // Burgers → only "BurgerTopping"
            else if (category == "burger" || category == "burgers")
            {
                filteredOptions = allOptions.Where(o =>
                    (o.Category ?? "").Equals("BurgerTopping", StringComparison.OrdinalIgnoreCase));
            }

            // ============================
            // GROUP OPTIONS FOR UI DISPLAY
            // ============================
            //
            // group CustomOptions by their Category ("Topping", "MilkType", etc.)
            // and convert them into Observable SelectableOption objects.
            //
            // Each SelectableOption has a PropertyChanged listener so macros
            // automatically update when a checkbox is toggled.
            //
            // ============================

            var grouped = filteredOptions
                .GroupBy(o => o.Category)
                .Select(g =>
                {
                    var optionList = new ObservableCollection<SelectableOption>();

                    foreach (var optModel in g)
                    {
                        var opt = new SelectableOption(optModel);

                        // I subscribe to IsSelected changes so totals recalc instantly.
                        opt.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(SelectableOption.IsSelected))
                            {
                                OnPropertyChanged(nameof(TotalNutritionText));
                                OnPropertyChanged(nameof(TotalCalories));
                                OnPropertyChanged(nameof(TotalProtein));
                                OnPropertyChanged(nameof(TotalCarbs));
                                OnPropertyChanged(nameof(TotalFat));
                                OnPropertyChanged(nameof(TotalSodium));
                            }
                        };

                        optionList.Add(opt);
                    }

                    return new OptionGroup
                    {
                        Category = g.Key,
                        Options = optionList
                    };
                });

            // ============================
            // APPLY GROUPS TO VIEW
            // ============================

            OptionGroups.Clear();
            foreach (var group in grouped)
                OptionGroups.Add(group);

            // I signal to the UI that everything is ready to display.
            OnPropertyChanged(nameof(BaseNutritionText));
            OnPropertyChanged(nameof(TotalNutritionText));
        }


        // QUANTITY CONTROL BUTTONS
        [RelayCommand]
        private void IncreaseQuantity()
        {
            Quantity++;
            OnPropertyChanged(nameof(TotalNutritionText));
            OnPropertyChanged(nameof(TotalCalories));
            OnPropertyChanged(nameof(TotalProtein));
            OnPropertyChanged(nameof(TotalCarbs));
            OnPropertyChanged(nameof(TotalFat));
            OnPropertyChanged(nameof(TotalSodium));
        }

        [RelayCommand]
        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                OnPropertyChanged(nameof(TotalNutritionText));
                OnPropertyChanged(nameof(TotalCalories));
                OnPropertyChanged(nameof(TotalProtein));
                OnPropertyChanged(nameof(TotalCarbs));
                OnPropertyChanged(nameof(TotalFat));
                OnPropertyChanged(nameof(TotalSodium));
            }
        }


        // ADD TO LOG — freezes final nutrition values for use on Home & Calculator pages
        [RelayCommand]
        private async Task AddToLog()
        {
            //  freeze the *calculated final macros* into LoggedItem override fields
            var loggedItem = new LoggedItem
            {
                MenuItemId = MenuItemId,
                LoggedAt = DateTime.Now,
                Quantity = Quantity,

                NameOverride = ItemName,
                CaloriesOverride = TotalCalories,
                ProteinOverride = (decimal)TotalProtein,
                CarbsOverride = (decimal)TotalCarbs,
                FatOverride = (decimal)TotalFat,
                SodiumOverride = TotalSodium
            };

            await _repo.InsertLoggedItemAsync(loggedItem);

            // Save selected customization options for historical accuracy
            foreach (var group in OptionGroups)
            {
                foreach (var opt in group.Options.Where(o => o.IsSelected))
                {
                    var loggedOption = new LoggedItemOption
                    {
                        LoggedItemId = loggedItem.Id,
                        CustomOptionId = opt.Id,
                        OptionName = opt.Name,
                        Calories = opt.Calories,
                        Protein = opt.Protein,
                        Carbs = opt.Carbs,
                        Fat = opt.Fat,
                        Sodium = opt.Sodium
                    };

                    await _repo.InsertLoggedItemOptionAsync(loggedOption);
                }
            }
        }


        // -----------------------------
        // LIVE CALCULATION PROPERTIES
        // -----------------------------

        public int TotalCalories =>
            (BaseCalories +
             OptionGroups.SelectMany(g => g.Options).Where(o => o.IsSelected).Sum(o => o.Calories))
            * Quantity;

        public double TotalProtein =>
            (double)((BaseProtein +
                      OptionGroups.SelectMany(g => g.Options).Where(o => o.IsSelected).Sum(o => o.Protein))
                      * Quantity);

        public double TotalCarbs =>
            (double)((BaseCarbs +
                      OptionGroups.SelectMany(g => g.Options).Where(o => o.IsSelected).Sum(o => o.Carbs))
                      * Quantity);

        public double TotalFat =>
            (double)((BaseFat +
                      OptionGroups.SelectMany(g => g.Options).Where(o => o.IsSelected).Sum(o => o.Fat))
                      * Quantity);

        public int TotalSodium =>
            (BaseSodium +
             OptionGroups.SelectMany(g => g.Options).Where(o => o.IsSelected).Sum(o => o.Sodium))
            * Quantity;


        public string TotalNutritionText =>
            $"{TotalCalories} cal | {TotalProtein}g P | {TotalCarbs}g C | {TotalFat}g F | {TotalSodium}mg Na";
    }
}
