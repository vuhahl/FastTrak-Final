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

        // Passed-in parameters
        [ObservableProperty]
        private int menuItemId;

        [ObservableProperty]
        private string itemName = string.Empty;

        // Base nutrition
        public int BaseCalories { get; set; }
        public decimal BaseProtein { get; set; }
        public decimal BaseCarbs { get; set; }
        public decimal BaseFat { get; set; }
        public int BaseSodium { get; set; }

        public string BaseNutritionText =>
            $"{BaseCalories} cal | {BaseProtein}g protein | " +
            $"{BaseCarbs}g carbs | {BaseFat}g fat | {BaseSodium}mg sodium";

        [ObservableProperty]
        private int quantity = 1;

        public ObservableCollection<OptionGroup> OptionGroups { get; set; } = new();

        public CustomizationViewModel(NutritionRepository repo)
        {
            _repo = repo;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            MenuItemId = (int)query["MenuItemId"];
        }

        public async Task LoadAsync()
        {
            var item = await _repo.GetMenuItemAsync(MenuItemId);

            ItemName = item.Name;

            BaseCalories = item.Calories;
            BaseProtein = (decimal)item.Protein;
            BaseCarbs = (decimal)item.Carbs;
            BaseFat = (decimal)item.Fat;
            BaseSodium = item.Sodium;

            // Load allowed options
            var options = await _repo.GetCustomOptionsForMenuItemAsync(MenuItemId);

            // Group and attach live-update listeners
            var grouped = options
                .GroupBy(o => o.Category)
                .Select(g =>
                {
                    var optionList = new ObservableCollection<SelectableOption>();

                    foreach (var optModel in g)
                    {
                        var opt = new SelectableOption(optModel);

                        //Attach listener for checkbox changes
                        opt.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(SelectableOption.IsSelected))
                            {
                                // Notify UI that TotalNutritionText must refresh
                                OnPropertyChanged(nameof(TotalNutritionText));
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

            OptionGroups.Clear();
            foreach (var group in grouped)
                OptionGroups.Add(group);
        }

        [RelayCommand]
        private void IncreaseQuantity()
        {
            Quantity++;
            OnPropertyChanged(nameof(TotalNutritionText));
        }

        [RelayCommand]
        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                OnPropertyChanged(nameof(TotalNutritionText));
            }
        }

        [RelayCommand]
        private async Task AddToLog()
        {
            var loggedItem = new LoggedItem
            {
                MenuItemId = MenuItemId,
                LoggedAt = DateTime.Now,
                Quantity = Quantity
            };

            await _repo.InsertLoggedItemAsync(loggedItem);

            // Insert selected options
            foreach (var group in OptionGroups)
            {
                foreach (var opt in group.Options.Where(o => o.IsSelected))
                    await _repo.InsertLoggedItemOptionAsync(loggedItem.Id, opt.Id);
            }

            await Shell.Current.DisplayAlert("Success", "Item added to your log.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        //properties for live calculation
        public int TotalCalories =>
    (BaseCalories + OptionGroups.SelectMany(g => g.Options)
                                .Where(o => o.IsSelected)
                                .Sum(o => o.Calories)) * Quantity;

        public double TotalProtein =>
            (double)((BaseProtein + OptionGroups.SelectMany(g => g.Options)
                                       .Where(o => o.IsSelected)
                                       .Sum(o => o.Protein)) * Quantity);

        public double TotalCarbs =>
            (double)((BaseCarbs + OptionGroups.SelectMany(g => g.Options)
                                     .Where(o => o.IsSelected)
                                     .Sum(o => o.Carbs)) * Quantity);

        public double TotalFat =>
            (double)((BaseFat + OptionGroups.SelectMany(g => g.Options)
                                   .Where(o => o.IsSelected)
                                   .Sum(o => o.Fat)) * Quantity);

        public int TotalSodium =>
            (BaseSodium + OptionGroups.SelectMany(g => g.Options)
                                      .Where(o => o.IsSelected)
                                      .Sum(o => o.Sodium)) * Quantity;

        public string TotalNutritionText =>
            $"{TotalCalories} cal | {TotalProtein}g P | {TotalCarbs}g C | {TotalFat}g F | {TotalSodium}mg Na";


    }
}
