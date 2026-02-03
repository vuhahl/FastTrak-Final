using FastTrak.Data;

namespace FastTrak.Views;

public partial class LoadingPage : ContentPage
{
    private readonly NutritionRepository _repo;

    public LoadingPage(NutritionRepository repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Wait for database initialization to complete
        // This ensures all tables are created and seeded before the user sees the main UI
        await _repo.InitializeAsync();

        // Navigate to the main app shell
        // Using assignment instead of navigation prevents the user from going "back" to the loading screen
        Application.Current!.MainPage = new AppShell();
    }
}
