using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Views;
using FastTrak.Models;
using FastTrak.Services;
using System.Collections.ObjectModel;

/// <summary>
/// Loads today's log items + count and handles navigation from Home screen.
/// </summary>
public partial class HomeViewModel : ObservableObject
{
    private readonly NutritionRepository _repository;

    public ObservableCollection<LoggedItem> TodayLoggedItems { get; } = new();

    [ObservableProperty] private int todayItemCount;

    [ObservableProperty] private int totalCalories;
    [ObservableProperty] private decimal totalProtein;
    [ObservableProperty] private decimal totalCarbs;
    [ObservableProperty] private decimal totalFat;

    public HomeViewModel(NutritionRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadAsync()
    {
        TodayLoggedItems.Clear();

        var items = await _repository.GetLoggedItemsForTodayAsync();

        foreach (var item in items)
            TodayLoggedItems.Add(item);

        TodayItemCount = TodayLoggedItems.Count;
        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        totalCalories = TodayLoggedItems.Sum(i => i.CaloriesOverride);
        totalProtein = TodayLoggedItems.Sum(i => i.ProteinOverride);
        totalCarbs = TodayLoggedItems.Sum(i => i.CarbsOverride);
        totalFat = TodayLoggedItems.Sum(i => i.FatOverride);
    }

    // ===== Navigation Commands =====

    [RelayCommand]
    private async Task GoToRestaurantsAsync()
    {
        await Shell.Current.GoToAsync(nameof(RestaurantsPage));
    }

    [RelayCommand]
    private async Task GoToFatSecretSearchAsync()
    {
        await Shell.Current.GoToAsync(nameof(FatSecretSearchPage));
    }

    // ===== Tray Item Commands =====

    // Editing: adjust quantity and recalc macros by treating stored macros as "total" for current quantity
    [RelayCommand]
    private async Task IncreaseItemQuantityAsync(LoggedItem item)
    {
        if (item == null) return;

        if (item.Quantity <= 0) item.Quantity = 1;

        var perCal = (decimal)item.CaloriesOverride / item.Quantity;
        var perPro = item.ProteinOverride / item.Quantity;
        var perCarb = item.CarbsOverride / item.Quantity;
        var perFat = item.FatOverride / item.Quantity;

        item.Quantity++;

        item.CaloriesOverride = (int)(perCal * item.Quantity);
        item.ProteinOverride = perPro * item.Quantity;
        item.CarbsOverride = perCarb * item.Quantity;
        item.FatOverride = perFat * item.Quantity;

        await _repository.UpdateLoggedItemAsync(item);
        RecalculateTotals();
    }

    [RelayCommand]
    private async Task DecreaseItemQuantityAsync(LoggedItem item)
    {
        if (item == null) return;
        if (item.Quantity <= 1) return; // don't go below 1

        var perCal = (decimal)item.CaloriesOverride / item.Quantity;
        var perPro = item.ProteinOverride / item.Quantity;
        var perCarb = item.CarbsOverride / item.Quantity;
        var perFat = item.FatOverride / item.Quantity;

        item.Quantity--;

        item.CaloriesOverride = (int)(perCal * item.Quantity);
        item.ProteinOverride = perPro * item.Quantity;
        item.CarbsOverride = perCarb * item.Quantity;
        item.FatOverride = perFat * item.Quantity;

        await _repository.UpdateLoggedItemAsync(item);
        RecalculateTotals();
    }

    [RelayCommand]
    private async Task DeleteLoggedItemAsync(LoggedItem item)
    {
        if (item == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Remove Item",
            $"Remove {item.DisplayName} from today's log?",
            "Remove",
            "Cancel");

        if (!confirm) return;

        await _repository.DeleteLoggedItemAsync(item.Id);
        TodayLoggedItems.Remove(item);

        TodayItemCount = TodayLoggedItems.Count;
        RecalculateTotals();
    }

    [RelayCommand]
    private async Task ClearAllItemsAsync()
    {
        if (!TodayLoggedItems.Any()) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Clear All Items",
            "This will remove all items in today's log.",
            "Clear",
            "Cancel");

        if (!confirm) return;

        await _repository.ClearLoggedItemsForTodayAsync();
        TodayLoggedItems.Clear();

        TodayItemCount = 0;
        RecalculateTotals();
    }
}
