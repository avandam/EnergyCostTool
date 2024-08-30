using NUnit.Framework;
using EnergyCostTool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyCostTool.Models;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool.Logic.Tests
{
    [TestFixture()]
    public class YearlyCostCalculatorTests
    {
        [Test()]
        public void YearlyCostCalculatorTest()
        {
            DateTime date = new DateTime(2024, 1, 1);
            Consumption consumption = new Consumption(500, 100, 150, 200, 50, 90);
            Tariff tariff = new Tariff(0.5, 0.05, 0.4, 0.04, 1.12);

            EnergyMonthCollection energyMonths = new EnergyMonthCollection();
            energyMonths.AddOrUpdateEnergyMonth(date, consumption);
            energyMonths.AddOrUpdateEnergyMonth(date, tariff);

            StandardCostCollection standardCostCollection = new StandardCostCollection();
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.StandingChargeElectricity, 5.99));
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.StandingChargeGas, 5.99));
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.TransportCostElectricity, 0.1));
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.TransportCostGas, 0.2));
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.MonthlyDeposit, 100));
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.SolarCost, 20));
            standardCostCollection.AddOrUpdate(new StandardCost(date, Models.Enumerations.FixedCostType.DiscountOnEnergyTax, -0.3));

            energyMonths.InjectStandardCosts(standardCostCollection);

            YearlyCostViewModel yearlyCostViewModel = YearlyCostCalculator.GetYearlyCostForYear(2024, energyMonths);

            Assert.AreEqual(100, yearlyCostViewModel.NormUsed, "NormUsed is incorrect");
            Assert.AreEqual(150, yearlyCostViewModel.NormReturned, "NormReturned is incorrect");
            Assert.AreEqual(200, yearlyCostViewModel.LowUsed, "LowUsed is incorrect");
            Assert.AreEqual(50, yearlyCostViewModel.LowReturned, "LowReturned is incorrect");
            Assert.AreEqual(90, yearlyCostViewModel.GasUsed, "GasUsed is incorrect");
            Assert.AreEqual(500, yearlyCostViewModel.SolarGenerated, "SolarGenerated is incorrect");

            Assert.AreEqual(-2.5, yearlyCostViewModel.NormPrice, "NormPrice is incorrect");
            Assert.AreEqual(60, yearlyCostViewModel.LowPrice, "LowPrice is incorrect");
            Assert.AreEqual(100.8, yearlyCostViewModel.GasPrice, "GasPrice is incorrect");

            Assert.AreEqual(5.99, yearlyCostViewModel.StandingChargesElectricity, "StandingChargesElectricity is incorrect");
            Assert.AreEqual(5.99, yearlyCostViewModel.StandingChargesGas, "StandingChargesGas is incorrect");
            Assert.AreEqual(3.1, yearlyCostViewModel.TransportCostElectricity, "TransportCostElectricity is incorrect");
            Assert.AreEqual(6.2, yearlyCostViewModel.TransportCostGas, "TransportCostGas is incorrect");
            Assert.AreEqual(-9.3, yearlyCostViewModel.DiscountOnEnergyTax, "DiscountOnEnergyTax is incorrect");
            Assert.AreEqual(100, yearlyCostViewModel.PayedDeposits, "PayedDeposits is incorrect");
            Assert.AreEqual(20, yearlyCostViewModel.SolarCost, "SolarCost is incorrect");
        }
    }
}