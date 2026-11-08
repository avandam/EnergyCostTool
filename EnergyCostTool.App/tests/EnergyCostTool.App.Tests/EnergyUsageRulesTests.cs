using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class EnergyUsageRulesTests
{
    [Test]
    public void DirectlyUsedIsGenerationMinusBothReturns()
    {
        EnergyUsageRecord record = CreateRecord(new DateTime(2026, 9, 1), 350, 155, 42);

        Assert.That(record.DirectlyUsed, Is.EqualTo(153));
    }

    [Test]
    public void NormalizeAndValidateSetsMonthToFirstDay()
    {
        EnergyUsageRecord record = CreateRecord(new DateTime(2026, 9, 24, 12, 30, 0), 350, 155, 42);

        EnergyUsageRecord normalized = EnergyUsageRules.NormalizeAndValidate(record);

        Assert.That(normalized.Month, Is.EqualTo(new DateTime(2026, 9, 1)));
    }

    [Test]
    public void NormalizeAndValidateRejectsNegativeUsageValues()
    {
        EnergyUsageRecord record = CreateRecord(new DateTime(2026, 9, 1), 350, 155, 42) with { Gas = -1 };

        Assert.Throws<EnergyUsageValidationException>(() => EnergyUsageRules.NormalizeAndValidate(record));
    }

    [Test]
    public void NormalizeAndValidateRejectsReturnsThatExceedGeneration()
    {
        EnergyUsageRecord record = CreateRecord(new DateTime(2026, 9, 1), 10, 7, 4);

        Assert.Throws<EnergyUsageValidationException>(() => EnergyUsageRules.NormalizeAndValidate(record));
    }

    [Test]
    public void NormalizeAndValidateAllowsReturnsEqualToGeneration()
    {
        EnergyUsageRecord record = CreateRecord(new DateTime(2026, 9, 1), 10, 7, 3);

        EnergyUsageRecord normalized = EnergyUsageRules.NormalizeAndValidate(record);

        Assert.That(normalized.DirectlyUsed, Is.Zero);
    }

    [Test]
    public void JsonRoundTripPreservesExistingFieldsAndOmitsDirectlyUsed()
    {
        const string json = "[{\"Month\":\"2026-09-01T00:00:00\",\"SolarGeneration\":350,\"ElectricityHigh\":142,\"ReturnElectricityHigh\":155,\"ElectricityLow\":225,\"ReturnElectricityLow\":42,\"Gas\":0}]";

        IReadOnlyList<EnergyUsageRecord> records = EnergyUsageJsonSerializer.Deserialize(json);
        string serialized = EnergyUsageJsonSerializer.Serialize(records);
        using System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(serialized);
        string[] fieldNames = document.RootElement[0].EnumerateObject().Select(property => property.Name).ToArray();

        Assert.That(records, Has.Count.EqualTo(1));
        Assert.That(records[0].DirectlyUsed, Is.EqualTo(153));
        string[] expectedFields = [
            "Month",
            "SolarGeneration",
            "ElectricityHigh",
            "ReturnElectricityHigh",
            "ElectricityLow",
            "ReturnElectricityLow",
            "Gas"
        ];
        Assert.That(fieldNames, Is.EquivalentTo(expectedFields));
    }

    [Test]
    public void LoadPreservesLegacyRecordWithNegativeDirectlyUsedWithoutRewritingFile()
    {
        const string json = "[{\"Month\":\"2026-09-01T00:00:00\",\"SolarGeneration\":10,\"ElectricityHigh\":142,\"ReturnElectricityHigh\":7,\"ElectricityLow\":225,\"ReturnElectricityLow\":4,\"Gas\":0}]";
        using TemporaryUsageFile file = new(json);

        EnergyUsageService service = new(new EnergyUsageRepository(file.Path));

        Assert.That(service.GetAll(), Has.Count.EqualTo(1));
        Assert.That(service.GetAll()[0].DirectlyUsed, Is.EqualTo(-1));
        Assert.That(File.ReadAllText(file.Path), Is.EqualTo(json));
    }

    [Test]
    public void LoadCreatesMissingFileAsEmptyJsonArray()
    {
        string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"energy-usage-{Guid.NewGuid():N}.dat");
        try
        {
            EnergyUsageService service = new(new EnergyUsageRepository(path));

            Assert.That(service.GetAll(), Is.Empty);
            Assert.That(File.ReadAllText(path), Is.EqualTo("[]"));
        }
        finally
        {
            if (File.Exists(path)) 
            {
                File.Delete(path);
            }
        }
    }

    [Test]
    public void AddRejectsDuplicateCalendarMonthEvenWhenDayDiffers()
    {
        using TemporaryUsageFile file = new(JsonFor(CreateRecord(new DateTime(2026, 9, 1), 350, 155, 42)));
        EnergyUsageService service = new(new EnergyUsageRepository(file.Path));
        EnergyUsageRecord duplicate = CreateRecord(new DateTime(2026, 9, 24), 350, 155, 42);

        Assert.Throws<EnergyUsageValidationException>(() => service.Add(duplicate));
        Assert.That(service.GetAll(), Has.Count.EqualTo(1));
    }

    [Test]
    public void UpdateRejectsDuplicateCalendarMonth()
    {
        EnergyUsageRecord september = CreateRecord(new DateTime(2026, 9, 1), 350, 155, 42);
        EnergyUsageRecord october = CreateRecord(new DateTime(2026, 10, 1), 450, 200, 100);
        using TemporaryUsageFile file = new(JsonFor(september, october));
        EnergyUsageService service = new(new EnergyUsageRepository(file.Path));

        Assert.Throws<EnergyUsageValidationException>(() => service.Update(
            new DateTime(2026, 10, 1), october with { Month = new DateTime(2026, 9, 12) }));
        Assert.That(service.GetAll(), Has.Count.EqualTo(2));
    }

    private static EnergyUsageRecord CreateRecord(DateTime month, int generation, int normalReturn, int lowReturn) => new()
    {
        Month = month,
        SolarGeneration = generation,
        ElectricityHigh = 142,
        ReturnElectricityHigh = normalReturn,
        ElectricityLow = 225,
        ReturnElectricityLow = lowReturn,
        Gas = 0
    };

    private static string JsonFor(params EnergyUsageRecord[] records) => EnergyUsageJsonSerializer.Serialize(records);

    private sealed class TemporaryUsageFile : IDisposable
    {
        public TemporaryUsageFile(string contents)
        {
            Path = System.IO.Path.GetTempFileName();
            File.WriteAllText(Path, contents);
        }

        public string Path { get; }

        public void Dispose()
        {
            File.Delete(Path);
        }
    }
}
