namespace EnergyCostTool.Core;

public sealed record VariableCostEntry(
    DateTime StartDate,
    decimal ElectricityHigh,
    decimal ElectricityLow,
    decimal ReturnElectricityHighCost,
    decimal ReturnElectricityHighCompensation,
    decimal ReturnElectricityLowCost,
    decimal ReturnElectricityLowCompensation,
    decimal Gas);
