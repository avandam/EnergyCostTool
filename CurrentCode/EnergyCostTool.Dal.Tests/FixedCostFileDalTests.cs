using NUnit.Framework;
using EnergyCostTool.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyCostTool.Models;
using EnergyCostTool.Dal.DataModels;
using EnergyCostTool.Models.Enumerations;

namespace EnergyCostTool.Dal.Tests
{
    [TestFixture()]
    public class FixedCostFileDalTests
    {
        private const string filename = "fixedCosts.dat";

        [Test()]
        public void FileHandlingTest()
        {
            List<FixedCost> fixedCosts =
            [
                new FixedCost(new DateTime(2024, 10, 1), FixedCostType.StandingChargeElectricity, 2.99),
                new FixedCost(new DateTime(2024, 10, 1), FixedCostType.TransportCostElectricity, 5.99),
                new FixedCost(new DateTime(2024, 10, 2), FixedCostType.TransportCostElectricity, 6.99),
            ];

            try
            {
                FixedCostFileDal.Save(fixedCosts);
                Assert.IsTrue(File.Exists(filename), $"The file {filename} should exist after saving");
                List<FixedCost> savedFixedCosts = FixedCostFileDal.Load();
                Assert.AreEqual(3, savedFixedCosts.Count, "There should be 3 fixed costs stored");
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