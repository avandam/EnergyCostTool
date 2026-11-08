using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class FixedCostRulesTests
{
    [Test]
    public void NormalizeAndValidateTariffTypeTrimsName()
    {
        FixedCostTariffType candidate = CreateTariffType("  Energy  ");

        FixedCostTariffType result = FixedCostRules.NormalizeAndValidateTariffType(candidate, []);

        Assert.That(result.Name, Is.EqualTo("Energy"));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void NormalizeAndValidateTariffTypeRejectsEmptyName(string name)
    {
        FixedCostTariffType candidate = CreateTariffType(name);

        Assert.Throws<FixedCostValidationException>(() => FixedCostRules.NormalizeAndValidateTariffType(candidate, []));
    }

    [Test]
    public void NormalizeAndValidateTariffTypeRejectsCaseInsensitiveDuplicateName()
    {
        FixedCostTariffType existing = CreateTariffType("Gas charge");
        FixedCostTariffType candidate = CreateTariffType(" GAS CHARGE ");

        Assert.Throws<FixedCostValidationException>(() => FixedCostRules.NormalizeAndValidateTariffType(candidate, [existing]));
    }

    [Test]
    public void NormalizeAndValidateTariffTypeAllowsSameNameWhenEditingThatType()
    {
        FixedCostTariffType existing = CreateTariffType("Gas charge");

        FixedCostTariffType result = FixedCostRules.NormalizeAndValidateTariffType(existing with { Name = " GAS CHARGE " }, [existing], existing.Id);

        Assert.That(result.Id, Is.EqualTo(existing.Id));
        Assert.That(result.Name, Is.EqualTo("GAS CHARGE"));
    }

    [Test]
    public void NormalizeAndValidateTariffTypeRejectsInvalidChoices()
    {
        FixedCostTariffType candidate = CreateTariffType("Invalid") with { EnergyType = (FixedCostEnergyType)99 };

        Assert.Throws<FixedCostValidationException>(() => FixedCostRules.NormalizeAndValidateTariffType(candidate, []));
    }

    [Test]
    public void NormalizeAndValidateEntryDropsTimeAndRejectsDuplicateDateAndType()
    {
        Guid tariffTypeId = Guid.NewGuid();
        FixedCostEntry existing = new(new DateTime(2026, 9, 1), tariffTypeId, 4.25m);
        FixedCostEntry candidate = new(new DateTime(2026, 9, 1, 18, 30, 0), tariffTypeId, 4.25m);

        Assert.Throws<FixedCostValidationException>(() => FixedCostRules.NormalizeAndValidateEntry(candidate, [existing]));
    }

    [Test]
    public void NormalizeAndValidateEntryAllowsSameDateForDifferentType()
    {
        FixedCostEntry existing = new(new DateTime(2026, 9, 1), Guid.NewGuid(), 4.25m);
        FixedCostEntry candidate = new(new DateTime(2026, 9, 1, 18, 30, 0), Guid.NewGuid(), 4.25m);

        FixedCostEntry result = FixedCostRules.NormalizeAndValidateEntry(candidate, [existing]);

        Assert.That(result.StartDate, Is.EqualTo(new DateTime(2026, 9, 1)));
    }

    private static FixedCostTariffType CreateTariffType(string name) =>
        new(Guid.NewGuid(), name, FixedCostTimeframe.Monthly, FixedCostEnergyType.Energy);
}
