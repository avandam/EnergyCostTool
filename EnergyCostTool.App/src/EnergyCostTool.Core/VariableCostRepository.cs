namespace EnergyCostTool.Core;

public sealed class VariableCostRepository
{
    private readonly JsonArrayFileRepository<VariableCostEntry> repository;

    public VariableCostRepository(string filePath)
    {
        repository = new JsonArrayFileRepository<VariableCostEntry>(filePath);
    }

    public IReadOnlyList<VariableCostEntry> Load() => repository.Load();

    public void Save(IEnumerable<VariableCostEntry> entries) => repository.Save(entries);
}
