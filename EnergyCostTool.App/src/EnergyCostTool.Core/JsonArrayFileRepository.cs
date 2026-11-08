using System.Text.Json;

namespace EnergyCostTool.Core;

internal sealed class JsonArrayFileRepository<T>
{
    private readonly string filePath;
    private readonly JsonSerializerOptions serializerOptions;

    public JsonArrayFileRepository(string filePath, JsonSerializerOptions? serializerOptions = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        this.filePath = Path.GetFullPath(filePath);
        this.serializerOptions = serializerOptions ?? new JsonSerializerOptions();
    }

    public IReadOnlyList<T> Load()
    {
        if (!File.Exists(filePath))
        {
            return Array.Empty<T>();
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, serializerOptions)
            ?? throw new InvalidDataException($"The data file '{filePath}' did not contain a JSON array.");
    }

    public void Save(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        string? directory = Path.GetDirectoryName(filePath);
        if (directory is null || !Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory for the configured data file does not exist: {directory}");
        }

        string temporaryFile = Path.Combine(directory, $".{Path.GetFileName(filePath)}.{Guid.NewGuid():N}.tmp");
        try
        {
            File.WriteAllText(temporaryFile, JsonSerializer.Serialize(items, serializerOptions));
            File.Move(temporaryFile, filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryFile))
            {
                File.Delete(temporaryFile);
            }
        }
    }
}
