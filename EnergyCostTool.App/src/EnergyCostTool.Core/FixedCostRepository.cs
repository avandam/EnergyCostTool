namespace EnergyCostTool.Core;

public sealed class FixedCostRepository
{
    private readonly JsonArrayFileRepository<FixedCostEntry> repository;

    public FixedCostRepository(string filePath)
    {
        repository = new JsonArrayFileRepository<FixedCostEntry>(filePath, FixedCostJsonSerializerOptions.Create());
    }

    public IReadOnlyList<FixedCostEntry> Load() => repository.Load();

    public void Save(IEnumerable<FixedCostEntry> entries) => repository.Save(entries);
}
