namespace EnergyCostTool.Core;

public sealed class FixedCostTariffTypeRepository
{
    private readonly JsonArrayFileRepository<FixedCostTariffType> repository;

    public FixedCostTariffTypeRepository(string filePath)
    {
        repository = new JsonArrayFileRepository<FixedCostTariffType>(filePath, FixedCostJsonSerializerOptions.Create());
    }

    public IReadOnlyList<FixedCostTariffType> Load() => repository.Load();

    public void Save(IEnumerable<FixedCostTariffType> types) => repository.Save(types);
}
