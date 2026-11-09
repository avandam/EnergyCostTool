using EnergyCostTool.Core;
using NUnit.Framework;

namespace EnergyCostTool.App.Tests;

[TestFixture]
public sealed class VariableCostRepositoryTests
{
    [Test]
    public void MissingVariableCostFileLoadsAsEmptyWithoutCreatingIt()
    {
        using TemporaryDataDirectory directory = new();
        string path = directory.FilePath("variableCost.dat");
        VariableCostRepository repository = new(path);

        Assert.That(repository.Load(), Is.Empty);
        Assert.That(File.Exists(path), Is.False);
    }

    [Test]
    public void RoundTripsAllSignedHighPrecisionPrices()
    {
        using TemporaryDataDirectory directory = new();
        VariableCostRepository repository = new(directory.FilePath("variableCost.dat"));
        VariableCostEntry entry = new(
            new DateTime(2027, 1, 1),
            -0.1234567890123456789012345678m,
            0.2345678901234567890123456789m,
            -0.345678901234567890123456789m,
            0.45678901234567890123456789m,
            -0.5678901234567890123456789m,
            0.67890123456789012345678901m,
            -0.78901234567890123456789012m);

        repository.Save([entry]);

        Assert.That(repository.Load(), Is.EqualTo([entry]));
    }

    private sealed class TemporaryDataDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("energy-cost-tool-variable-cost-repository-");

        public string FilePath(string name) => Path.Combine(directory.FullName, name);

        public void Dispose() => directory.Delete(recursive: true);
    }
}
