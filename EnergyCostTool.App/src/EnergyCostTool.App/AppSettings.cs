using System.IO;
using System.Text.Json;

namespace EnergyCostTool.App;

internal sealed class AppSettings
{
    public string ConsumptionFilePath { get; init; } = string.Empty;

    public static AppSettings Load()
    {
        string settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        string json = File.ReadAllText(settingsPath);
        AppSettings settings = JsonSerializer.Deserialize<AppSettings>(json)
            ?? throw new InvalidDataException("The application settings file is empty or invalid.");

        if (string.IsNullOrWhiteSpace(settings.ConsumptionFilePath))
        {
            throw new InvalidDataException("ConsumptionFilePath must be set in appsettings.json.");
        }

        return settings;
    }
}
