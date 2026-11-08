using System.Text.Json;

namespace EnergyCostTool.Core;

public sealed class EnergyCostToolSettings
{
    public string ConsumptionFilePath { get; init; } = string.Empty;

    public string FixedCostsFilePath { get; init; } = string.Empty;

    public string FixedCostTariffTypesFilePath { get; init; } = string.Empty;

    public static EnergyCostToolSettings Load() =>
        Load(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));

    public static EnergyCostToolSettings Load(string settingsPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settingsPath);
        string json = File.ReadAllText(settingsPath);
        EnergyCostToolSettings settings = JsonSerializer.Deserialize<EnergyCostToolSettings>(json)
            ?? throw new InvalidDataException("The application settings file is empty or invalid.");

        ValidatePath(settings.ConsumptionFilePath, nameof(ConsumptionFilePath));
        ValidatePath(settings.FixedCostsFilePath, nameof(FixedCostsFilePath));
        ValidatePath(settings.FixedCostTariffTypesFilePath, nameof(FixedCostTariffTypesFilePath));
        return settings;
    }

    private static void ValidatePath(string path, string settingName)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidDataException($"{settingName} must be set in appsettings.json.");
        }
    }
}
