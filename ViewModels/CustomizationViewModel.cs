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

        // I extract navigation parameters (MenuItemId)
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            MenuItemId = (int)query["MenuItemId"];
        }

        // MAIN LOAD METHOD — loads base item and attaches appropriate options
        public async Task LoadAsync()
        {
            // Load base menu item
            var item = await _repo.GetMenuItemAsync(MenuItemId);

            ItemName = item.Name;

            BaseCalories = item.Calories;
            BaseProtein = (decimal)item.Protein;
            BaseCarbs = (decimal)item.Carbs;
            BaseFat = (decimal)item.Fat;
            BaseSodium = item.Sodium;

           
            var allOptions = await _repo.GetCustomOptionsForMenuItemAsync(MenuItemId);

            IEnumerable<MenuItemOption> filteredOptions = (IEnumerable<MenuItemOption>)allOptions;

            // Normalize category text so spelling variants don't break logic
            string category = item.Category?.Trim().ToLower() ?? "";

            if (category == "sandwich")
            {
                //  ONLY include "Topping" for sandwiches
                filteredOptions = (IEnumerable<MenuItemOption>)allOptions.Where(o => o.Category == "Topping");
            }
            else if (category == "burger")
            {
                // ONLY include "BurgerTopping" for burgers
                filteredOptions = (IEnumerable<MenuItemOption>)allOptions.Where(o => o.Category == "BurgerTopping");
            }
            // Wings, Donuts, Beverages, etc. → no filtering required

            // -----------------------------
            // GROUP OPTIONS FOR UI
            // -----------------------------
            var grouped = filteredOptions
                .GroupBy(o => o.Category)
                .Select(g =>
                {
                    var optionList = new ObservableCollection<SelectableOption>();

                    foreach (var optModel in g)
                    {
                        // I create a selectable wrapper (holds IsSelected & Model)
                        var opt = new SelectableOption(optModel);

                        // I wire up live macro recalculation when a checkbox toggles
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

            // Clear and repopulate groups
            OptionGroups.Clear();
            foreach (var group in grouped)
                OptionGroups.Add(group);

            // Notify UI that base nutrition + totals are available
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
                    await _repo.InsertLoggedItemOptionAsync(loggedItem.Id, opt.Id);
            }

            await Shell.Current.DisplayAlert("Success", "Item added to your log.", "OK");
            await Shell.Current.GoToAsync("..");
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
