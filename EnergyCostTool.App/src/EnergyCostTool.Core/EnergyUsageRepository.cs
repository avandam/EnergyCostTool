namespace EnergyCostTool.Core;

public sealed class EnergyUsageRepository
{
    private readonly string filePath;

    public EnergyUsageRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        this.filePath = Path.GetFullPath(filePath);
    }

    public IReadOnlyList<EnergyUsageRecord> Load()
    {
        if (!File.Exists(filePath))
        {
            Save(Array.Empty<EnergyUsageRecord>());
            return Array.Empty<EnergyUsageRecord>();
        }

        string json = File.ReadAllText(filePath);
        return EnergyUsageJsonSerializer.Deserialize(json);
    }

    public void Save(IEnumerable<EnergyUsageRecord> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        string? directory = Path.GetDirectoryName(filePath);
        if (directory is null || !Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory for the configured energy usage file does not exist: {directory}");
        }

        string temporaryFile = Path.Combine(directory, $".{Path.GetFileName(filePath)}.{Guid.NewGuid():N}.tmp");
        try
        {
            File.WriteAllText(temporaryFile, EnergyUsageJsonSerializer.Serialize(records));
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
