namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class CalculatorPage : ContentPage
{
    private readonly CalculatorViewModel _vm;

    public CalculatorPage(CalculatorViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }


}