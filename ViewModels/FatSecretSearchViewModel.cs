using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Models;
using FastTrak.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.ViewModels
{
    public partial class FatSecretSearchViewModel : ObservableObject
    {
        private readonly FatSecretService _service;
        private readonly NutritionRepository _repo;

        [ObservableProperty]
        string query;

        public ObservableCollection<FatSecretFoodSearchItem> Results { get; } =
            new ObservableCollection<FatSecretFoodSearchItem>();

        public FatSecretSearchViewModel(FatSecretService service, NutritionRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        [RelayCommand]
        public async Task SearchAsync()
        {
            Results.Clear();
            var items = await _service.SearchFoodsAsync(Query);

            foreach (var item in items)
                Results.Add(item);
        }

        [RelayCommand]
        public async Task AddToLogAsync(FatSecretFoodSearchItem item)
        {
            if (item == null) return;

            var details = await _service.GetFoodDetailsAsync(item.food_id);
            var s = details.food.servings.serving;

            var logged = new LoggedItem
            {
                MenuItemId = 0,
                LoggedAt = DateTime.Now,
                Quantity = 1,
                NameOverride = item.food_name,
                CaloriesOverride = (int)s.calories,
                ProteinOverride = (decimal)s.protein,
                CarbsOverride = (decimal)s.carbohydrate,
                FatOverride = (decimal)s.fat,
                SodiumOverride = (int)s.sodium
            };

            await _repo.InsertLoggedItemAsync(logged);

            await Shell.Current.DisplayAlert("Added", $"{item.food_name} added to today's log.", "OK");
        }
    }
}
