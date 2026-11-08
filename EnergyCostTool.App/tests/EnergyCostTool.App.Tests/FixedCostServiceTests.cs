using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class FixedCostServiceTests
{
    [Test]
    public void AddTariffTypeGeneratesAndPersistsGuid()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);

        FixedCostTariffType added = service.AddTariffType("  Gas service  ", FixedCostTimeframe.Monthly, FixedCostEnergyType.Generic);

        Assert.That(added.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(added.Name, Is.EqualTo("Gas service"));
        Assert.That(added.EnergyType, Is.EqualTo(FixedCostEnergyType.Generic));
        Assert.That(new FixedCostTariffTypeRepository(directory.FilePath("fixedCostTariffTypes.dat")).Load(), Is.EqualTo([added]));
    }

    [Test]
    public void EditTariffTypeKeepsIdAndUpdatesJoinedDisplay()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType tariffType = service.AddTariffType("Grid", FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);
        FixedCostEntry entry = service.AddFixedCost(new DateTime(2026, 9, 14), tariffType.Id, 1.234567m);

        FixedCostTariffType updatedType = service.UpdateTariffType(tariffType.Id, "Gas grid", FixedCostTimeframe.Daily, FixedCostEnergyType.Gas);
        FixedCostView view = service.GetFixedCosts().Single();

        Assert.That(updatedType.Id, Is.EqualTo(tariffType.Id));
        Assert.That(view.TariffTypeId, Is.EqualTo(entry.TariffTypeId));
        Assert.That(view.TariffTypeName, Is.EqualTo("Gas grid"));
        Assert.That(view.Timeframe, Is.EqualTo(FixedCostTimeframe.Daily));
        Assert.That(view.EnergyType, Is.EqualTo(FixedCostEnergyType.Gas));
    }

    [Test]
    public void DeleteTariffTypeRejectsWhenAnEntryReferencesIt()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType tariffType = service.AddTariffType("Electricity", FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);
        service.AddFixedCost(new DateTime(2026, 9, 1), tariffType.Id, 2.75m);

        Assert.Throws<FixedCostValidationException>(() => service.DeleteTariffType(tariffType.Id));
        Assert.That(service.GetTariffTypes(), Has.Count.EqualTo(1));
        Assert.That(service.GetFixedCosts(), Has.Count.EqualTo(1));
    }

    [Test]
    public void DeleteUnusedTariffTypeRemovesIt()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType tariffType = service.AddTariffType("Unused", FixedCostTimeframe.Daily, FixedCostEnergyType.Solar);

        service.DeleteTariffType(tariffType.Id);

        Assert.That(service.GetTariffTypes(), Is.Empty);
    }

    [Test]
    public void AddFixedCostRequiresExistingTariffType()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);

        Assert.Throws<FixedCostValidationException>(() => service.AddFixedCost(new DateTime(2026, 9, 1), Guid.NewGuid(), 1m));
    }

    [Test]
    public void AddAndEditFixedCostPreserveSignedDecimalAndOriginalDate()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType firstType = service.AddTariffType("Electricity", FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);
        FixedCostTariffType secondType = service.AddTariffType("Solar", FixedCostTimeframe.Daily, FixedCostEnergyType.Solar);
        DateTime startDate = new(2026, 9, 14);
        FixedCostEntry entry = service.AddFixedCost(startDate.AddHours(18), firstType.Id, -0.123456789m);

        FixedCostEntry updated = service.UpdateFixedCost(startDate, firstType.Id, secondType.Id, -0.987654321m);

        Assert.That(entry.StartDate, Is.EqualTo(startDate));
        Assert.That(updated.StartDate, Is.EqualTo(startDate));
        Assert.That(updated.TariffTypeId, Is.EqualTo(secondType.Id));
        Assert.That(updated.Price, Is.EqualTo(-0.987654321m));
    }

    [Test]
    public void AddFixedCostRejectsDuplicateStartDateAndTariffTypePair()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType tariffType = service.AddTariffType("Electricity", FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);
        service.AddFixedCost(new DateTime(2026, 9, 1), tariffType.Id, 1m);

        Assert.Throws<FixedCostValidationException>(() => service.AddFixedCost(new DateTime(2026, 9, 1, 13, 0, 0), tariffType.Id, 2m));
        Assert.That(service.GetFixedCosts(), Has.Count.EqualTo(1));
    }

    [Test]
    public void UpdateFixedCostRejectsPairAlreadyUsedByAnotherEntry()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType firstType = service.AddTariffType("Electricity", FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);
        FixedCostTariffType secondType = service.AddTariffType("Solar", FixedCostTimeframe.Daily, FixedCostEnergyType.Solar);
        DateTime startDate = new(2026, 9, 1);
        service.AddFixedCost(startDate, firstType.Id, 1m);
        service.AddFixedCost(startDate, secondType.Id, 2m);

        Assert.Throws<FixedCostValidationException>(() => service.UpdateFixedCost(startDate, firstType.Id, secondType.Id, 3m));
        Assert.That(service.GetFixedCosts(), Has.Count.EqualTo(2));
    }

    [Test]
    public void FixedCostsSortByDateOrTariffTypeThenDate()
    {
        using TemporaryDataDirectory directory = new();
        FixedCostService service = CreateService(directory);
        FixedCostTariffType gas = service.AddTariffType("Gas", FixedCostTimeframe.Monthly, FixedCostEnergyType.Gas);
        FixedCostTariffType energy = service.AddTariffType("Energy", FixedCostTimeframe.Daily, FixedCostEnergyType.Energy);
        service.AddFixedCost(new DateTime(2026, 1, 1), gas.Id, 1m);
        service.AddFixedCost(new DateTime(2026, 3, 1), energy.Id, 1m);
        service.AddFixedCost(new DateTime(2026, 2, 1), energy.Id, 1m);

        DateTime[] byDate = service.GetFixedCosts().Select(entry => entry.StartDate).ToArray();
        FixedCostView[] byType = service.GetFixedCosts(FixedCostSortOrder.TariffType).ToArray();

        Assert.That(byDate, Is.EqualTo([new DateTime(2026, 3, 1), new DateTime(2026, 2, 1), new DateTime(2026, 1, 1)]));
        Assert.That(byType.Select(entry => entry.TariffTypeName), Is.EqualTo(["Energy", "Energy", "Gas"]));
        Assert.That(byType.Take(2).Select(entry => entry.StartDate), Is.EqualTo([new DateTime(2026, 3, 1), new DateTime(2026, 2, 1)]));
    }

    private static FixedCostService CreateService(TemporaryDataDirectory directory) => new(
        new FixedCostRepository(directory.FilePath("fixedCosts.dat")),
        new FixedCostTariffTypeRepository(directory.FilePath("fixedCostTariffTypes.dat")));

    private sealed class TemporaryDataDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("energy-cost-tool-service-");

        public string FilePath(string name) => Path.Combine(directory.FullName, name);

        public void Dispose() => directory.Delete(recursive: true);
    }
}
