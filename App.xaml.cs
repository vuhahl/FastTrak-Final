using FastTrak.Data;
namespace FastTrak;

public partial class App : Application
{
    public App(NutritionRepository repo)
    {
        InitializeComponent();
        MainPage = new AppShell();
        Task.Run(async () => await repo.InitializeAsync());
    }
}
