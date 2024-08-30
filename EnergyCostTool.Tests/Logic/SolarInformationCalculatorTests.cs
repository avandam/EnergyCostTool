using NUnit.Framework;
using EnergyCostTool.Models;
using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool.Logic.Tests
{
    [TestFixture()]
    public class SolarInformationCalculatorTests
    {
        [Test()]
        public void ComputeSolarInformationTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(500, 150, 150, 150, 50, 100);
            Tariff tariff = new Tariff(4.00, 3.00, 2.00, 1.00, 10.00);

            DateTime date2 = new DateTime(2024, 11, 1);
            Consumption consumption2 = new Consumption(1000, 300, 300, 300, 100, 200);
            Tariff tariff2 = new Tariff(8.00, 6.00, 4.00, 2.00, 20.00);

            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();
            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);
            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);
            energyMonthCollection.AddOrUpdateEnergyMonth(date2, consumption2);
            energyMonthCollection.AddOrUpdateEnergyMonth(date2, tariff2);

            StandardCostCollection standardCostCollection = new StandardCostCollection();
            standardCostCollection.AddOrUpdate(new StandardCost(new DateTime(2024, 11, 1), FixedCostType.SolarCost, 20.00));
            energyMonthCollection.InjectStandardCosts(standardCostCollection);

            SolarInformationViewModel solarInformationViewModel = SolarInformationCalculator.ComputeSolarInformation(energyMonthCollection);
            Assert.AreEqual(1500, solarInformationViewModel.Generation);
            Assert.AreEqual(2, solarInformationViewModel.NrOfMonthsWithPanels);
            Assert.AreEqual(-20.00, solarInformationViewModel.FixedCostPrice);
            Assert.AreEqual(900.00, solarInformationViewModel.DirectlyUsed);
            Assert.AreEqual(5250.00, solarInformationViewModel.DirectlyUsedPrice);
            Assert.AreEqual(450.00, solarInformationViewModel.ReturnedNorm);
            Assert.AreEqual(150.00, solarInformationViewModel.ReturnedLow);
            Assert.AreEqual(2250.00, solarInformationViewModel.ReturnedNormPrice);
            Assert.AreEqual(250.00, solarInformationViewModel.ReturnedLowPrice);

        }
    }
}