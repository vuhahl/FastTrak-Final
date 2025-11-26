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
}