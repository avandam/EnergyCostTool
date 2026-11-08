using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class FixedCostModelTests
{
    [Test]
    public void FixedCostTariffTypeRetainsGuidAndAttributes()
    {
        Guid id = Guid.NewGuid();

        FixedCostTariffType tariffType = new(id, "Electricity standing charge", FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);

        Assert.That(tariffType.Id, Is.EqualTo(id));
        Assert.That(tariffType.Name, Is.EqualTo("Electricity standing charge"));
        Assert.That(tariffType.Timeframe, Is.EqualTo(FixedCostTimeframe.Monthly));
        Assert.That(tariffType.EnergyType, Is.EqualTo(FixedCostEnergyType.Energy));
    }

    [Test]
    public void FixedCostEntryRetainsDateTypeReferenceAndSignedDecimalPrice()
    {
        DateTime startDate = new(2026, 9, 14);
        Guid typeId = Guid.NewGuid();
        const decimal price = -0.123456789m;

        FixedCostEntry entry = new(startDate, typeId, price);

        Assert.That(entry.StartDate, Is.EqualTo(startDate));
        Assert.That(entry.TariffTypeId, Is.EqualTo(typeId));
        Assert.That(entry.Price, Is.EqualTo(price));
        Assert.That(entry.Price, Is.Negative);
    }

    [Test]
    public void EnergyTypeChoicesIncludeGeneric()
    {
        Assert.That(Enum.GetValues<FixedCostEnergyType>(), Is.EqualTo([
            FixedCostEnergyType.Energy,
            FixedCostEnergyType.Gas,
            FixedCostEnergyType.Solar,
            FixedCostEnergyType.Generic]));
    }
}
