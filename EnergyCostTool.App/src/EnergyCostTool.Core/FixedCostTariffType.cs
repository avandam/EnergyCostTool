namespace EnergyCostTool.Core;

public sealed record FixedCostTariffType(
    Guid Id,
    string Name,
    FixedCostTimeframe Timeframe,
    FixedCostEnergyType EnergyType);
