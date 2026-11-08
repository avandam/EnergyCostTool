using System.Text.Json;

namespace EnergyCostTool.Core;

public static class EnergyUsageJsonSerializer
{
    public static IReadOnlyList<EnergyUsageRecord> Deserialize(string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        return JsonSerializer.Deserialize<List<EnergyUsageRecord>>(json)
            ?? throw new InvalidDataException("The energy usage file did not contain a JSON array.");
    }

    public static string Serialize(IEnumerable<EnergyUsageRecord> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        return JsonSerializer.Serialize(records);
    }
}
