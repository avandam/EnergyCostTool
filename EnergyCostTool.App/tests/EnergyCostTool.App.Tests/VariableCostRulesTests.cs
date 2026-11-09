using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class VariableCostRulesTests
{
    [Test]
    public void NormalizeAndValidateEntryNormalizesMonthAndPreservesCompensationBefore2027()
    {
        VariableCostEntry candidate = CreateEntry(new DateTime(2026, 12, 19)) with
        {
            ReturnElectricityHighCompensation = 0.123456m,
            ReturnElectricityLowCompensation = -0.654321m
        };

        VariableCostEntry result = VariableCostRules.NormalizeAndValidateEntry(candidate, []);

        Assert.That(result.StartDate, Is.EqualTo(new DateTime(2026, 12, 1)));
        Assert.That(result.ReturnElectricityHighCompensation, Is.EqualTo(0.123456m));
        Assert.That(result.ReturnElectricityLowCompensation, Is.EqualTo(-0.654321m));
    }

    [Test]
    public void NormalizeAndValidateEntryRejectsDuplicateCalendarMonth()
    {
        VariableCostEntry existing = CreateEntry(new DateTime(2026, 9, 1));
        VariableCostEntry candidate = CreateEntry(new DateTime(2026, 9, 30));

        Assert.Throws<VariableCostValidationException>(() => VariableCostRules.NormalizeAndValidateEntry(candidate, [existing]));
    }

    [Test]
    public void NormalizeAndValidateEntryPreservesSignedPricesAndCompensationFromJanuary2027()
    {
        VariableCostEntry candidate = CreateEntry(new DateTime(2027, 1, 20)) with
        {
            ElectricityHigh = -0.123456789m,
            ReturnElectricityHighCompensation = -0.987654321m,
            ReturnElectricityLowCompensation = 0.123456789m
        };

        VariableCostEntry result = VariableCostRules.NormalizeAndValidateEntry(candidate, []);

        Assert.That(result.StartDate, Is.EqualTo(new DateTime(2027, 1, 1)));
        Assert.That(result.ElectricityHigh, Is.EqualTo(-0.123456789m));
        Assert.That(result.ReturnElectricityHighCompensation, Is.EqualTo(-0.987654321m));
        Assert.That(result.ReturnElectricityLowCompensation, Is.EqualTo(0.123456789m));
    }

    private static VariableCostEntry CreateEntry(DateTime month) =>
        new(month, 0.25m, 0.15m, 0.04m, 0.02m, 0.03m, 0.01m, 1.2m);
}
