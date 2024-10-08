﻿using NUnit.Framework;
using EnergyCostTool.Models.Exceptions;
using EnergyCostTool.Models.Enumerations;

namespace EnergyCostTool.Models.Tests
{
    [TestFixture()]
    public class EnergyMonthCollectionTests
    {
        [Test()]
        public void AddOrUpdateConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "tariff should be null");
        }

        [Test()]
        public void AddOrUpdateConsumptionTwiceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "tariff should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "tariff should be null");
        }

        [Test()]
        public void AddOrUpdateConsumptionNewTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            Consumption consumption2 = new Consumption(110, 111, 112, 113, 114, 115);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "tariff should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption2);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption2, energyMonthCollection.Get()[0].Consumption, "Consumption should be updated");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "tariff should be null");
        }

        [Test()]
        public void AddOrUpdateConsumptionToExistingTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumtpion is incorrect");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
        }

        [Test()]
        public void AddOrUpdateTariffTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
        }

        [Test()]
        public void AddOrUpdateTariffTwiceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
        }

        [Test()]
        public void AddOrUpdateTariffNewTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);
            Tariff tariff2 = new Tariff(10.1, 10.2, 10.3, 10.4, 10.5);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff2);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(tariff2, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
        }

        [Test()]
        public void AddOrUpdateTariffToExistingTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.5, 0.5);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "tariff should be null");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumtpion is incorrect");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
        }

        [Test()]
        public void AddOrUpdateFixedPriceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.SolarCost, 1.00);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, fixedPrice);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(fixedPrice, energyMonthCollection.Get()[0].GetFixedPrices()[0], "Fixed Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");
        }

        [Test()]
        public void AddOrUpdateFixedPriceTwiceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.SolarCost, 1.00);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();


            energyMonthCollection.AddOrUpdateEnergyMonth(date, fixedPrice);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(fixedPrice, energyMonthCollection.Get()[0].GetFixedPrices()[0], "Fixed Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, fixedPrice);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(fixedPrice, energyMonthCollection.Get()[0].GetFixedPrices()[0], "Fixed Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");
        }

        [Test()]
        public void AddOrUpdateFixedPriceNewTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.SolarCost, 1.00);
            FixedPrice fixedPrice2 = new FixedPrice(FixedCostType.TransportCostGas, 2.00);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, fixedPrice);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(fixedPrice, energyMonthCollection.Get()[0].GetFixedPrices()[0], "Fixed Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, fixedPrice2);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(fixedPrice, energyMonthCollection.Get()[0].GetFixedPrices()[0], "Fixed Price is incorrect");
            Assert.AreEqual(fixedPrice2, energyMonthCollection.Get()[0].GetFixedPrices()[1], "Fixed Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");
        }

        [Test()]
        public void AddOrUpdateFixedPriceToExistingTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.SolarCost, 1.00);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(0, energyMonthCollection.Get()[0].GetFixedPrices().Count, "Fixed Price is incorrect");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, fixedPrice);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(fixedPrice, energyMonthCollection.Get()[0].GetFixedPrices()[0], "Fixed Price is incorrect");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Tariff, "Tariff should be null");
        }

        [Test()]
        public void AddTariffTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);
            Consumption consumption = new Consumption(100, 200, 300, 400, 500, 600);

            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();
            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);
            energyMonthCollection.AddTariff(date, tariff);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(tariff, energyMonthCollection.Get()[0].Tariff, "tariff is incorrect");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
        }

        [Test()]
        public void AddTariffNoConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);

            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();
            energyMonthCollection.AddTariff(date, tariff);

            Assert.AreEqual(0, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
        }

        [Test()]
        public void DeleteEnergyMonthNonExistingTest()
        {
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            Assert.Throws<EnergyMonthNotFoundException>(() => energyMonthCollection.DeleteEnergyMonth(new DateTime(2024, 10, 1)));
        }

        [Test()]
        public void DeleteEnergyMonthTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");

            energyMonthCollection.DeleteEnergyMonth(date);
            Assert.AreEqual(0, energyMonthCollection.Get().Count, "There should be no EnergyMonths in the collection");
        }

        [Test()]
        public void ContainsDataForTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.IsTrue(energyMonthCollection.ContainsDataFor(new DateTime(2024, 10, 1)), "EnergyMonth should have been found");
        }

        [Test()]
        public void ContainsDataForNonExistingDateTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.IsFalse(energyMonthCollection.ContainsDataFor(new DateTime(2024, 11, 1)), "EnergyMonth should not have been found");
        }

        [Test()]
        public void GetByDateTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.DoesNotThrow(() => energyMonthCollection.Get(new DateTime(2024, 10, 1)), "EnergyMonth should have been found");
        }

        [Test()]
        public void GetByDateNonExistingDateTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.Throws<EnergyMonthNotFoundException>(() => energyMonthCollection.Get(new DateTime(2024, 11, 1)), "EnergyMonth should not have been found");
        }

        [Test()]
        public void GetByDateEmptyCollectionTest()
        {
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            Assert.Throws<EnergyMonthNotFoundException>(() => energyMonthCollection.Get(new DateTime(2024, 11, 1)), "EnergyMonth should not have been found");
        }

        [Test()]
        public void InjectStandardCostTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(100, 200, 300, 400, 500, 600);

            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();
            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            StandardCostCollection standardCostCollection = new StandardCostCollection();
            standardCostCollection.AddOrUpdate(new StandardCost(new DateTime(2024, 9, 1), FixedCostType.StandingChargeElectricity, 1.00));
            standardCostCollection.AddOrUpdate(new StandardCost(new DateTime(2024, 11, 1), FixedCostType.TransportCostElectricity, 2.00));
            standardCostCollection.AddOrUpdate(new StandardCost(new DateTime(2024, 10, 1), FixedCostType.TransportCostGas, 3.00));

            energyMonthCollection.InjectStandardCosts(standardCostCollection);

            Assert.AreEqual(1.00, energyMonthCollection.Get(date).GetFixedPrice(FixedCostType.StandingChargeElectricity), "The standingChargeElectricity should be Set correctly");
            Assert.AreEqual(0.00, energyMonthCollection.Get(date).GetFixedPrice(FixedCostType.TransportCostElectricity), "TransportCostelectritiy should not be injected, thus should be 0.0");
            Assert.AreEqual(93.00, energyMonthCollection.Get(date).GetFixedPrice(FixedCostType.TransportCostGas), "The TransportCostGas should be Set correctly");
        }

    }
}