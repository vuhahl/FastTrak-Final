namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class RestaurantsPage : ContentPage
{
    public RestaurantsViewModel ViewModel { get; }

    public RestaurantsPage(RestaurantsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        ViewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoadAsync();
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is RestaurantsViewModel vm)
            await vm.SelectRestaurantCommand.ExecuteAsync(null);
    }
}