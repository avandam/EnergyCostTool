using System.Globalization;

namespace EnergyCostTool.Core;

public static class EnergyUsageRules
{
    public static EnergyUsageRecord NormalizeAndValidate(EnergyUsageRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        if (record.SolarGeneration < 0
            || record.ElectricityHigh < 0
            || record.ReturnElectricityHigh < 0
            || record.ElectricityLow < 0
            || record.ReturnElectricityLow < 0
            || record.Gas < 0)
        {
            throw new EnergyUsageValidationException("Usage values must be zero or greater.");
        }

        long returned = (long)record.ReturnElectricityHigh + record.ReturnElectricityLow;
        if (returned > record.SolarGeneration)
        {
            throw new EnergyUsageValidationException(
                "Normal Return and Low Return cannot exceed Generation because Directly Used would be negative.");
        }

        DateTime normalizedMonth = new(record.Month.Year, record.Month.Month, 1, 0, 0, 0, DateTimeKind.Unspecified);
        return record with { Month = normalizedMonth };
    }

    public static bool IsSameMonth(DateTime first, DateTime second) =>
        first.Year == second.Year && first.Month == second.Month;

    public static string FormatMonth(DateTime month) =>
        month.ToString("MMM yyyy", CultureInfo.CurrentCulture);
}
