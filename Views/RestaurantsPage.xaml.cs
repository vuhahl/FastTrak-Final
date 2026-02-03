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

        // AdMob Diagnostic
        System.Diagnostics.Debug.WriteLine("RestaurantsPage: OnAppearing - Checking for AdMob banner");

        // Give the ad time to load
        await Task.Delay(2000);

        // Check if ad view exists in the visual tree
        var adView = this.FindByName<Plugin.MauiMTAdmob.Controls.MTAdView>("AdBanner");
        if (adView != null)
        {
            System.Diagnostics.Debug.WriteLine($"AdMob: Banner found, Height={adView.Height}, IsVisible={adView.IsVisible}");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("AdMob: Banner NOT found in view hierarchy");
        }
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