namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class FatSecretSearchPage : ContentPage
{
	public FatSecretSearchPage(FatSecretSearchViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }

}