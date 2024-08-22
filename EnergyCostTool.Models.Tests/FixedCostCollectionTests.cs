using EnergyCostTool.Models;
using EnergyCostTool.Models.Exceptions;
using NUnit.Framework;

namespace EnergyCostTool.Models.Tests;

[TestFixture()]
public class FixedCostCollectionTests
{
    [Test()]
    public void AddFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
            
        fixedCosts.Add(fixedCost);

        Assert.AreEqual(1, fixedCosts.Get().Count);
        Assert.AreEqual(fixedCost, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void AddExistingFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);

        fixedCosts.Add(fixedCost);

        Assert.Throws<FixedCostExistsException>(() => fixedCosts.Add(fixedCost));
        Assert.AreEqual(1, fixedCosts.Get().Count);
    }

    [Test()]
    public void AddDifferentFixedCostForSameDateTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 0.99);

        fixedCosts.Add(fixedCost);
        fixedCosts.Add(fixedCost2);

        Assert.AreEqual(2, fixedCosts.Get().Count);
        Assert.AreEqual(fixedCost, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas));
        Assert.AreEqual(fixedCost2, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity));
    }


    [Test()]
    public void DeleteFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);

        fixedCosts.Add(fixedCost);
        Assert.AreEqual(1, fixedCosts.Get().Count);

        fixedCosts.Delete(fixedCost);
        Assert.AreEqual(0, fixedCosts.Get().Count);
    }

    [Test()]
    public void DeleteFixedCostWhenMoreSameDateFixedCostExistsTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 0.99);

        fixedCosts.Add(fixedCost);
        fixedCosts.Add(fixedCost2);
        Assert.AreEqual(2, fixedCosts.Get().Count);

        fixedCosts.Delete(fixedCost);
        Assert.AreEqual(1, fixedCosts.Get().Count);
        Assert.AreEqual(fixedCost2, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity));
    }

    [Test()]
    public void DeleteFixedCostFromEmptyListTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();

        Assert.Throws<FixedCostDoesNotExistException>(() =>fixedCosts.Delete(new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 0.00)));
    }

    [Test()]
    public void DeleteIncorrectFixedCostFromNonEmptyListTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);

        fixedCosts.Add(fixedCost);
        Assert.AreEqual(1, fixedCosts.Get().Count);

        Assert.Throws<FixedCostDoesNotExistException>(() => fixedCosts.Delete(new FixedCost(new DateTime(2023, 10, 2), FixedCostType.StandingChargeGas, 0.00)));
        Assert.AreEqual(1, fixedCosts.Get().Count);
    }

    [Test()]
    public void UpdateFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);

        fixedCosts.Add(fixedCost);

        Assert.AreEqual(1, fixedCosts.Get().Count);
        Assert.AreEqual(5.99, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas).Price);

        fixedCosts.Update(fixedCost2);
        Assert.AreEqual(1, fixedCosts.Get().Count);
        Assert.AreEqual(6.99, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas).Price);
    }

    [Test()]
    public void UpdateNonExistingFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 2), FixedCostType.StandingChargeGas, 6.99);

        fixedCosts.Add(fixedCost);
        Assert.AreEqual(1, fixedCosts.Get().Count);

        Assert.Throws<FixedCostDoesNotExistException>(() => fixedCosts.Update(fixedCost2));
        Assert.AreEqual(1, fixedCosts.Get().Count);
        Assert.AreEqual(5.99, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas).Price);
    }

    [Test()]
    public void UpdateSameDateDifferentFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 6.99);

        fixedCosts.Add(fixedCost);
        Assert.AreEqual(1, fixedCosts.Get().Count);

        Assert.Throws<FixedCostDoesNotExistException>(() => fixedCosts.Update(fixedCost2));
        Assert.AreEqual(1, fixedCosts.Get().Count);
        Assert.AreEqual(5.99, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas).Price);
    }


    [Test()]
    public void GetFixedCostTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        FixedCost fixedCost3 = new FixedCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        fixedCosts.Add(fixedCost);
        fixedCosts.Add(fixedCost2);
        fixedCosts.Add(fixedCost3);

        Assert.AreEqual(fixedCost2, fixedCosts.Get(new DateTime(2023, 10, 10), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetFixedCostForDifferentTypesTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.TransportCostElectricity, 6.99);
        FixedCost fixedCost3 = new FixedCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        fixedCosts.Add(fixedCost);
        fixedCosts.Add(fixedCost2);
        fixedCosts.Add(fixedCost3);

        Assert.AreEqual(fixedCost, fixedCosts.Get(new DateTime(2023, 10, 10), FixedCostType.StandingChargeGas));
        Assert.AreEqual(fixedCost2, fixedCosts.Get(new DateTime(2023, 10, 10), FixedCostType.TransportCostElectricity));
    }
        
    [Test()]
    public void GetFixedCostEdgeCaseTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        FixedCost fixedCost3 = new FixedCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        fixedCosts.Add(fixedCost);
        fixedCosts.Add(fixedCost2);
        fixedCosts.Add(fixedCost3);

        Assert.AreEqual(fixedCost2, fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetFixedCostNoPricesSetTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();

        Assert.Throws<FixedCostNotFoundException>(() => fixedCosts.Get(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetFixedCostOutOfLowerBoundTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);


        fixedCosts.Add(fixedCost);

        Assert.Throws<FixedCostNotFoundException>(() => fixedCosts.Get(new DateTime(2023, 8, 1), FixedCostType.StandingChargeGas));
    }

    [Test()]
    public void GetFixedCostOutOfUpperBoundTest()
    {
        FixedCostCollection fixedCosts = new FixedCostCollection();
        FixedCost fixedCost = new FixedCost(new DateTime(2023, 9, 1), FixedCostType.StandingChargeGas, 5.99);
        FixedCost fixedCost2 = new FixedCost(new DateTime(2023, 10, 1), FixedCostType.StandingChargeGas, 6.99);
        FixedCost fixedCost3 = new FixedCost(new DateTime(2023, 11, 1), FixedCostType.StandingChargeGas, 7.99);


        fixedCosts.Add(fixedCost);
        fixedCosts.Add(fixedCost2);
        fixedCosts.Add(fixedCost3);

        Assert.AreEqual(fixedCost3, fixedCosts.Get(new DateTime(2023, 11, 2), FixedCostType.StandingChargeGas));
    }
}