using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using FastTrak.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace FastTrak.ViewModels
{
    /// <summary>
    /// Searches external FatSecret API for foods and logs them locally.
    ///
    /// DEPENDENCIES:
    /// - FatSecretService: External API for food search (cloud)
    /// - IUserLogRepository: Saves logged items (user data → SQLite)
    /// </summary>
    public partial class FatSecretSearchViewModel : ObservableObject
    {
        private readonly FatSecretService _service;
        private readonly IUserLogRepository _userLog;

        [ObservableProperty]
        private string query = string.Empty;

        [ObservableProperty]
        private bool isSearching;

        [ObservableProperty]
        private bool hasSearched;

        public ObservableCollection<FatSecretFoodSearchItem> Results { get; } =
            new ObservableCollection<FatSecretFoodSearchItem>();

        public FatSecretSearchViewModel(FatSecretService service, IUserLogRepository userLog)
        {
            _service = service;
            _userLog = userLog;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            if (IsSearching) return;
            

            Results.Clear();
            HasSearched = false;

            if (string.IsNullOrWhiteSpace(Query))
            {
                await Shell.Current.DisplayAlert("Search Required", "Please enter a food name to search.", "OK");
                return;
            }

            try
            {
                IsSearching = true;
                Debug.WriteLine($"[FatSecret] Searching for: {Query}");

                var items = await _service.SearchFoodsAsync(Query.Trim(), maxResults: 20);

                Debug.WriteLine($"[FatSecret] Found {items.Count} results");

                foreach (var item in items)
                    Results.Add(item);

                HasSearched = true;

                if (Results.Count == 0)
                {
                    await Shell.Current.DisplayAlert("No Results",
                        $"No foods found for '{Query}'. Try a different search term.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FatSecret] Search Error: {ex}");
                await Shell.Current.DisplayAlert("Error",
                    $"Unable to search foods.\n\nError: {ex.Message}", "OK");
            }
            finally
            {
                IsSearching = false;
            }
        }

        [RelayCommand]
        private async Task AddToLogAsync(FatSecretFoodSearchItem item)
        {
            if (item == null) return;

            try
            {
                Debug.WriteLine($"[FatSecret] Getting details for food_id: {item.food_id}");

                var details = await _service.GetFoodDetailsAsync(item.food_id);
                var s = GetPrimaryServing(details);

                if (s == null)
                {
                    await Shell.Current.DisplayAlert("Unavailable",
                        "Nutrition details are unavailable for this item (no serving data returned).", "OK");
                    return;
                }

                Debug.WriteLine($"[FatSecret] Serving: {s.serving_description}, Calories: {s.calories}");

                var calories = TryParseIntInvariant(s.calories, out var cal) ? cal : 0;
                var protein = TryParseDoubleInvariant(s.protein, out var prot) ? prot : 0;
                var carbs = TryParseDoubleInvariant(s.carbohydrate, out var carb) ? carb : 0;
                var fat = TryParseDoubleInvariant(s.fat, out var f) ? f : 0;
                var sodium = TryParseDoubleInvariant(s.sodium, out var sod) ? sod : 0;

                var logged = new LoggedItem
                {
                    MenuItemId = 0,
                    LoggedAt = DateTime.Now,
                    Quantity = 1,
                    NameOverride = item.food_name,
                    CaloriesOverride = calories,
                    ProteinOverride = (decimal)protein,
                    CarbsOverride = (decimal)carbs,
                    FatOverride = (decimal)fat,
                    SodiumOverride = (int)Math.Round(sodium)
                };

                await _userLog.InsertLoggedItemAsync(logged);

                Debug.WriteLine($"[FatSecret] Added to log: {logged.DisplayName}");

                await Shell.Current.DisplayAlert("Added to Tray",
                    $"{item.food_name} has been added to your daily log.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FatSecret] Add to Log Error: {ex}");
                await Shell.Current.DisplayAlert("Error",
                    $"Unable to add {item.food_name} to log.\n\nError: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private void ClearSearch()
        {
            Query = string.Empty;
            Results.Clear();
            HasSearched = false;
        }

        private static bool TryParseIntInvariant(string s, out int value)
        {
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryParseDoubleInvariant(string s, out double value)
        {
            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private static FatSecretServing GetPrimaryServing(FatSecretFoodDetails details)
        {
            if (details?.food?.servings == null) return null;

            var el = details.food.servings.serving;

            try
            {
                if (el.ValueKind == JsonValueKind.Object)
                {
                    return JsonSerializer.Deserialize<FatSecretServing>(el.GetRawText());
                }

                if (el.ValueKind == JsonValueKind.Array && el.GetArrayLength() > 0)
                {
                    var first = el[0];
                    return JsonSerializer.Deserialize<FatSecretServing>(first.GetRawText());
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FatSecret] Serving normalize error: {ex}");
                return null;
            }
        }
    }
}
