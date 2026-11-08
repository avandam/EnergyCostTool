namespace EnergyCostTool.Core;

public sealed record FixedCostEntry(DateTime StartDate, Guid TariffTypeId, decimal Price);
