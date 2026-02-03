using System.Net.Http.Json;
using System.Text.Json;
using FastTrak.Models;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.Services;

/// <summary>
/// Fetches restaurant data from the FastTrak REST API.
/// Replaces local SQLite queries with HTTP calls.
/// </summary>
public class RestaurantApiService : IRestaurantDataService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public RestaurantApiService(HttpClient http)
    {
        _http = http;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<Restaurant>> GetRestaurantsAsync()
    {
        var response = await _http.GetFromJsonAsync<ApiResponse<List<Restaurant>>>(
            "/api/v1/restaurants", _jsonOptions);

        return response?.Data ?? new List<Restaurant>();
    }

    public async Task<List<MenuItem>> GetMenuItemsForRestaurantAsync(int restaurantId)
    {
        // Request all items (pageSize=100 should cover most menus)
        var response = await _http.GetFromJsonAsync<ApiResponse<List<MenuItem>>>(
            $"/api/v1/restaurants/{restaurantId}/menu-items?pageSize=100", _jsonOptions);

        return response?.Data ?? new List<MenuItem>();
    }

    public async Task<MenuItem> GetMenuItemAsync(int id)
    {
        var response = await _http.GetFromJsonAsync<ApiResponse<MenuItem>>(
            $"/api/v1/menu-items/{id}", _jsonOptions);

        return response?.Data ?? throw new InvalidOperationException($"Menu item {id} not found");
    }

    public async Task<List<CustomOption>> GetCustomOptionsForMenuItemAsync(int menuItemId)
    {
        var response = await _http.GetFromJsonAsync<ApiResponse<List<CustomOption>>>(
            $"/api/v1/menu-items/{menuItemId}/options", _jsonOptions);

        return response?.Data ?? new List<CustomOption>();
    }

    /// <summary>
    /// Wrapper for API responses. The API returns { data: T, meta: {...} }
    /// </summary>
    private class ApiResponse<T>
    {
        public T? Data { get; set; }
    }
}
