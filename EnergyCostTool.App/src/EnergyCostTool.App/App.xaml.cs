using System.IO;
using System.Windows;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        try
        {
            AppSettings settings = AppSettings.Load();
            EnergyUsageService service = new(new EnergyUsageRepository(settings.ConsumptionFilePath));
            MainWindow window = new(service);
            MainWindow = window;
            window.Show();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or System.Text.Json.JsonException)
        {
            MessageBox.Show($"Energy usage data could not be loaded.\n\n{exception.Message}", "Energy Cost Tool", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }
}
