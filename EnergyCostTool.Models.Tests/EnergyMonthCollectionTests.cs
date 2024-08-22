using NUnit.Framework;
using EnergyCostTool.Models.Exceptions;

namespace EnergyCostTool.Models.Tests
{
    [TestFixture()]
    public class EnergyMonthCollectionTests
    {
        [Test()]
        public void AddOrUpdateEnergyMonthConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Price, "Price should be null");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthConsumptionTwiceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Price, "Price should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Price, "Price should be null");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthNewConsumptionTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            Consumption consumption2 = new Consumption(110, 111, 112, 113, 114, 115);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Price, "Price should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption2);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption2, energyMonthCollection.Get()[0].Consumption, "Consumption should be updated");
            Assert.IsNull(energyMonthCollection.Get()[0].Price, "Price should be null");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthConsumptionToExistingTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Price price = new Price(0.1, 0.2, 0.3, 0.4, 0.5);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "price is incorrect");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumtpion is incorrect");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "price is incorrect");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthPriceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Price price = new Price(0.1, 0.2, 0.3, 0.4, 0.5);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthPriceTwiceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Price price = new Price(0.1, 0.2, 0.3, 0.4, 0.5);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthNewPriceTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Price price = new Price(0.1, 0.2, 0.3, 0.4, 0.5);
            Price price2 = new Price(10.1, 10.2, 10.3, 10.4, 10.5);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price2);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(price2, energyMonthCollection.Get()[0].Price, "Price is incorrect");
            Assert.IsNull(energyMonthCollection.Get()[0].Consumption, "Consumption should be null");
        }

        [Test()]
        public void AddOrUpdateEnergyMonthPriceToExistingTest()
        {
            DateTime date = new DateTime(2024, 10, 1);
            Price price = new Price(0.1, 0.2, 0.3, 0.5, 0.5);
            Consumption consumption = new Consumption(10, 11, 12, 13, 14, 15);
            EnergyMonthCollection energyMonthCollection = new EnergyMonthCollection();

            energyMonthCollection.AddOrUpdateEnergyMonth(date, consumption);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.IsNull(energyMonthCollection.Get()[0].Price, "Price should be null");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumption is incorrect");

            energyMonthCollection.AddOrUpdateEnergyMonth(date, price);

            Assert.AreEqual(1, energyMonthCollection.Get().Count, "There should be one EnergyMonth in the collection");
            Assert.AreEqual(consumption, energyMonthCollection.Get()[0].Consumption, "Consumtpion is incorrect");
            Assert.AreEqual(price, energyMonthCollection.Get()[0].Price, "price is incorrect");
        }

        [Test()]
        public void DeleteNonExistingEnergyMonthTest()
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
    }
}