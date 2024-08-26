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
    public class EnergyPriceFileDalTests
    {
        private const string filename = "energyPrices.dat";

        [Test()]
        public void FileHandlingTest()
        {
            List<EnergyPrice> energyPrices =
            [
                new EnergyPrice(new DateTime(2024, 10, 1), 0.1, 0.2, 0.3, 0.4, 0.5),
                new EnergyPrice(new DateTime(2024, 12, 1), 10.1, 10.2, 10.3, 10.4, 10.5),
            ];

            try
            {
                EnergyPriceFileDal.Save(energyPrices);
                Assert.IsTrue(File.Exists(filename), $"The file {filename} should exist after saving");
                List<EnergyPrice> savedEnergyPrices = EnergyPriceFileDal.Load();
                Assert.AreEqual(2, savedEnergyPrices.Count, "There should be 2 energy prices stored");
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