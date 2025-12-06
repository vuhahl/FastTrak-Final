using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastTrak.Data;
using FastTrak.Views;
using FastTrak.Models;
using MenuItem = FastTrak.Models.MenuItem;

namespace FastTrak.ViewModels
{
    public partial class MenuItemsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly NutritionRepository _repository;

        [ObservableProperty]
        private string restaurantName = string.Empty;

        [ObservableProperty]
        private int restaurantId;

        public ObservableCollection<MenuItem> MenuItems { get; } = new();

        public MenuItemsViewModel(NutritionRepository repository)
        {
            _repository = repository;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RestaurantId = (int)query["RestaurantId"];
            RestaurantName = (string)query["RestaurantName"];
        }

        public async Task LoadAsync()
        {
            MenuItems.Clear();
            var items = await _repository.GetMenuItemsForRestaurantAsync(RestaurantId);

            foreach (var m in items)
                MenuItems.Add(m);
        }

        [RelayCommand]
        private async Task SelectMenuItemAsync(MenuItem item)
        {
            if (item == null)
                return;

            var parameters = new Dictionary<string, object>
        {
            { "MenuItemId", item.Id }
        };

            await Shell.Current.GoToAsync(nameof(CustomizationPage), parameters);
        }
    }
}
