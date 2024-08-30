using NUnit.Framework;
using EnergyCostTool.Models.Exceptions;
using EnergyCostTool.Models.Enumerations;
using System.Security.Authentication;

namespace EnergyCostTool.Models.Tests
{
    [TestFixture()]
    public class EnergyMonthTests
    {
        [Test()]
        public void EnergyMonthCreateWithIncorrectDateTest()
        {
            DateTime date = new DateTime(2024, 10, 2);

            Assert.Throws<EnergyMonthException>(() => new EnergyMonth(date), "A date not starting at the first should give an exception");
        }

        [Test()]
        public void EnergyMonthCreateWithConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);

            EnergyMonth energyMonth = new EnergyMonth(date, consumption);
            Assert.IsNotNull(energyMonth.Consumption, "Concumption should be filled in");
            Assert.IsNull(energyMonth.Tariff, "Tariff should not be filled in");
            Assert.AreEqual(0, energyMonth.GetFixedPrices().Count, "There should be no fixed prices");
        }

        [Test()]
        public void EnergyMonthCreateWithTariffTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);

            EnergyMonth energyMonth = new EnergyMonth(date, tariff);
            Assert.IsNotNull(energyMonth.Tariff, "tariff should be filled in");
            Assert.IsNull(energyMonth.Consumption, "Cosnumption should not be filled in");
            Assert.AreEqual(0, energyMonth.GetFixedPrices().Count, "There should be no fixed prices");
        }

        [Test()]
        public void EnergyMonthCreateWithFixedCostTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.StandingChargeElectricity, 1.00);

            EnergyMonth energyMonth = new EnergyMonth(date, fixedPrice);
            Assert.IsNull(energyMonth.Consumption, "Concumption should not be filled in");
            Assert.IsNull(energyMonth.Tariff, "Tariff should not be filled in");
            Assert.AreEqual(1, energyMonth.GetFixedPrices().Count, "There should be a fixed price");
        }

        [Test()]
        public void TariffAddTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);

            EnergyMonth energyMonth = new EnergyMonth(date);
            Assert.IsNull(energyMonth.Tariff, "tariff should not be filled in");

            energyMonth.AddOrUpdate(tariff);
            Assert.AreEqual(tariff, energyMonth.Tariff, "Tariff should be added correctly.");
        }

        [Test()]
        public void TariffUpdateTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Tariff tariff = new Tariff(0.1, 0.2, 0.3, 0.4, 0.5);
            Tariff tariff2 = new Tariff(10.1, 0.2, 0.3, 0.4, 0.5);

            EnergyMonth energyMonth = new EnergyMonth(date, tariff);
            Assert.AreEqual(tariff, energyMonth.Tariff, "tariff should be filled in correctly");

            energyMonth.AddOrUpdate(tariff2);
            Assert.AreEqual(tariff2, energyMonth.Tariff, "Tariff should be updated correctly.");
        }

        [Test()]
        public void ConsumptionAddTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(100, 200, 300, 400, 500, 600);

            EnergyMonth energyMonth = new EnergyMonth(date);
            Assert.IsNull(energyMonth.Consumption, "Consumption should not be filled in");

            energyMonth.AddOrUpdate(consumption);
            Assert.AreEqual(consumption, energyMonth.Consumption, "Consumption should be added correctly.");
        }

        [Test()]
        public void ConsumptionUpdateTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(100, 200, 300, 400, 500, 600);
            Consumption consumption2 = new Consumption(1100, 200, 300, 400, 500, 600);

            EnergyMonth energyMonth = new EnergyMonth(date, consumption);
            Assert.AreEqual(consumption, energyMonth.Consumption, "Consumption should be filled in correctly");

            energyMonth.AddOrUpdate(consumption2);
            Assert.AreEqual(consumption2, energyMonth.Consumption, "Consumption should be updated correctly.");
        }

        [Test()]
        public void FixedPriceAddTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.TransportCostElectricity, 1.00);

            EnergyMonth energyMonth = new EnergyMonth(date);
            Assert.AreEqual(0, energyMonth.GetFixedPrices().Count, "Fixed prices should be empty");

            energyMonth.AddOrUpdate(fixedPrice);
            Assert.AreEqual(fixedPrice, energyMonth.GetFixedPrices()[0], "Fixed Price should be added correctly.");
        }



        [Test()]
        public void FixedPriceUpdateTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.TransportCostElectricity, 1.00);
            FixedPrice fixedPrice2 = new FixedPrice(FixedCostType.TransportCostElectricity, 2.00);

            EnergyMonth energyMonth = new EnergyMonth(date, fixedPrice);
            Assert.AreEqual(fixedPrice, energyMonth.GetFixedPrices()[0], "Fixed Price should be filled in correctly");

            energyMonth.AddOrUpdate(fixedPrice2);
            Assert.AreEqual(fixedPrice2, energyMonth.GetFixedPrices()[0], "Fixed Price should be updated correctly.");
        }

        [Test()]
        public void FixedPriceAddTwoDifferentFixedPricesTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.TransportCostElectricity, 1.00);
            FixedPrice fixedPrice2 = new FixedPrice(FixedCostType.StandingChargeElectricity, 2.00);

            EnergyMonth energyMonth = new EnergyMonth(date, fixedPrice);
            Assert.AreEqual(fixedPrice, energyMonth.GetFixedPrices()[0], "Fixed Price should be filled in correctly");

            energyMonth.AddOrUpdate(fixedPrice2);
            Assert.AreEqual(fixedPrice2, energyMonth.GetFixedPrices()[1], "Fixed Price should be updated correctly.");
        }

        [Test()]
        public void FixedPriceDeleteTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.TransportCostElectricity, 1.00);

            EnergyMonth energyMonth = new EnergyMonth(date, fixedPrice);
            Assert.AreEqual(1, energyMonth.GetFixedPrices().Count, "There should be one Fixed price");

            energyMonth.DeleteFixedPrice(fixedPrice);
            Assert.AreEqual(0, energyMonth.GetFixedPrices().Count, "Fixed price should be deleted correctly.");
        }

        [Test()]
        public void GetFixedPriceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            FixedPrice fixedPrice = new FixedPrice(FixedCostType.TransportCostElectricity, 1.00);
            FixedPrice fixedPrice2 = new FixedPrice(FixedCostType.TransportCostGas, 2.00);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(fixedPrice);
            energyMonth.AddOrUpdate(fixedPrice2);
            Assert.AreEqual(1.00, energyMonth.GetFixedPrice(FixedCostType.TransportCostElectricity), "The transport cost electricity should be correctly found");
            Assert.AreEqual(2.00, energyMonth.GetFixedPrice(FixedCostType.TransportCostGas), "The transport cost gas should be correctly found");
            Assert.AreEqual(0.0, energyMonth.GetFixedPrice(FixedCostType.StandingChargeGas), "The standing charge gas is not filled in, so 0.0 should be returned");
        }

        [Test()]
        public void GetDirectlyUsedPrice()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(500, 200, 150, 200, 50, 0);
            Tariff tariff = new Tariff(2.00, 3.00, 1.00, 4.00, 5.00);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(consumption);
            energyMonth.AddOrUpdate(tariff);

            Assert.AreEqual(450.00, energyMonth.GetDirectlyUsedPrice(), "The Directly used price should be correctly computed");
        }

        [Test()]
        public void GetDirectlyUsedLowPrice()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(500, 200, 150, 200, 50, 0);
            Tariff tariff = new Tariff(2.00, 3.00, 1.00, 4.00, 5.00);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(consumption);
            energyMonth.AddOrUpdate(tariff);

            Assert.AreEqual(75.00, energyMonth.GetDirectlyUsedLowPrice(), "The Directly used price should be correctly computed");
        }

        [Test()]
        public void GetTotalPriceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(81.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceNoConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(6.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceNoTariffTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(6.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceNoFixedCostTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5));

            Assert.AreEqual(75.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotaMultipleFixedPriceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.StandingChargeElectricity, 7.00));

            Assert.AreEqual(88.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceElectricityHighBelowCapTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.6, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(81.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceElectricityHighAboveCapTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.4, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(71.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceElectricityLowBelowCapTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.6, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(81.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceElectricityLowAboveCapTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.3, 1.5));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(41.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceGasBelowCapTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.6, 2.0));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(81.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        [Test()]
        public void GetTotalPriceGasAboveCapTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.6, 1.0));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.AreEqual(76.00, energyMonth.GetTotalPrice(), "Total price is incorrect");
        }

        public void DeleteConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.6, 1.0));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.IsNotNull(energyMonth.Consumption, "Consumption should be filled in");

            energyMonth.DeleteConsumption();

            Assert.IsNotNull(energyMonth.Consumption, "Consumption should not be filled in");
        }

        public void DeleteTariffTest()
        {
            DateTime date = new DateTime(2024, 10, 1);

            EnergyMonth energyMonth = new EnergyMonth(date);
            energyMonth.AddOrUpdate(new Consumption(500, 100, 300, 200, 100, 10));
            energyMonth.AddOrUpdate(new Tariff(0.5, 0.2, 0.4, 0.1, 1.5, 0.6, 1.0));
            energyMonth.AddOrUpdate(new FixedPrice(FixedCostType.TransportCostElectricity, 6.00));

            Assert.IsNotNull(energyMonth.Tariff, "Tariff should be filled in");

            energyMonth.DeleteTariff();

            Assert.IsNotNull(energyMonth.Tariff, "Tariff should not be filled in");
        }

    }
}