namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((HomeViewModel)BindingContext).LoadAsync();
    }
}