namespace FastTrak.Views;

using FastTrak.Models;
using FastTrak.ViewModels;

public partial class RestaurantsPage : ContentPage
{
    private RestaurantsViewModel ViewModel => (RestaurantsViewModel)BindingContext;

    public RestaurantsPage(RestaurantsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoadAsync();
    }

    private void RestaurantsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as Restaurant;
        if (selected == null)
            return;

        // Fire the VM command with the selected restaurant
        ViewModel.SelectRestaurantCommand.Execute(selected);

        // Clear selection so taps feel responsive
        ((CollectionView)sender).SelectedItem = null;
    }
}