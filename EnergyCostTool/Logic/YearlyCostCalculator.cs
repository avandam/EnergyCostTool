using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyCostTool.Data;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool.Logic
{
    public static class YearlyCostCalculator
    {
        public static YearlyCostViewModel GetYearlyCostForYear(int year, EnergyViewModel energyData)
        {
            List<EnergyConsumption> energyConsumptions = energyData.EnergyConsumptionCollection.Get()
                .FindAll(consumption => consumption.Month.Year == year);
            List<EnergyPrice> energyPrices =
                energyData.EnergyPriceCollection.Get().FindAll(price => price.StartDate.Year == year);

            YearlyCostViewModel yearlyCostViewModel = new YearlyCostViewModel(year);
            // First compute the total usage
            yearlyCostViewModel = ComputeTotalUsage(yearlyCostViewModel, energyConsumptions);
            // Then compute the total price for each of the usages
            yearlyCostViewModel = ComputeUsagePrices(yearlyCostViewModel, energyConsumptions, energyPrices);
            // Finally get the fixed costs
            yearlyCostViewModel = ComputeFixedCosts(yearlyCostViewModel, energyData.FixedCostCollection);

            // Compute totals
            yearlyCostViewModel.TotalElectricityPrice = yearlyCostViewModel.NormPrice +
                                                        yearlyCostViewModel.LowPrice +
                                                        yearlyCostViewModel.StandingChargesElectricity +
                                                        yearlyCostViewModel.TransportCostElectricity +
                                                        yearlyCostViewModel.DiscountOnEnergyTax;

            yearlyCostViewModel.TotalGasPrice = yearlyCostViewModel.GasPrice + yearlyCostViewModel.StandingChargesGas +
                                                yearlyCostViewModel.TransportCostGas;

            yearlyCostViewModel.TotalPrice =
                yearlyCostViewModel.TotalElectricityPrice + yearlyCostViewModel.TotalGasPrice;

            return yearlyCostViewModel;
        }



        private static YearlyCostViewModel ComputeTotalUsage(YearlyCostViewModel yearlyCostViewModel,
            List<EnergyConsumption> energyConsumptions)
        {
            yearlyCostViewModel.NormUsed = energyConsumptions.Sum(consumption => consumption.ElectricityHigh);
            yearlyCostViewModel.NormReturned = energyConsumptions.Sum(consumption => consumption.ReturnElectricityHigh);
            yearlyCostViewModel.LowUsed = energyConsumptions.Sum(consumption => consumption.ElectricityLow);
            yearlyCostViewModel.LowReturned = energyConsumptions.Sum(consumption => consumption.ReturnElectricityLow);
            yearlyCostViewModel.GasUsed = energyConsumptions.Sum(consumption => consumption.Gas);
            yearlyCostViewModel.SolarGenerated = energyConsumptions.Sum(consumption => consumption.SolarGeneration);

            return yearlyCostViewModel;
        }

        private static YearlyCostViewModel ComputeUsagePrices(YearlyCostViewModel yearlyCostViewModel,
            List<EnergyConsumption> consumptions, List<EnergyPrice> prices)
        {
            yearlyCostViewModel.NormPrice = 0;
            yearlyCostViewModel.LowPrice = 0;
            yearlyCostViewModel.GasPrice = 0;
            foreach (EnergyConsumption energyConsumption in consumptions)
            {
                List<EnergyPrice> energyPricesForMonth =
                    prices.FindAll(price => price.StartDate.Month == energyConsumption.Month.Month);
                if (!energyPricesForMonth.Any())
                {
                    throw new ArgumentException($"No prices found month {energyConsumption.Month.Month}");
                }
                else if (energyPricesForMonth.Count() == 2)
                {
                    throw new NotImplementedException("Functionality for split months has not been implemented yet");
                }

                EnergyPrice price = energyPricesForMonth[0];
                yearlyCostViewModel.NormPrice += energyConsumption.ElectricityHigh *
                                                 Math.Min(price.ElectricityHigh, price.ElectricityCap);
                yearlyCostViewModel.LowPrice += energyConsumption.ElectricityLow *
                                                Math.Min(price.ElectricityLow, price.ElectricityCap);
                yearlyCostViewModel.GasPrice += energyConsumption.Gas * Math.Min(price.Gas, price.GasCap);
            }

            // Compute return price
            yearlyCostViewModel.NormPrice -= (consumptions.Sum(consumption => consumption.ReturnElectricityHigh) *
                                              prices.Average(price => price.ReturnElectricityHigh));
            yearlyCostViewModel.LowPrice -= (consumptions.Sum(consumption => consumption.ReturnElectricityLow) *
                                             prices.Average(price => price.ReturnElectricityLow));

            return yearlyCostViewModel;
        }

        private static YearlyCostViewModel ComputeFixedCosts(YearlyCostViewModel yearlyCostViewModel,
            FixedCostCollection fixedCosts)
        {
            yearlyCostViewModel.StandingChargesElectricity = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.StandingChargeElectricity, fixedCosts);
        }

        private static double ComputeFixedCostForType(int year, FixedCostType fixedCostType,
            FixedCostCollection fixedCostsInput)
        {
            List<FixedCost> fixedCosts = fixedCostsInput.Get(year, fixedCostType);
            if (fixedCosts.Count == 0)
            {
                throw new ArgumentException($"No cost found for {fixedCostType}");
            }

            switch (fixedCosts[0].TariffType)
            {
                case FixedCostTariffType.Daily:
                    return ComputeFixedCostDaily(year, fixedCosts);
                case FixedCostTariffType.Monthly:
                    break;
                case FixedCostTariffType.Yearly:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return 0;
        }

        private static double ComputeFixedCostDaily(int year, List<FixedCost> fixedCosts)
        {
            if (fixedCosts.Count == 1 && fixedCosts[0].StartDate.Year < year)
            {
                int numberOfDays = DateTime.IsLeapYear(year) ? 366 : 365;
                return numberOfDays * fixedCosts[0].Price;
            }

            double result = 0;

            for (int i = 0; i < fixedCosts.Count - 1; i++)
            {
                TimeSpan span = fixedCosts[i + 1].StartDate.Subtract(fixedCosts[i].StartDate.Year < year ? new DateTime(year, 1, 1) : fixedCosts[i].StartDate);
                int numberOfDays = span.Days;
                result += numberOfDays * fixedCosts[i].Price;
            }

            TimeSpan finalSpan = new DateTime(year, 12, 31).Subtract(fixedCosts.Last().StartDate);
            result += finalSpan.Days * fixedCosts.Last().Price;

            return result;
        }
    }
}
