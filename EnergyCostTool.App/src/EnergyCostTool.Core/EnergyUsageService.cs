namespace EnergyCostTool.Core;

public sealed class EnergyUsageService
{
    private readonly EnergyUsageRepository repository;
    private List<EnergyUsageRecord> records;

    public EnergyUsageService(EnergyUsageRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        this.repository = repository;
        records = repository.Load().ToList();
    }

    public IReadOnlyList<EnergyUsageRecord> GetAll() =>
        records.OrderByDescending(record => record.Month.Year)
            .ThenByDescending(record => record.Month.Month)
            .ToArray();

    public IReadOnlyList<EnergyUsageRecord> GetForYear(int year) =>
        GetAll().Where(record => record.Month.Year == year).ToArray();

    public void Add(EnergyUsageRecord record)
    {
        EnergyUsageRecord normalized = EnergyUsageRules.NormalizeAndValidate(record);
        if (records.Any(existing => EnergyUsageRules.IsSameMonth(existing.Month, normalized.Month)))
        {
            throw DuplicateMonth(normalized.Month);
        }

        SaveRecords([.. records, normalized]);
    }

    public void Update(DateTime originalMonth, EnergyUsageRecord updatedRecord)
    {
        int originalIndex = records.FindIndex(record => EnergyUsageRules.IsSameMonth(record.Month, originalMonth));
        if (originalIndex < 0)
        {
            throw new EnergyUsageValidationException($"No usage record exists for {EnergyUsageRules.FormatMonth(originalMonth)}.");
        }

        EnergyUsageRecord normalized = EnergyUsageRules.NormalizeAndValidate(updatedRecord);
        if (records.Where((_, index) => index != originalIndex)
            .Any(existing => EnergyUsageRules.IsSameMonth(existing.Month, normalized.Month)))
        {
            throw DuplicateMonth(normalized.Month);
        }

        List<EnergyUsageRecord> updated = [.. records];
        updated[originalIndex] = normalized;
        SaveRecords(updated);
    }

    public void Delete(DateTime month)
    {
        int index = records.FindIndex(record => EnergyUsageRules.IsSameMonth(record.Month, month));
        if (index < 0)
        {
            throw new EnergyUsageValidationException($"No usage record exists for {EnergyUsageRules.FormatMonth(month)}.");
        }

        List<EnergyUsageRecord> updated = [.. records];
        updated.RemoveAt(index);
        SaveRecords(updated);
    }

    private void SaveRecords(List<EnergyUsageRecord> updated)
    {
        repository.Save(updated);
        records = updated;
    }

    private static EnergyUsageValidationException DuplicateMonth(DateTime month) =>
        new($"A usage record already exists for {EnergyUsageRules.FormatMonth(month)}.");
}
