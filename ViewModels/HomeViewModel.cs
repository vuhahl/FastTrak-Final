using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using FastTrak.Services;
using FastTrak.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

/// <summary>
/// Loads today's log items + count and handles navigation from Home screen.
/// </summary>
public partial class HomeViewModel : ObservableObject
{
    private readonly NutritionRepository _repository;

    /// <summary>
    /// Items logged for today.
    /// </summary>
    public ObservableCollection<LoggedItem> TodayLoggedItems { get; } = new();

    // "Today’s Log" count
    [ObservableProperty] private int todayItemCount;

    // Motivational quote from Quotable.io
    [ObservableProperty]
    private string motivationalQuote;

    private readonly string[] zenQuoteEndpoints = new[]
{
    "https://zenquotes.io/api/today",
    "https://zenquotes.io/api/random"
};

    // Totals shown in the summary card
    [ObservableProperty] private int totalCalories;
    [ObservableProperty] private decimal totalProtein;
    [ObservableProperty] private decimal totalCarbs;
    [ObservableProperty] private decimal totalFat;

    public HomeViewModel(NutritionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Reloads today's log from the database.
    /// Called from HomePage.OnAppearing().
    /// </summary>
    public async Task LoadAsync()
    {
        TodayLoggedItems.Clear();

        var items = await _repository.GetLoggedItemsForTodayAsync();
        foreach (var item in items)
            TodayLoggedItems.Add(item);

        TodayItemCount = TodayLoggedItems.Count;
        RecalculateTotals();

        await Task.Delay(4000);
        // Load quote after totals
        await LoadQuoteAsync();
    }


    private readonly HttpClient _httpClient = new HttpClient();

    private async Task LoadQuoteAsync()
    {
        Debug.WriteLine("[Quotes] Fetching ZenQuote...");

        try
        {
            var random = new Random();
            var url = zenQuoteEndpoints[random.Next(zenQuoteEndpoints.Length)];

            Debug.WriteLine($"[Quotes] Selected endpoint: {url}");

            // GET JSON
            var json = await _httpClient.GetStringAsync(url);
            Debug.WriteLine($"[Quotes] Raw ZenQuotes JSON: {json}");

            // Parse JSON array
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
                throw new Exception("Unexpected ZenQuotes JSON format");

            var obj = root[0];

            string quote = obj.GetProperty("q").GetString() ?? "";
            string author = obj.GetProperty("a").GetString() ?? "Unknown";

            MotivationalQuote = $"\"{quote}\" — {author}";

            Debug.WriteLine("[Quotes] Successfully loaded ZenQuote.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Quotes] ERROR loading ZenQuote: {ex.Message}");

            MotivationalQuote =
                "\"Stay disciplined. Small steps forward lead to big results.\" — FastTrak";
        }
    }

    /// <summary>
    /// Totals are computed as per-unit macros * quantity.
    /// CaloriesOverride / ProteinOverride / etc are stored per 1 unit.
    /// </summary>
    private void RecalculateTotals()
    {
        TotalCalories = TodayLoggedItems.Sum(i => i.CaloriesOverride * i.Quantity);
        TotalProtein = TodayLoggedItems.Sum(i => i.ProteinOverride * i.Quantity);
        TotalCarbs = TodayLoggedItems.Sum(i => i.CarbsOverride * i.Quantity);
        TotalFat = TodayLoggedItems.Sum(i => i.FatOverride * i.Quantity);
    }

    // ========== Navigation ==========

    [RelayCommand]
    private Task GoToRestaurantsAsync()
    {
        // RestaurantsPage is a ShellContent item
        return Shell.Current.GoToAsync("//RestaurantsPage");
    }

    [RelayCommand]
    private Task GoToFatSecretSearchAsync()
    {
        return Shell.Current.GoToAsync(nameof(FatSecretSearchPage));
    }

    [RelayCommand]
    private async Task GoToCalculatorAsync()
    {
        await Shell.Current.GoToAsync(nameof(CalculatorPage));
    }

    // ========== Tray Editing Commands ==========

    [RelayCommand]
    private async Task IncreaseItemQuantityAsync(LoggedItem item)
    {
        if (item == null) return;

        // simple +1, per-unit macros stay the same
        item.Quantity++;
        await _repository.UpdateLoggedItemAsync(item);

        // count & totals
        TodayItemCount = TodayLoggedItems.Count;
        RecalculateTotals();
    }

    [RelayCommand]
    private async Task DecreaseItemQuantityAsync(LoggedItem item)
    {
        if (item == null) return;
        if (item.Quantity <= 1) return; // do not go below 1

        item.Quantity--;
        await _repository.UpdateLoggedItemAsync(item);

        TodayItemCount = TodayLoggedItems.Count;
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
            "Clear Today's Log",
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
    

