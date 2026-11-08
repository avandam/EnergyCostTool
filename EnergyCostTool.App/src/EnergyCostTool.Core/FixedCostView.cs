namespace EnergyCostTool.Core;

public sealed record FixedCostView(
    DateTime StartDate,
    Guid TariffTypeId,
    string TariffTypeName,
    decimal Price,
    FixedCostTimeframe Timeframe,
    FixedCostEnergyType EnergyType);
