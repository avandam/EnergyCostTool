using NUnit.Framework;
using EnergyCostTool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyCostTool.Data;

namespace EnergyCostTool.Logic.Tests
{
    [TestFixture()]
    public class YearlyCostCalculatorTests
    {
        [Test()]
        public void ComputeFixedCostMonthlyForWholeYearTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2023,1,1), FixedCostType.StandingChargeElectricity, 5.99));

            Assert.AreEqual(12*5.99, YearlyCostCalculator.ComputeFixedCostMonthly(2023, fixedCosts));
        }

        [Test()]
        public void ComputeFixedCostMonthlyValueFromOldYearTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2022, 1, 1), FixedCostType.StandingChargeElectricity, 5.99));

            Assert.AreEqual(12 * 5.99, YearlyCostCalculator.ComputeFixedCostMonthly(2023, fixedCosts));
        }
       
        [Test()]
        public void ComputeFixedCostMonthlyWithChangeHalfwayTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2023, 1, 1), FixedCostType.StandingChargeElectricity, 5.99));
            fixedCosts.Add(new FixedCost(new DateTime(2023, 7, 1), FixedCostType.StandingChargeElectricity, 6.99));

            Assert.AreEqual(6 * 5.99 + 6 * 6.99, YearlyCostCalculator.ComputeFixedCostMonthly(2023, fixedCosts));
        }

        [Test()]
        public void ComputeFixedCostDailyForWholeYearTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2023, 1, 1), FixedCostType.TransportCostElectricity, 0.12345));

            Assert.AreEqual(365 * 0.12345, YearlyCostCalculator.ComputeFixedCostDaily(2023, fixedCosts));
        }

        [Test()]
        public void ComputeFixedCostDailyForWholeLeapYearTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2024, 1, 1), FixedCostType.TransportCostElectricity, 0.12345));

            Assert.AreEqual(366 * 0.12345, YearlyCostCalculator.ComputeFixedCostDaily(2024, fixedCosts));
        }


        [Test()]
        public void ComputeFixedCostDailyValueFromOldYearTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2022, 1, 1), FixedCostType.TransportCostElectricity, 0.12345));

            Assert.AreEqual(365 * 0.12345, YearlyCostCalculator.ComputeFixedCostDaily(2023, fixedCosts));
        }

        [Test()]
        public void ComputeFixedCostDailyWithChangeHalfwayTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2023, 1, 1), FixedCostType.TransportCostElectricity, 0.12345));
            fixedCosts.Add(new FixedCost(new DateTime(2023, 7, 1), FixedCostType.TransportCostElectricity, 0.23456));

            Assert.AreEqual(181 * 0.12345 + 184 * 0.23456, YearlyCostCalculator.ComputeFixedCostDaily(2023, fixedCosts));
        }

        [Test()]
        public void ComputeFixedCostDailyWithChangeHalfwayLeapyearTest()
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();
            fixedCosts.Add(new FixedCost(new DateTime(2024, 1, 1), FixedCostType.TransportCostElectricity, 0.12345));
            fixedCosts.Add(new FixedCost(new DateTime(2024, 7, 1), FixedCostType.TransportCostElectricity, 0.23456));

            Assert.AreEqual(182 * 0.12345 + 184 * 0.23456, YearlyCostCalculator.ComputeFixedCostDaily(2024, fixedCosts));
        }




    }
}