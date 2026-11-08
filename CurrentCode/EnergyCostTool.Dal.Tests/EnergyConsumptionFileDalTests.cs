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
    public class EnergyConsumptionFileDalTests
    {
        private const string filename = "energyConsumptions.dat";

        [Test()]
        public void FileHandlingTest()
        {
            List<EnergyConsumption> energyConsumptions =
            [
                new EnergyConsumption(new DateTime(2024, 10, 1), 100, 200, 300, 400, 500, 600),
                new EnergyConsumption(new DateTime(2024, 11, 1), 1100, 1200, 1300, 1400, 1500, 1600),
            ];

            try
            {
                EnergyConsumptionFileDal.Save(energyConsumptions);
                Assert.IsTrue(File.Exists(filename), $"The file {filename} should exist after saving");
                List<EnergyConsumption> savedEnergyConsumptions = EnergyConsumptionFileDal.Load();
                Assert.AreEqual(2, savedEnergyConsumptions.Count, "There should be 2 energy consumptions stored");
            }
            catch
            {
                Assert.Fail("Save and load of data should work well");
            }
            finally
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }
    }
}