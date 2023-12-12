using EnergyCostTool.Data;
using EnergyCostTool.Exceptions;
using NUnit.Framework;

namespace EnergyCostTool.Tests.Data;

[TestFixture()]
public class EnergyConsumptionTests
{
    [Test()]
    public void EnergyConsumptionInvalidDateTest()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<EnergyConsumptionException>(() => new EnergyConsumption(new DateTime(2023, 10, 5), 500, 600, 700, 800, 900, 1000));
    }
}