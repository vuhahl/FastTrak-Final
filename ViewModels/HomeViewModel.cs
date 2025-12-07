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

    /// <summary>
    /// Number of items logged today.
    /// </summary>
    [ObservableProperty]
    private int todayItemCount;

    /// <summary>
    /// The actual logged items for today.
    /// </summary>
    public ObservableCollection<LoggedItem> TodayLoggedItems { get; } =
        new ObservableCollection<LoggedItem>();

    public HomeViewModel(NutritionRepository repo)
    {
        _repository = repo;
    }

    /// <summary>
    /// Reloads all log data every time the Home page appears.
    /// </summary>
    public async Task LoadAsync()
    {
        TodayLoggedItems.Clear();

        var items = await _repository.GetLoggedItemsForTodayAsync();

        foreach (var item in items)
            TodayLoggedItems.Add(item);
        TodayItemCount = TodayLoggedItems.Count;
    }

    /// <summary>
    /// Navigates to Restaurants list.
    /// </summary>
    [RelayCommand]
    private Task GoToRestaurantsAsync()
    {
        return Shell.Current.GoToAsync("//RestaurantsPage");
    }

    /// </summary>
    [RelayCommand]
    private Task GoToFatSecretSearchAsync()
    {
        return Shell.Current.GoToAsync(nameof(FatSecretSearchPage));
    }
}