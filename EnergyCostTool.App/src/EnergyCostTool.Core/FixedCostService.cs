namespace EnergyCostTool.Core;

public sealed class FixedCostService
{
    private readonly FixedCostRepository fixedCostRepository;
    private readonly FixedCostTariffTypeRepository tariffTypeRepository;
    private List<FixedCostEntry> entries;
    private List<FixedCostTariffType> tariffTypes;

    public FixedCostService(FixedCostRepository fixedCostRepository, FixedCostTariffTypeRepository tariffTypeRepository)
    {
        ArgumentNullException.ThrowIfNull(fixedCostRepository);
        ArgumentNullException.ThrowIfNull(tariffTypeRepository);
        this.fixedCostRepository = fixedCostRepository;
        this.tariffTypeRepository = tariffTypeRepository;
        entries = fixedCostRepository.Load().ToList();
        tariffTypes = tariffTypeRepository.Load().ToList();
    }

    public IReadOnlyList<FixedCostTariffType> GetTariffTypes() =>
        tariffTypes.OrderBy(tariffType => tariffType.Name, StringComparer.OrdinalIgnoreCase).ToArray();

    public IReadOnlyList<FixedCostView> GetFixedCosts(FixedCostSortOrder sortOrder = FixedCostSortOrder.StartDate)
    {
        IEnumerable<FixedCostView> view = entries.Select(ToView);
        return sortOrder switch
        {
            FixedCostSortOrder.StartDate => view.OrderByDescending(entry => entry.StartDate).ToArray(),
            FixedCostSortOrder.TariffType => view
                .OrderBy(entry => entry.TariffTypeName, StringComparer.OrdinalIgnoreCase)
                .ThenByDescending(entry => entry.StartDate)
                .ToArray(),
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, "Unknown fixed-cost sort order.")
        };
    }

    public FixedCostTariffType AddTariffType(string name, FixedCostTimeframe timeframe, FixedCostEnergyType energyType)
    {
        FixedCostTariffType tariffType = new(Guid.NewGuid(), name, timeframe, energyType);
        tariffType = FixedCostRules.NormalizeAndValidateTariffType(tariffType, tariffTypes);
        List<FixedCostTariffType> updated = [.. tariffTypes, tariffType];
        tariffTypeRepository.Save(updated);
        tariffTypes = updated;
        return tariffType;
    }

    public FixedCostTariffType UpdateTariffType(Guid id, string name, FixedCostTimeframe timeframe, FixedCostEnergyType energyType)
    {
        int index = tariffTypes.FindIndex(tariffType => tariffType.Id == id);
        if (index < 0)
        {
            throw new FixedCostValidationException("The fixed-cost tariff type no longer exists.");
        }

        FixedCostTariffType updatedType = FixedCostRules.NormalizeAndValidateTariffType(
            new FixedCostTariffType(id, name, timeframe, energyType), tariffTypes, id);
        List<FixedCostTariffType> updated = [.. tariffTypes];
        updated[index] = updatedType;
        tariffTypeRepository.Save(updated);
        tariffTypes = updated;
        return updatedType;
    }

    public void DeleteTariffType(Guid id)
    {
        if (!tariffTypes.Any(tariffType => tariffType.Id == id))
        {
            throw new FixedCostValidationException("The fixed-cost tariff type no longer exists.");
        }

        if (entries.Any(entry => entry.TariffTypeId == id))
        {
            throw new FixedCostValidationException("This tariff type is in use by one or more fixed-cost entries and cannot be deleted.");
        }

        List<FixedCostTariffType> updated = tariffTypes.Where(tariffType => tariffType.Id != id).ToList();
        tariffTypeRepository.Save(updated);
        tariffTypes = updated;
    }

    public FixedCostEntry AddFixedCost(DateTime startDate, Guid tariffTypeId, decimal price)
    {
        EnsureTariffTypeExists(tariffTypeId);
        FixedCostEntry entry = FixedCostRules.NormalizeAndValidateEntry(new FixedCostEntry(startDate, tariffTypeId, price), entries);
        List<FixedCostEntry> updated = [.. entries, entry];
        fixedCostRepository.Save(updated);
        entries = updated;
        return entry;
    }

    public FixedCostEntry UpdateFixedCost(DateTime startDate, Guid originalTariffTypeId, Guid updatedTariffTypeId, decimal price)
    {
        int index = entries.FindIndex(entry => entry.StartDate.Date == startDate.Date && entry.TariffTypeId == originalTariffTypeId);
        if (index < 0)
        {
            throw new FixedCostValidationException("The fixed-cost entry no longer exists.");
        }

        EnsureTariffTypeExists(updatedTariffTypeId);
        FixedCostEntry original = entries[index];
        FixedCostEntry updatedEntry = FixedCostRules.NormalizeAndValidateEntry(
            new FixedCostEntry(original.StartDate, updatedTariffTypeId, price), entries, original);
        List<FixedCostEntry> updated = [.. entries];
        updated[index] = updatedEntry;
        fixedCostRepository.Save(updated);
        entries = updated;
        return updatedEntry;
    }

    public void DeleteFixedCost(DateTime startDate, Guid tariffTypeId)
    {
        int index = entries.FindIndex(entry => entry.StartDate.Date == startDate.Date && entry.TariffTypeId == tariffTypeId);
        if (index < 0)
        {
            throw new FixedCostValidationException("The fixed-cost entry no longer exists.");
        }

        List<FixedCostEntry> updated = [.. entries];
        updated.RemoveAt(index);
        fixedCostRepository.Save(updated);
        entries = updated;
    }

    private FixedCostView ToView(FixedCostEntry entry)
    {
        FixedCostTariffType? tariffType = tariffTypes.Find(type => type.Id == entry.TariffTypeId);
        if (tariffType is null)
        {
            throw new InvalidDataException($"Fixed-cost entry for {entry.StartDate:d} references a missing tariff type ({entry.TariffTypeId}).");
        }

        return new FixedCostView(entry.StartDate, entry.TariffTypeId, tariffType.Name, entry.Price, tariffType.Timeframe, tariffType.EnergyType);
    }

    private void EnsureTariffTypeExists(Guid tariffTypeId)
    {
        if (!tariffTypes.Any(tariffType => tariffType.Id == tariffTypeId))
        {
            throw new FixedCostValidationException("Select an existing fixed-cost tariff type.");
        }
    }
}
