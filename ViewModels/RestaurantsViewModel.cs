using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Models;
using FastTrak.Services;
using FastTrak.Views;
using System.Collections.ObjectModel;

namespace FastTrak.ViewModels
{
    /// <summary>
    /// Displays list of restaurants for user selection.
    ///
    /// DEPENDENCY: IRestaurantDataService (reference data)
    /// - Today: SQLite via NutritionRepository
    /// - Future: REST API via RestaurantApiService
    /// </summary>
    public partial class RestaurantsViewModel : ObservableObject
    {
        private readonly IRestaurantDataService _dataService;

        public ObservableCollection<Restaurant> Restaurants { get; } = new();

        public RestaurantsViewModel(IRestaurantDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadAsync()
        {
            Restaurants.Clear();
            var items = await _dataService.GetRestaurantsAsync();
            foreach (var r in items)
                Restaurants.Add(r);
        }

        [RelayCommand]
        private async Task SelectRestaurantAsync(Restaurant restaurant)
        {
            if (restaurant == null)
                return;

            var parameters = new Dictionary<string, object>
        {
            { "RestaurantId", restaurant.Id },
            { "RestaurantName", restaurant.Name }
        };

            await Shell.Current.GoToAsync(nameof(MenuItemsPage), parameters);
        }
    }
}

