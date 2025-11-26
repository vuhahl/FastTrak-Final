namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class MenuItemsPage : ContentPage
{
    public MenuItemsViewModel ViewModel { get; }

    public MenuItemsPage(MenuItemsViewModel vm)
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