namespace FastTrak.Views;
using FastTrak.ViewModels;

public partial class MenuItemsPage : ContentPage
{
    private MenuItemsViewModel ViewModel => (MenuItemsViewModel)BindingContext;

    public MenuItemsPage(MenuItemsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoadAsync();
    }

    private void MenuItemsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as MenuItem;
        if (selected == null)
            return;

        ViewModel.SelectMenuItemCommand.Execute(selected);
        ((CollectionView)sender).SelectedItem = null;
    }

    private void Customize_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is object raw)
        {
            var item = raw as FastTrak.Models.MenuItem;

            if (item != null)
                ViewModel.SelectMenuItemCommand.Execute(item);
        }

    }


}