using NUnit.Framework;
using EnergyCostTool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyCostTool.Logic.Tests
{
    [TestFixture()]
    public class SolarInformationCalculatorTests
    {
        [TestCase(2024, 01, 2024, 02, 2)]
        [TestCase(2024, 01, 2024, 01, 1)]
        [TestCase(2023, 11, 2024, 01, 3)]
        [TestCase(2022, 11, 2024, 01, 15)]
        public void GetNrOfMonthsTest(int startYear, int startMonth, int endYear, int endMonth, int expectedResult)
        {
            Assert.AreEqual(expectedResult, SolarInformationCalculator.GetNrOfMonths(new DateTime(startYear, startMonth, 1), new DateTime(endYear, endMonth, 1)));
        }
    }
}