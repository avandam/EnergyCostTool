using System.Text.Json;
using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class EnergyCostToolSettingsTests
{
    [Test]
    public void LoadUsesAllConfiguredDataFilePaths()
    {
        using TemporaryDataDirectory directory = new();
        string settingsPath = directory.FilePath("appsettings.json");
        EnergyCostToolSettings configured = new()
        {
            ConsumptionFilePath = directory.FilePath("custom-usage.dat"),
            FixedCostsFilePath = directory.FilePath("custom-fixed-costs.dat"),
            FixedCostTariffTypesFilePath = directory.FilePath("custom-fixed-cost-types.dat"),
            VariableCostFilePath = directory.FilePath("custom-variable-costs.dat")
        };
        File.WriteAllText(settingsPath, JsonSerializer.Serialize(configured));

        EnergyCostToolSettings loaded = EnergyCostToolSettings.Load(settingsPath);
        FixedCostRepository fixedCostRepository = new(loaded.FixedCostsFilePath);
        FixedCostTariffTypeRepository tariffTypeRepository = new(loaded.FixedCostTariffTypesFilePath);
        fixedCostRepository.Save([]);
        tariffTypeRepository.Save([]);

        Assert.That(loaded.ConsumptionFilePath, Is.EqualTo(configured.ConsumptionFilePath));
        Assert.That(loaded.FixedCostsFilePath, Is.EqualTo(configured.FixedCostsFilePath));
        Assert.That(loaded.FixedCostTariffTypesFilePath, Is.EqualTo(configured.FixedCostTariffTypesFilePath));
        Assert.That(loaded.VariableCostFilePath, Is.EqualTo(configured.VariableCostFilePath));
        Assert.That(File.Exists(configured.FixedCostsFilePath), Is.True);
        Assert.That(File.Exists(configured.FixedCostTariffTypesFilePath), Is.True);
    }

    [Test]
    public void LoadRejectsMissingFixedCostsPath()
    {
        using TemporaryDataDirectory directory = new();
        string settingsPath = directory.FilePath("appsettings.json");
        string json = "{\"ConsumptionFilePath\":\"usage.dat\",\"FixedCostTariffTypesFilePath\":\"types.dat\"}";
        File.WriteAllText(settingsPath, json);

        Assert.Throws<InvalidDataException>(() => EnergyCostToolSettings.Load(settingsPath));
    }

    [Test]
    public void LoadRejectsMissingVariableCostPath()
    {
        using TemporaryDataDirectory directory = new();
        string settingsPath = directory.FilePath("appsettings.json");
        string json = "{\"ConsumptionFilePath\":\"usage.dat\",\"FixedCostsFilePath\":\"fixed.dat\",\"FixedCostTariffTypesFilePath\":\"types.dat\"}";
        File.WriteAllText(settingsPath, json);

        Assert.Throws<InvalidDataException>(() => EnergyCostToolSettings.Load(settingsPath));
    }

    private sealed class TemporaryDataDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("energy-cost-tool-settings-");

        public string FilePath(string name) => Path.Combine(directory.FullName, name);

        public void Dispose() => directory.Delete(recursive: true);
    }
}
