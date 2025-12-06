using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Views;
using FastTrak.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.ViewModels
{
    public partial class RestaurantsViewModel : ObservableObject
    {
        private readonly NutritionRepository _repository;

        public ObservableCollection<Restaurant> Restaurants { get; } = new();

        public RestaurantsViewModel(NutritionRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadAsync()
        {
            Restaurants.Clear();
            var items = await _repository.GetRestaurantsAsync();
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

