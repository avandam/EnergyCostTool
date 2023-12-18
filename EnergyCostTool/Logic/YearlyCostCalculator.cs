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
            int actualNormUsage = yearlyCostViewModel.NormUsed - yearlyCostViewModel.NormReturned;
            yearlyCostViewModel.NormPrice = actualNormUsage < 0
                ? prices.Average(price => price.ReturnElectricityHigh) * actualNormUsage
                : prices.Average(price => price.ElectricityHigh) * actualNormUsage;


            int actualLowUsage = yearlyCostViewModel.LowUsed - yearlyCostViewModel.LowReturned;
            yearlyCostViewModel.LowPrice = actualLowUsage < 0
                ? prices.Average(price => price.ReturnElectricityLow) * actualLowUsage
                : prices.Average(price => price.ElectricityLow) * actualLowUsage;

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
                    int splitDay = energyPricesForMonth[1].StartDate.Day;
                    int lastDay = DateTime.DaysInMonth(energyPricesForMonth[1].StartDate.Year, energyPricesForMonth[1].StartDate.Month);
                    double factor = ((double)splitDay - 1) / lastDay;
                    int firstPartUsage = Convert.ToInt32(energyConsumption.Gas * factor);
                    int secondPartUsage = energyConsumption.Gas - firstPartUsage;

                    yearlyCostViewModel.GasPrice += firstPartUsage * Math.Min(energyPricesForMonth[0].Gas, energyPricesForMonth[0].GasCap);
                    yearlyCostViewModel.GasPrice += secondPartUsage * Math.Min(energyPricesForMonth[1].Gas, energyPricesForMonth[1].GasCap);
                }
                else
                {
                    EnergyPrice price = energyPricesForMonth[0];
                    yearlyCostViewModel.GasPrice += energyConsumption.Gas * Math.Min(price.Gas, price.GasCap);
                }
            }

            return yearlyCostViewModel;
        }

        private static YearlyCostViewModel ComputeFixedCosts(YearlyCostViewModel yearlyCostViewModel, FixedCostCollection fixedCosts)
        {
            yearlyCostViewModel.StandingChargesElectricity = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.StandingChargeElectricity, fixedCosts);
            yearlyCostViewModel.StandingChargesGas = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.StandingChargeGas, fixedCosts);
            yearlyCostViewModel.TransportCostElectricity = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.TransportCostElectricity, fixedCosts);
            yearlyCostViewModel.TransportCostGas = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.TransportCostGas, fixedCosts);
            yearlyCostViewModel.DiscountOnEnergyTax = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.DiscountOnEnergyTax, fixedCosts);
            yearlyCostViewModel.PayedDeposits = ComputeFixedCostForType(yearlyCostViewModel.Year, FixedCostType.MonthlyDeposit, fixedCosts);

            return yearlyCostViewModel;
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
                    return ComputeFixedCostMonthly(year, fixedCosts);
                case FixedCostTariffType.Yearly:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static double ComputeFixedCostMonthly(int year, List<FixedCost> fixedCosts)
        {
            if (fixedCosts.Count == 1)
            {
                return 12 * fixedCosts[0].Price;
            }

            double result = 0;

            for (int i = 0; i < fixedCosts.Count - 1; i++)
            {
                int startMonth = fixedCosts[i].StartDate.Year < year ? 1 : fixedCosts[i].StartDate.Month;
                int numberofMonths = fixedCosts[i + 1].StartDate.Month - startMonth;
                result += numberofMonths * fixedCosts[i].Price;
            }

            result += (13 - fixedCosts.Last().StartDate.Month) * fixedCosts.Last().Price;

            return result;
            }

        internal static double ComputeFixedCostDaily(int year, List<FixedCost> fixedCosts)
        {
            if (fixedCosts.Count == 1)
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

            TimeSpan finalSpan = new DateTime(year + 1, 1, 1).Subtract(fixedCosts.Last().StartDate);
            result += finalSpan.Days * fixedCosts.Last().Price;

            return result;
        }
    }
}
