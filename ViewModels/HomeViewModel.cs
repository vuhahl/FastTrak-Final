using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Views;

namespace FastTrak.ViewModels
{
    /// <summary>
    /// Loads today's log count and handles navigation buttons.
    /// </summary>
    public partial class HomeViewModel : ObservableObject
    {
        private readonly NutritionRepository _repository;

        [ObservableProperty]
        private int todayItemCount;

        public HomeViewModel(NutritionRepository repo)
        {
            _repository = repo;
        }

        public async Task LoadAsync()
        {
            TodayItemCount = await _repository.GetTodaySelectionCountAsync();
        }

        [RelayCommand]
        private Task GoToRestaurantsAsync()
        {
            return Shell.Current.GoToAsync("//RestaurantsPage");
        }

        [RelayCommand]
        private Task GoToNutritionixSearchAsync()
        {
            return Shell.Current.GoToAsync(nameof(NutritionixSearchPage));
        }
    }
}

