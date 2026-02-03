using FastTrak.Views;

namespace FastTrak;

public partial class App : Application
{
    public App(LoadingPage loadingPage)
    {
        InitializeComponent();

        // Start with LoadingPage which handles database initialization
        // This ensures the DB is fully seeded BEFORE the user sees the main UI,
        // preventing race conditions where pages query empty tables
        MainPage = loadingPage;
    }
}
