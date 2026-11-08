using System.Text.Json.Serialization;

namespace EnergyCostTool.Core;

public sealed record EnergyUsageRecord
{
    public DateTime Month { get; init; }

    public int SolarGeneration { get; init; }

    public int ElectricityHigh { get; init; }

    public int ReturnElectricityHigh { get; init; }

    public int ElectricityLow { get; init; }

    public int ReturnElectricityLow { get; init; }

    public int Gas { get; init; }

    [JsonIgnore]
    public long DirectlyUsed => (long)SolarGeneration - ReturnElectricityHigh - ReturnElectricityLow;
}
