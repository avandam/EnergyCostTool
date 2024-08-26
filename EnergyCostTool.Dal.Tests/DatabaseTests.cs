using NUnit.Framework;
using EnergyCostTool.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyCostTool.Models;
using EnergyCostTool.Dal.DataModels;

namespace EnergyCostTool.Dal.Tests
{
    [TestFixture()]
    public class DatabaseTests
    {
        private const string pricesFilename = "energyPrices.dat";
        private const string consumptionsFilename = "energyConsumptions.dat";

        [Test()]
        public void ConvertToEnergyMonthCollectionTest()
        {
            List<EnergyConsumption> energyConsumptions =
            [
                new EnergyConsumption(new DateTime(2024, 10, 1), 100, 200, 300, 400, 500, 600),
                new EnergyConsumption(new DateTime(2024, 11, 1), 1100, 1200, 1300, 1400, 1500, 1600),
            ];

            List<EnergyPrice> energyPrices =
            [
                new EnergyPrice(new DateTime(2024, 10, 1), 0.1, 0.2, 0.3, 0.4, 0.5),
                new EnergyPrice(new DateTime(2024, 12, 1), 10.1, 10.2, 10.3, 10.4, 10.5),
            ];

            EnergyMonthCollection energyMonths = Database.ConvertToEnergyMonthCollection(energyConsumptions, energyPrices);

            Assert.AreEqual(3, energyMonths.Get().Count, "Number of EnergyMonths should be 3");
            Assert.AreEqual(100, energyMonths.Get()[0].Consumption.SolarGeneration, "First Solar Generation should be 100");
            Assert.AreEqual(0.1, energyMonths.Get()[0].Tariff.ElectricityHigh, "First electricity high should be 0.1");
            Assert.AreEqual(1100, energyMonths.Get()[1].Consumption.SolarGeneration, "Second Solar Generation should be 1100");
            Assert.IsNull(energyMonths.Get()[1].Tariff, "Second Price should not exist");
            Assert.IsNull(energyMonths.Get()[2].Consumption, "Third Consumption should not exist");
            Assert.AreEqual(10.1, energyMonths.Get()[2].Tariff.ElectricityHigh, "Third electricity high should be 10.1");
        }

        [Test()]
        public void ConvertFromEnergyMonthCollectionTest()
        {
            EnergyMonthCollection energyMonths = new EnergyMonthCollection();
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 10, 1), new Consumption(100, 200, 300, 400, 500, 600));
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 11, 1), new Consumption(1100, 1200, 1300, 1400, 1500, 1600));
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 10, 1), new Tariff(0.1, 0.2, 0.3, 0.4, 0.5));
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 12, 1), new Tariff(10.1, 10.2, 10.3, 10.4, 10.5));

            (List<EnergyConsumption> consumptions, List<EnergyPrice> prices) energyInformation = Database.ConvertFromEnergyMonthCollection(energyMonths);
            
            List<EnergyConsumption> energyConsumptions = energyInformation.consumptions;
            Assert.AreEqual(2, energyConsumptions.Count, "Number of Consumptions should be 2");
            Assert.AreEqual(100, energyConsumptions[0].SolarGeneration, "First Solar should be 100");
            Assert.AreEqual(1100, energyConsumptions[1].SolarGeneration, "Second Solar should be 1100");

            List<EnergyPrice> energyPrices = energyInformation.prices;
            Assert.AreEqual(2, energyPrices.Count, "Number of Prices should be 2");
            Assert.AreEqual(0.1, energyPrices[0].ElectricityHigh, "First electricity high should be 0.1");
            Assert.AreEqual(10.1, energyPrices[1].ElectricityHigh, "Second electricity high should be 10.1");

        }

        [Test()]
        public void EnergyMonthIntegrationTest()
        {
            EnergyMonthCollection energyMonths = new EnergyMonthCollection();
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 10, 1), new Consumption(100, 200, 300, 400, 500, 600));
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 11, 1), new Consumption(1100, 1200, 1300, 1400, 1500, 1600));
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 10, 1), new Tariff(0.1, 0.2, 0.3, 0.4, 0.5));
            energyMonths.AddOrUpdateEnergyMonth(new DateTime(2024, 12, 1), new Tariff(10.1, 10.2, 10.3, 10.4, 10.5));

            try
            {
                Database.SaveEnergyMonths(energyMonths);
                Assert.IsTrue(File.Exists(pricesFilename), $"The file {pricesFilename} should exist after saving");
                Assert.IsTrue(File.Exists(consumptionsFilename), $"The file {consumptionsFilename} should exist after saving");
                EnergyMonthCollection expected = Database.GetEnergyMonths();
                Assert.AreEqual(3, expected.Get().Count, "There should be 3 energy Months retrieved");
            }
            catch
            {
                Assert.Fail("Save and load of data should work well");
            }
            finally
            {
                if (File.Exists(pricesFilename))
                {
                    File.Delete(pricesFilename);
                }
                if (File.Exists(consumptionsFilename))
                {
                    File.Delete(consumptionsFilename);
                }
            }
        }

    }
}
