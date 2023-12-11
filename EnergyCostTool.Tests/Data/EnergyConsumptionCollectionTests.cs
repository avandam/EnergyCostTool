using EnergyCostTool.Data;
using EnergyCostTool.Exceptions;
using NUnit.Framework;

namespace EnergyCostTool.Tests.Data
{
    [TestFixture()]
    public class EnergyConsumptionCollectionTests
    {
        [Test()]
        public void AddEnergyConsumptionTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1),500, 250, 150, 300, 50, 5);
            
            energyConsumptions.Add(energyConsumption);

            Assert.AreEqual(1, energyConsumptions.Count());
            Assert.AreEqual(energyConsumption, energyConsumptions.Get(new DateTime(2023, 10, 1)));
        }

        [Test()]
        public void AddExistingEnergyConsumptionTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);

            energyConsumptions.Add(energyConsumption);

            Assert.Throws<EnergyConsumptionExistsException>(() => energyConsumptions.Add(energyConsumption));
            Assert.AreEqual(1, energyConsumptions.Count());
        }

        [Test()]
        public void DeleteEnergyConsumptionTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);

            energyConsumptions.Add(energyConsumption);
            Assert.AreEqual(1, energyConsumptions.Count());

            energyConsumptions.Delete(energyConsumption.Month);
            Assert.AreEqual(0, energyConsumptions.Count());
        }

        [Test()]
        public void DeleteEnergyConsumptionFromEmptyListTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);

            Assert.Throws<EnergyConsumptionDoesNotExistException>(() =>energyConsumptions.Delete(energyConsumption.Month));
        }

        [Test()]
        public void DeleteIncorrectEnergyConsumptionFromNonEmptyListTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption2 = new EnergyConsumption(new DateTime(2023, 11, 1), 500, 250, 150, 300, 50, 5);

            energyConsumptions.Add(energyConsumption);
            Assert.AreEqual(1, energyConsumptions.Count());

            Assert.Throws<EnergyConsumptionDoesNotExistException>(() => energyConsumptions.Delete(energyConsumption2.Month));
            Assert.AreEqual(1, energyConsumptions.Count());
        }

        [Test()]
        public void UpdateEnergyConsumptionTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption2 = new EnergyConsumption(new DateTime(2023, 10, 1), 600, 250, 150, 300, 50, 5);

            energyConsumptions.Add(energyConsumption);

            Assert.AreEqual(1, energyConsumptions.Count());
            Assert.AreEqual(500, energyConsumptions.Get(new DateTime(2023, 10, 1)).SolarGeneration);

            energyConsumptions.Update(energyConsumption2);
            Assert.AreEqual(1, energyConsumptions.Count());
            Assert.AreEqual(600, energyConsumptions.Get(new DateTime(2023, 10, 1)).SolarGeneration);
        }

        [Test()]
        public void UpdateNonExistingEnergyConsumptionTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption2 = new EnergyConsumption(new DateTime(2023, 11, 1), 600, 250, 150, 300, 50, 5);

            energyConsumptions.Add(energyConsumption);
            Assert.AreEqual(1, energyConsumptions.Count());

            Assert.Throws<EnergyConsumptionDoesNotExistException>(() => energyConsumptions.Update(energyConsumption2));
            Assert.AreEqual(1, energyConsumptions.Count());
            Assert.AreEqual(500, energyConsumptions.Get(new DateTime(2023, 10, 1)).SolarGeneration);

        }

        [Test()]
        public void GetEnergyConsumptionTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption2 = new EnergyConsumption(new DateTime(2023, 11, 1), 600, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption3 = new EnergyConsumption(new DateTime(2023, 12, 1), 700, 250, 150, 300, 50, 5);


            energyConsumptions.Add(energyConsumption);
            energyConsumptions.Add(energyConsumption2);
            energyConsumptions.Add(energyConsumption3);

            Assert.AreEqual(energyConsumption2, energyConsumptions.Get(new DateTime(2023, 11, 1)));
        }


        [Test()]
        public void GetEnergyConsumptionNoConsumptionsSetTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();

            Assert.Throws<EnergyConsumptionNotFoundException>(() => energyConsumptions.Get(new DateTime(2023, 10, 1)));
        }


        [Test()]
        public void FileHandlingTest()
        {
            EnergyConsumptionCollection energyConsumptions = new EnergyConsumptionCollection();
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(2023, 10, 1), 500, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption2 = new EnergyConsumption(new DateTime(2023, 11, 1), 600, 250, 150, 300, 50, 5);
            EnergyConsumption energyConsumption3 = new EnergyConsumption(new DateTime(2023, 12, 1), 700, 250, 150, 300, 50, 5);


            energyConsumptions.Add(energyConsumption);
            energyConsumptions.Add(energyConsumption2);
            energyConsumptions.Add(energyConsumption3);

            energyConsumptions.Save();
            try
            {
                Assert.IsTrue(File.Exists("energyConsumptions.dat"));
                EnergyConsumptionCollection energyConsumptions2 = new EnergyConsumptionCollection();
                Assert.AreEqual(3, energyConsumptions2.Count());
                Assert.AreEqual(500, energyConsumptions2.Get(new DateTime(2023, 10, 1)).SolarGeneration);
            }
            finally
            {
                if (File.Exists("energyConsumptions.dat"))
                {
                    File.Delete("energyConsumptions.dat");
                }
            }
        }

    }
}