namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class CustomizationPage : ContentPage
{
	public CustomizationPage(CustomizationViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CustomizationViewModel vm)
            await vm.LoadAsync();
    }
}