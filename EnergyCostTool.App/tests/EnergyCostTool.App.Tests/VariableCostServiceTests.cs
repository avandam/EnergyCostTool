using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class VariableCostServiceTests
{
    [Test]
    public void AddAndUpdatePersistsEntryAndKeepsOriginalMonth()
    {
        using TemporaryDataDirectory directory = new();
        VariableCostService service = CreateService(directory);
        VariableCostEntry original = service.Add(CreateEntry(new DateTime(2026, 9, 18)));
        VariableCostEntry candidate = CreateEntry(new DateTime(2028, 4, 27)) with
        {
            ElectricityHigh = -0.123456789m,
            ReturnElectricityHighCompensation = 0.456789m,
            ReturnElectricityLowCompensation = -0.987654m
        };

        VariableCostEntry updated = service.Update(original.StartDate.AddDays(23), candidate);

        Assert.That(updated.StartDate, Is.EqualTo(new DateTime(2026, 9, 1)));
        Assert.That(updated.ElectricityHigh, Is.EqualTo(-0.123456789m));
        Assert.That(updated.ReturnElectricityHighCompensation, Is.EqualTo(0.456789m));
        Assert.That(updated.ReturnElectricityLowCompensation, Is.EqualTo(-0.987654m));
        Assert.That(new VariableCostRepository(directory.FilePath("variableCost.dat")).Load(), Is.EqualTo([updated]));

        service.Delete(original.StartDate);

        Assert.That(service.GetEntries(), Is.Empty);
        Assert.That(new VariableCostRepository(directory.FilePath("variableCost.dat")).Load(), Is.Empty);
    }

    [Test]
    public void GetEntriesReturnsNewestMonthFirst()
    {
        using TemporaryDataDirectory directory = new();
        VariableCostService service = CreateService(directory);
        service.Add(CreateEntry(new DateTime(2025, 12, 1)));
        service.Add(CreateEntry(new DateTime(2026, 2, 1)));
        service.Add(CreateEntry(new DateTime(2026, 1, 1)));

        DateTime[] months = service.GetEntries().Select(entry => entry.StartDate).ToArray();

        Assert.That(months, Is.EqualTo([new DateTime(2026, 2, 1), new DateTime(2026, 1, 1), new DateTime(2025, 12, 1)]));
    }

    private static VariableCostService CreateService(TemporaryDataDirectory directory) =>
        new(new VariableCostRepository(directory.FilePath("variableCost.dat")));

    private static VariableCostEntry CreateEntry(DateTime month) =>
        new(month, 0.25m, 0.15m, 0.04m, 0.02m, 0.03m, 0.01m, 1.2m);

    private sealed class TemporaryDataDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("energy-cost-tool-variable-cost-service-");

        public string FilePath(string name) => Path.Combine(directory.FullName, name);

        public void Dispose() => directory.Delete(recursive: true);
    }
}
