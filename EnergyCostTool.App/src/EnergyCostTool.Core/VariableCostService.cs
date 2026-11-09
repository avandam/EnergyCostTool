namespace EnergyCostTool.Core;

public sealed class VariableCostService
{
    private readonly VariableCostRepository repository;
    private List<VariableCostEntry> entries;

    public VariableCostService(VariableCostRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        this.repository = repository;
        entries = repository.Load().ToList();
    }

    public IReadOnlyList<VariableCostEntry> GetEntries() =>
        entries.OrderByDescending(entry => entry.StartDate.Year)
            .ThenByDescending(entry => entry.StartDate.Month)
            .ToArray();

    public VariableCostEntry Add(VariableCostEntry candidate)
    {
        VariableCostEntry normalized = VariableCostRules.NormalizeAndValidateEntry(candidate, entries);
        SaveEntries([.. entries, normalized]);
        return normalized;
    }

    public VariableCostEntry Update(DateTime originalMonth, VariableCostEntry candidate)
    {
        int originalIndex = entries.FindIndex(entry => IsSameMonth(entry.StartDate, originalMonth));
        if (originalIndex < 0)
        {
            throw new VariableCostValidationException($"No variable-cost entry exists for {originalMonth:MMMM yyyy}.");
        }

        VariableCostEntry original = entries[originalIndex];
        VariableCostEntry normalized = VariableCostRules.NormalizeAndValidateEntry(
            candidate with { StartDate = original.StartDate }, entries, original);
        List<VariableCostEntry> updated = [.. entries];
        updated[originalIndex] = normalized;
        SaveEntries(updated);
        return normalized;
    }

    public void Delete(DateTime month)
    {
        int index = entries.FindIndex(entry => IsSameMonth(entry.StartDate, month));
        if (index < 0)
        {
            throw new VariableCostValidationException($"No variable-cost entry exists for {month:MMMM yyyy}.");
        }

        List<VariableCostEntry> updated = [.. entries];
        updated.RemoveAt(index);
        SaveEntries(updated);
    }

    private void SaveEntries(List<VariableCostEntry> updated)
    {
        repository.Save(updated);
        entries = updated;
    }

    private static bool IsSameMonth(DateTime first, DateTime second) =>
        first.Year == second.Year && first.Month == second.Month;
}
