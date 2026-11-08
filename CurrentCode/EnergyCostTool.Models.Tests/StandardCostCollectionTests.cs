using EnergyCostTool.Models;
using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.Models.Exceptions;
using NUnit.Framework;

namespace EnergyCostTool.Models.Tests;

[TestFixture()]
public class StandardCostCollectionTests
{
    [Test()]
    public void AddTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
            
        standardCosts.AddOrUpdate(standardCost);

        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");
        Assert.AreEqual(standardCost, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas), "The standard cost should be correct");
    }

    [Test()]
    public void AddDifferentForSameDateTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 0.99);

        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);

        Assert.AreEqual(2, standardCosts.Get().Count, "There should be 2 standard cost");
        Assert.AreEqual(standardCost, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas), "The StandingChargeGas standard cost should be correct");
        Assert.AreEqual(standardCost2, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity), "The TransportCostElectricity standard cost should be correct");
    }

    [Test()]
    public void AddForDifferentDateTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 0.99);

        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);

        Assert.AreEqual(2, standardCosts.Get().Count, "There should be 2 standard cost");
        Assert.AreEqual(standardCost, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas), "The StandingChargeGas standard cost for October should be correct");
        Assert.AreEqual(standardCost2, standardCosts.Get(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas), "The StandingChargeGas standard cost for September should be correct");
    }

    [Test()]
    public void UpdateTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);

        standardCosts.AddOrUpdate(standardCost);

        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");
        Assert.AreEqual(5.99, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas).Price, "The StandingChargeGas standard cost should be correct");

        standardCosts.AddOrUpdate(standardCost2);
        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");
        Assert.AreEqual(6.99, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas).Price, "The StandingChargeGas standard cost should be correct");
    }

    [Test()]
    public void DeleteTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);

        standardCosts.AddOrUpdate(standardCost);
        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");

        standardCosts.Delete(standardCost);
        Assert.AreEqual(0, standardCosts.Get().Count, "There should be 0 standard cost");
    }

    [Test()]
    public void DeleteSpecificWhenMultipleCostsExistInSameMonthTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 0.99);

        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        Assert.AreEqual(2, standardCosts.Get().Count, "There should be 2 standard cost");

        standardCosts.Delete(standardCost);
        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");
        Assert.AreEqual(standardCost2, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity), "The TransportCostElectricity standard cost should be correct");
    }

    [Test()]
    public void DeleteWhenNoFixedCostsExistTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();

        Assert.Throws<StandardCostDoesNotExistException>(() =>standardCosts.Delete(new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 0.00)), "A StandardCostDoesNotExistException should have been thrown");
    }

    [Test()]
    public void DeleteNonExistingTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);

        standardCosts.AddOrUpdate(standardCost);
        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");

        Assert.Throws<StandardCostDoesNotExistException>(() => standardCosts.Delete(new StandardCost(new DateTime(2023, 10, 2), FixedCostType.StandingChargeGas, 0.00)), "A StandardCostDoesNotExistException should have been thrown");
        Assert.AreEqual(1, standardCosts.Get().Count, "There should be 1 standard cost");
    }

    [Test()]
    public void GetTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        StandardCost standardCost3 = new StandardCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        standardCosts.AddOrUpdate(standardCost3);

        Assert.AreEqual(3, standardCosts.Get().Count, "There should be 3 standard Costs");
    }


    [Test()]
    public void GetForSpecificDateTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        StandardCost standardCost3 = new StandardCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        standardCosts.AddOrUpdate(standardCost3);

        Assert.AreEqual(standardCost2, standardCosts.Get(new DateTime(2023, 10, 10), FixedCostType.StandingChargeGas), "The StandingChargeGAs should be correct");
    }

    [Test()]
    public void GetForSpecificTypesTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 6.99);
        StandardCost standardCost3 = new StandardCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        standardCosts.AddOrUpdate(standardCost3);

        Assert.AreEqual(standardCost, standardCosts.Get(new DateTime(2023, 10, 10), FixedCostType.StandingChargeGas), "The StandingChargeGAs should be correct");
        Assert.AreEqual(standardCost2, standardCosts.Get(new DateTime(2023, 10, 10), FixedCostType.TransportCostElectricity), "The TransportCostElectricity should be correct");
    }
        
    [Test()]
    public void GetForDateEdgeCaseTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        StandardCost standardCost3 = new StandardCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        standardCosts.AddOrUpdate(standardCost3);

        Assert.AreEqual(standardCost2, standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas), "The StandingChargeGAs should be correct");
    }

    [Test()]
    public void GetNonExistingForTypeTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();

        Assert.Throws<StandardCostNotFoundException>(() => standardCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetBelowLowestDateTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);


        standardCosts.AddOrUpdate(standardCost);

        Assert.Throws<StandardCostNotFoundException>(() => standardCosts.Get(new DateTime(2023, 8, 1), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetAboveLatestDateTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        StandardCost standardCost3 = new StandardCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        standardCosts.AddOrUpdate(standardCost3);

        Assert.AreEqual(standardCost3, standardCosts.Get(new DateTime(2023, 11, 2), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetForYearTest()
    {
        StandardCostCollection standardCosts = new StandardCostCollection();
        StandardCost standardCost = new StandardCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        StandardCost standardCost2 = new StandardCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        StandardCost standardCost3 = new StandardCost(new DateTime(2024, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        standardCosts.AddOrUpdate(standardCost);
        standardCosts.AddOrUpdate(standardCost2);
        standardCosts.AddOrUpdate(standardCost3);

        Assert.AreEqual(2, standardCosts.Get(2023, FixedCostType.StandingChargeGas).Count, "THere should be 2 standard costs in 2023");
    }

}