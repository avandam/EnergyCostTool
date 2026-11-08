using System.Text.Json;
using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class FixedCostRepositoryTests
{
    [Test]
    public void MissingFixedCostAndTariffTypeFilesLoadAsEmptyWithoutCreatingFiles()
    {
        using TemporaryDataDirectory directory = new();
        string fixedCostPath = directory.FilePath("fixedCosts.dat");
        string tariffTypePath = directory.FilePath("fixedCostTariffTypes.dat");
        FixedCostRepository fixedCosts = new(fixedCostPath);
        FixedCostTariffTypeRepository tariffTypes = new(tariffTypePath);

        Assert.That(fixedCosts.Load(), Is.Empty);
        Assert.That(tariffTypes.Load(), Is.Empty);
        Assert.That(File.Exists(fixedCostPath), Is.False);
        Assert.That(File.Exists(tariffTypePath), Is.False);
    }

    [Test]
    public void FixedCostRepositoryRoundTripsSignedHighPrecisionPriceAndGuidReference()
    {
        using TemporaryDataDirectory directory = new();
        string path = directory.FilePath("fixedCosts.dat");
        FixedCostRepository repository = new(path);
        Guid tariffTypeId = Guid.NewGuid();
        const decimal price = -0.1234567890123456789012345678m;
        FixedCostEntry entry = new(new DateTime(2026, 9, 14), tariffTypeId, price);

        repository.Save([entry]);

        FixedCostEntry actual = repository.Load().Single();
        Assert.That(actual, Is.EqualTo(entry));
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
        string[] fieldNames = document.RootElement[0].EnumerateObject().Select(property => property.Name).ToArray();
        Assert.That(fieldNames, Is.EquivalentTo(["StartDate", "TariffTypeId", "Price"]));
        Assert.That(document.RootElement[0].TryGetProperty("Name", out _), Is.False);
    }

    [Test]
    public void TariffTypeRepositoryRoundTripsGuidAndStringEnumValues()
    {
        using TemporaryDataDirectory directory = new();
        string path = directory.FilePath("fixedCostTariffTypes.dat");
        FixedCostTariffTypeRepository repository = new(path);
        FixedCostTariffType tariffType = new(Guid.NewGuid(), "Generic charge", FixedCostTimeframe.Daily, FixedCostEnergyType.Generic);

        repository.Save([tariffType]);

        Assert.That(repository.Load(), Is.EqualTo([tariffType]));
        string json = File.ReadAllText(path);
        Assert.That(json, Does.Contain("\"Timeframe\":\"Daily\""));
        Assert.That(json, Does.Contain("\"EnergyType\":\"Generic\""));
        Assert.That(json, Does.Contain(tariffType.Id.ToString()));
    }

    private sealed class TemporaryDataDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("energy-cost-tool-");

        public string FilePath(string name) => Path.Combine(directory.FullName, name);

        public void Dispose() => directory.Delete(recursive: true);
    }
}
