using FastTrak.Data;
using System.Text;
namespace FastTrak;

public partial class App : Application
{
    public App(NutritionRepository repo)
    {
        InitializeComponent();
        MainPage = new AppShell(); //main navigation container
        Task.Run(async () => await repo.InitializeAsync()); //Creates tables. Seeds initial data(restaurants, menu items, options), Runs asynchronously so the UI does not freeze
    }
}
