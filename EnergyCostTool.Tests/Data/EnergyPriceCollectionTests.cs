using EnergyCostTool.Data;
using EnergyCostTool.Exceptions;
using NUnit.Framework;

namespace EnergyCostTool.Tests.Data
{
    [TestFixture()]
    public class EnergyPriceCollectionTests
    {
        [Test()]
        public void AddEnergyPriceTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            
            energyPrices.Add(energyPrice);

            Assert.AreEqual(1, energyPrices.Count());
            Assert.AreEqual(energyPrice, energyPrices.Get(new DateTime(2023, 10, 1)));
        }

        [Test()]
        public void AddExistingEnergyPriceTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);

            energyPrices.Add(energyPrice);

            Assert.Throws<EnergyPriceExistsException>(() => energyPrices.Add(energyPrice));
            Assert.AreEqual(1, energyPrices.Count());
        }

        [Test()]
        public void DeleteEnergyPriceTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);

            energyPrices.Add(energyPrice);
            Assert.AreEqual(1, energyPrices.Count());

            energyPrices.Delete(energyPrice);
            Assert.AreEqual(0, energyPrices.Count());
        }

        [Test()]
        public void DeleteEnergyPriceFromEmptyListTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);

            Assert.Throws<EnergyPriceDoesNotExistException>(() =>energyPrices.Delete(energyPrice));
        }

        [Test()]
        public void DeleteIncorrectEnergyPriceFromNonEmptyListTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 2), 1.00, 1.01, 1.02, 1.03, 1.04);

            energyPrices.Add(energyPrice);
            Assert.AreEqual(1, energyPrices.Count());

            Assert.Throws<EnergyPriceDoesNotExistException>(() => energyPrices.Delete(energyPrice2));
            Assert.AreEqual(1, energyPrices.Count());
        }

        [Test()]
        public void UpdateEnergyPriceTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 1), 2.00, 2.01, 2.02, 2.03, 2.04);

            energyPrices.Add(energyPrice);

            Assert.AreEqual(1, energyPrices.Count());
            Assert.AreEqual(1.00, energyPrices.Get(new DateTime(2023, 10, 1)).ElectricityHigh);

            energyPrices.Update(energyPrice2);
            Assert.AreEqual(1, energyPrices.Count());
            Assert.AreEqual(2.00, energyPrices.Get(new DateTime(2023, 10, 1)).ElectricityHigh);
        }

        [Test()]
        public void UpdateNonExistingEnergyPriceTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 2), 2.00, 2.01, 2.02, 2.03, 2.04);

            energyPrices.Add(energyPrice);
            Assert.AreEqual(1, energyPrices.Count());

            Assert.Throws<EnergyPriceDoesNotExistException>(() => energyPrices.Update(energyPrice2));
            Assert.AreEqual(1, energyPrices.Count());
            Assert.AreEqual(1.00, energyPrices.Get(new DateTime(2023, 10, 1)).ElectricityHigh);

        }

        [Test()]
        public void GetEnergyPriceTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 9, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice3 = new EnergyPrice(new DateTime(2023, 11, 1), 1.00, 1.01, 1.02, 1.03, 1.04);


            energyPrices.Add(energyPrice);
            energyPrices.Add(energyPrice2);
            energyPrices.Add(energyPrice3);

            Assert.AreEqual(energyPrice2, energyPrices.Get(new DateTime(2023, 10, 10)));
        }


        [Test()]
        public void GetEnergyPriceEdgeCaseTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 9, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice3 = new EnergyPrice(new DateTime(2023, 11, 1), 1.00, 1.01, 1.02, 1.03, 1.04);


            energyPrices.Add(energyPrice);
            energyPrices.Add(energyPrice2);
            energyPrices.Add(energyPrice3);

            Assert.AreEqual(energyPrice2, energyPrices.Get(new DateTime(2023, 10, 1)));
        }

        [Test()]
        public void GetEnergyPriceNoPricesSetTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();

            Assert.Throws<EnergyPriceNotFoundException>(() => energyPrices.Get(new DateTime(2023, 10, 1)));
        }

        [Test()]
        public void GetEnergyPriceOutOfLowerBoundTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 9, 1), 1.00, 1.01, 1.02, 1.03, 1.04);


            energyPrices.Add(energyPrice);

            Assert.Throws<EnergyPriceNotFoundException>(() => energyPrices.Get(new DateTime(2023, 8, 1)));
        }

        [Test()]
        public void GetEnergyPriceOutOfUpperBoundTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 9, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice3 = new EnergyPrice(new DateTime(2023, 11, 1), 1.00, 1.01, 1.02, 1.03, 1.04);


            energyPrices.Add(energyPrice);
            energyPrices.Add(energyPrice2);
            energyPrices.Add(energyPrice3);

            Assert.AreEqual(energyPrice3, energyPrices.Get(new DateTime(2023, 11, 2)));
        }

        [Test()]
        public void FileHandlingTest()
        {
            EnergyPriceCollection energyPrices = new EnergyPriceCollection();
            EnergyPrice energyPrice = new EnergyPrice(new DateTime(2023, 9, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice2 = new EnergyPrice(new DateTime(2023, 10, 1), 1.00, 1.01, 1.02, 1.03, 1.04);
            EnergyPrice energyPrice3 = new EnergyPrice(new DateTime(2023, 11, 1), 1.00, 1.01, 1.02, 1.03, 1.04);


            energyPrices.Add(energyPrice);
            energyPrices.Add(energyPrice2);
            energyPrices.Add(energyPrice3);

            energyPrices.Save();
            try
            {
                Assert.IsTrue(File.Exists("energyPrices.dat"));
                EnergyPriceCollection energyPrices2 = new EnergyPriceCollection();
                Assert.AreEqual(3, energyPrices2.Count());
            }
            finally
            {
                if (File.Exists("energyPrices.dat"))
                {
                    File.Delete("energyPrices.dat");
                }
            }
        }

    }
}