using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;

namespace FastTrak.ViewModels

/// Loads today's log count and handles navigation buttons.
{
    public partial class HomeViewModel: ObservableObject
    {
        private readonly NutritionRepository _repository;

        /// <summary>
        /// Number of items the user logged today.
        /// Bound to the card displaying "X items logged".
        /// </summary>
        [ObservableProperty]
        private int todayItemCount;

        public HomeViewModel(NutritionRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Loads fresh daily data every time the page appears.
        /// </summary>
        public async Task LoadAsync()
        {
            TodayItemCount = await _repository.GetTodaySelectionCountAsync();
        }

        /// <summary>
        /// Navigate to the Restaurants tab.
        /// </summary>
        [RelayCommand]
        private Task GoToRestaurantsAsync()
        {
            return Shell.Current.GoToAsync("//RestaurantsPage");
        }

        /// <summary>
        /// Navigate to the Nutritionix cloud search screen.
        /// </summary>
        [RelayCommand]
        private Task GoToNutritionixSearchAsync()
        {
            return Shell.Current.GoToAsync(nameof(NutritionixSearchPage));
        }
    }
}
