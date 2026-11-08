namespace EnergyCostTool.Core;

public static class FixedCostRules
{
    public static FixedCostTariffType NormalizeAndValidateTariffType(
        FixedCostTariffType candidate,
        IEnumerable<FixedCostTariffType> existingTypes,
        Guid? excludedTypeId = null)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(existingTypes);

        if (candidate.Id == Guid.Empty)
        {
            throw new FixedCostValidationException("A fixed-cost tariff type must have a valid identifier.");
        }

        string name = candidate.Name?.Trim() ?? string.Empty;
        if (name.Length == 0)
        {
            throw new FixedCostValidationException("Enter a name for the fixed-cost tariff type.");
        }

        if (!Enum.IsDefined(candidate.Timeframe))
        {
            throw new FixedCostValidationException("Select Daily or Monthly as the timeframe.");
        }

        if (!Enum.IsDefined(candidate.EnergyType))
        {
            throw new FixedCostValidationException("Select Energy, Gas, or Solar as the energy type.");
        }

        bool duplicateName = existingTypes.Any(existing =>
            existing.Id != excludedTypeId &&
            string.Equals(existing.Name.Trim(), name, StringComparison.OrdinalIgnoreCase));
        if (duplicateName)
        {
            throw new FixedCostValidationException($"A fixed-cost tariff type named '{name}' already exists.");
        }

        return candidate with { Name = name };
    }

    public static FixedCostEntry NormalizeAndValidateEntry(
        FixedCostEntry candidate,
        IEnumerable<FixedCostEntry> existingEntries,
        FixedCostEntry? excludedEntry = null)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(existingEntries);

        if (candidate.TariffTypeId == Guid.Empty)
        {
            throw new FixedCostValidationException("Select a fixed-cost tariff type.");
        }

        FixedCostEntry normalized = candidate with { StartDate = candidate.StartDate.Date };
        bool duplicate = existingEntries.Any(existing =>
            existing != excludedEntry &&
            existing.StartDate.Date == normalized.StartDate &&
            existing.TariffTypeId == normalized.TariffTypeId);
        if (duplicate)
        {
            throw new FixedCostValidationException("A fixed-cost entry already exists for this start date and tariff type.");
        }

        return normalized;
    }
}
