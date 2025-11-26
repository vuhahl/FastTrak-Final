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

        [ObservableProperty]
        private Restaurant? selectedRestaurant;

        public RestaurantsViewModel(NutritionRepository repo)
        {
            _repository = repo;
        }

        public async Task LoadAsync()
        {
            Restaurants.Clear();
            var items = await _repository.GetRestaurantsAsync();

            foreach (var restaurant in items)
                Restaurants.Add(restaurant);
        }

        [RelayCommand]
        private async Task SelectRestaurantAsync()
        {
            if (SelectedRestaurant == null)
            {
                await Shell.Current.DisplayAlert(
                    "No restaurant selected",
                    "Please tap a restaurant first.",
                    "OK");
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                { "RestaurantId", SelectedRestaurant.Id },
                { "RestaurantName", SelectedRestaurant.Name }
            };

            await Shell.Current.GoToAsync(nameof(MenuItemsPage), parameters);
        }
    }
}

