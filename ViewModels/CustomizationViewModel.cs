using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace FastTrak.ViewModels
{
    public partial class CustomizationViewModel : ObservableObject, IQueryAttributable //IQUERY - to receive parameters during navigation (knowing what to load)

    /// <summary>
    /// Displays base nutrition, Handles custom options
    /// Calculates totals live, Saves customized items 
    /// 
    /// </summary>
    {
        private readonly NutritionRepository _repo;

        [ObservableProperty]
        private int menuItemId;

        [ObservableProperty] //OP  is an attribute of MVVM (public property, private variable)
        private string itemName = string.Empty;

        public int BaseCalories { get; set; } //nutrition values before customizations
        public decimal BaseProtein { get; set; }
        public decimal BaseCarbs { get; set; }
        public decimal BaseFat { get; set; }
        public int BaseSodium { get; set; }

        public string BaseNutritionText =>
            $"{BaseCalories} cal | {BaseProtein}g protein | {BaseCarbs}g carbs | {BaseFat}g fat | {BaseSodium}mg sodium";

        [ObservableProperty]
        private int quantity = 1;

        public ObservableCollection<OptionGroup> OptionGroups { get; set; } = new(); //groups of options for the UI

        public CustomizationViewModel(NutritionRepository repo)
        {
            _repo = repo;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query) //Iquery
        {
            MenuItemId = (int)query["MenuItemId"]; //get MenuItemId from navigation parameters
        }

        public async Task LoadAsync()
        {
            var item = await _repo.GetMenuItemAsync(MenuItemId);

            // Base nutrition values
            BaseCalories = item.Calories;
            BaseProtein = (decimal)item.Protein;
            BaseCarbs = (decimal)item.Carbs;
            BaseFat = (decimal)item.Fat;
            BaseSodium = item.Sodium;

            ItemName = item.Name;
            OnPropertyChanged(nameof(ItemName));

            //
            // FIX: Notify UI of base nutrition values
            //
            OnPropertyChanged(nameof(BaseCalories));
            OnPropertyChanged(nameof(BaseProtein));
            OnPropertyChanged(nameof(BaseCarbs));
            OnPropertyChanged(nameof(BaseFat));
            OnPropertyChanged(nameof(BaseSodium));
            OnPropertyChanged(nameof(BaseNutritionText));

            var allOptions = await _repo.GetCustomOptionsForMenuItemAsync(MenuItemId);

            string category = (item.Category ?? "").Trim().ToLowerInvariant();

            IEnumerable<CustomOption> filtered = allOptions;

            // CATEGORY FILTERING...
            switch (category)
            {
                case "sandwiches":
                    filtered = allOptions.Where(o =>
                        o.Category == "Topping" || o.Category == "Sauce");
                    break;

                case "burgers":
                    filtered = allOptions.Where(o =>
                        o.Category == "BurgerTopping");
                    break;

                case "potato":
                    filtered = allOptions.Where(o =>
                        o.Category == "Potato");
                    break;

                case "coffee":
                    filtered = allOptions.Where(o =>
                        o.Category == "MilkType" ||
                        o.Category == "Shot" ||
                        o.Category == "Swirl" ||
                        o.Category == "Sweetener");
                    break;

                case "wings":
                case "desserts":
                case "donuts":
                case "breakfast":
                    filtered = Enumerable.Empty<CustomOption>();
                    break;
            }

            // GROUP OPTIONS FOR UI
            var grouped = filtered
                .GroupBy(o => o.Category)
                .Select(g =>
                {
                    var opts = new ObservableCollection<SelectableOption>();

                    foreach (var optModel in g)
                    {
                        var opt = new SelectableOption(optModel);

                        opt.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(SelectableOption.IsSelected))
                                NotifyTotals();
                        };

                        opts.Add(opt);
                    }

                    return new OptionGroup
                    {
                        Category = g.Key,
                        Options = opts
                    };
                });

            OptionGroups.Clear();
            foreach (var group in grouped)
                OptionGroups.Add(group);

            // Also compute totals once UI is ready
            NotifyTotals();
        }

        // QUANTITY COMMANDS
        [RelayCommand]
        private void IncreaseQuantity()
        {
            Quantity++;
            NotifyTotals();
        }

        [RelayCommand]
        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                NotifyTotals();
            }
        }

        // ADD ITEM + CUSTOMIZATIONS
        [RelayCommand]
        private async Task AddToLog()
        {
            //
            // 1. Compute PER-UNIT nutrition (base item + selected option

            int perUnitCalories =
                BaseCalories +
                OptionGroups.SelectMany(g => g.Options) //o = SelectableOption
                            .Where(o => o.IsSelected)
                            .Sum(o => o.Calories);

            decimal perUnitProtein =
                BaseProtein +
                OptionGroups.SelectMany(g => g.Options)
                            .Where(o => o.IsSelected)
                            .Sum(o => o.Protein);

            decimal perUnitCarbs =
                BaseCarbs +
                OptionGroups.SelectMany(g => g.Options)
                            .Where(o => o.IsSelected)
                            .Sum(o => o.Carbs);

            decimal perUnitFat =
                BaseFat +
                OptionGroups.SelectMany(g => g.Options)
                            .Where(o => o.IsSelected)
                            .Sum(o => o.Fat);

            int perUnitSodium =
                BaseSodium +
                OptionGroups.SelectMany(g => g.Options)
                            .Where(o => o.IsSelected)
                            .Sum(o => o.Sodium);

            //
            // 2. Create & insert the main LoggedItem with PER-UNIT macros
            //

            var menuItem = await _repo.GetMenuItemAsync(MenuItemId);

            var loggedItem = new LoggedItem
            {
                MenuItemId = MenuItemId,
                RestaurantId = menuItem.RestaurantId,
                LoggedAt = DateTime.Now,
                Quantity = Quantity,

                NameOverride = ItemName,

                CaloriesOverride = perUnitCalories,
                ProteinOverride = perUnitProtein,
                CarbsOverride = perUnitCarbs,
                FatOverride = perUnitFat,
                SodiumOverride = perUnitSodium
            };

            await _repo.InsertLoggedItemAsync(loggedItem);

            //
            // 3. Save ALL selected customization options for historical accuracy
            //

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

            //
            // 4. Confirmation popup for user
            //
            await Shell.Current.DisplayAlert(
                "Added to Tray",
                $"{ItemName} (Qty {Quantity}) has been added to your calculator.",
                "OK");

            //
            // 5. Navigate back to previous page
            //
            await Shell.Current.GoToAsync("..");
        }


        private void NotifyTotals()
        {
            OnPropertyChanged(nameof(TotalNutritionText));
            OnPropertyChanged(nameof(TotalCalories));
            OnPropertyChanged(nameof(TotalProtein));
            OnPropertyChanged(nameof(TotalCarbs));
            OnPropertyChanged(nameof(TotalFat));
            OnPropertyChanged(nameof(TotalSodium));
        }

        // NUTRITION CALCULATION
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
