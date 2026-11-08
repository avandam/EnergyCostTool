using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnergyCostTool.Core;

internal static class FixedCostJsonSerializerOptions
{
    public static JsonSerializerOptions Create() => new()
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
