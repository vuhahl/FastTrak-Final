using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.ViewModels
{
    public class RestaurantsViewModel : ObservableObject
    {
        private readonly NutritionRepository _repository;

        /// <summary>
        /// List of restaurants displayed in the UI.
        /// </summary>
        public ObservableCollection<Restaurant> Restaurants { get; } = new();

        /// <summary>
        /// The selected restaurant in the list.
        /// </summary>
        [ObservableProperty]
        private Restaurant? selectedRestaurant;

        public RestaurantsViewModel(NutritionRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Loads restaurants from SQLite.
        /// Called every time the Restaurants page appears.
        /// </summary>
        public async Task LoadAsync()
        {
            Restaurants.Clear();
            var items = await _repository.GetRestaurantsAsync();

            foreach (var restaurant in items)
                Restaurants.Add(restaurant);
        }

        /// <summary>
        /// Navigates to MenuItemsPage with the selected restaurant's ID and name.
        /// </summary>
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
}
