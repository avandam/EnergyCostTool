namespace EnergyCostTool.Core;

public static class VariableCostRules
{
    public static VariableCostEntry NormalizeAndValidateEntry(
        VariableCostEntry candidate,
        IEnumerable<VariableCostEntry> existingEntries,
        VariableCostEntry? excludedEntry = null)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(existingEntries);

        DateTime month = new(candidate.StartDate.Year, candidate.StartDate.Month, 1);
        if (existingEntries.Any(existing =>
                existing != excludedEntry &&
                existing.StartDate.Year == month.Year &&
                existing.StartDate.Month == month.Month))
        {
            throw new VariableCostValidationException($"A variable-cost entry already exists for {month:MMMM yyyy}.");
        }

        return candidate with { StartDate = month };
    }
}
