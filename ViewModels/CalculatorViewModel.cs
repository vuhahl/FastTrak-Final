using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FastTrak.ViewModels
{
    public partial class CalculatorViewModel : ObservableObject
    {
        private readonly NutritionRepository _repo;

        public ObservableCollection<LoggedItem> TodayLoggedItems { get; } =
    new ObservableCollection<LoggedItem>();


        [ObservableProperty] private int totalCalories;
        [ObservableProperty] private decimal totalProtein;
        [ObservableProperty] private decimal totalCarbs;
        [ObservableProperty] private decimal totalFat;
        [ObservableProperty] private int totalSodium;

        public CalculatorViewModel(NutritionRepository repo)
        {
            _repo = repo;
        }

        public async Task LoadAsync()
        {
            TodayLoggedItems.Clear();

            var items = await _repo.GetLoggedItemsForTodayAsync();
            foreach (var item in items)
                TodayLoggedItems.Add(item);

            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            TotalCalories = TodayLoggedItems.Sum(i => i.CaloriesOverride * i.Quantity);
            TotalProtein = TodayLoggedItems.Sum(i => i.ProteinOverride * i.Quantity);
            TotalCarbs = TodayLoggedItems.Sum(i => i.CarbsOverride * i.Quantity);
            TotalFat = TodayLoggedItems.Sum(i => i.FatOverride * i.Quantity);
            TotalSodium = TodayLoggedItems.Sum(i => i.SodiumOverride * i.Quantity);
        }

        // Increase quantity of an item in the calculator
        [RelayCommand]
        private async Task IncreaseItemQuantityAsync(LoggedItem item)
        {
            if (item == null) return;

            item.Quantity++;
            await _repo.UpdateLoggedItemAsync(item);

            // Recalculate totals after change
            RecalculateTotals();
        }

        // Decrease quantity of an item in the calculator
        [RelayCommand]
        private async Task DecreaseItemQuantityAsync(LoggedItem item)
        {
            if (item == null) return;
            if (item.Quantity <= 1) return; // don't go below 1

            item.Quantity--;
            await _repo.UpdateLoggedItemAsync(item);

            RecalculateTotals();
        }

        // Remove a single item from the calculator
        [RelayCommand]
        private async Task RemoveItemAsync(LoggedItem item)
        {
            if (item == null) return;

            // Optional confirm dialog (can remove if you want silent delete)
            bool confirm = await Shell.Current.DisplayAlert(
                "Remove Item",
                $"Remove {item.DisplayName} from today's log?",
                "Remove",
                "Cancel");

            if (!confirm) return;

            await _repo.DeleteLoggedItemAsync(item.Id);
            TodayLoggedItems.Remove(item);

            RecalculateTotals();
        }


        [RelayCommand]
        private async Task ClearAllAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Clear Today's Log?",
                "This will remove all logged items for today.",
                "Clear", "Cancel");

            if (!confirm) return;

            await _repo.ClearLoggedItemsForTodayAsync();
            TodayLoggedItems.Clear();
            RecalculateTotals();
        }
    }
}
