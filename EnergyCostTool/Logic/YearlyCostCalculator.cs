using EnergyCostTool.Dal;
using EnergyCostTool.Models;
using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool.Logic
{
    public static class YearlyCostCalculator
    {
        public static YearlyCostViewModel GetYearlyCostForYear(int year, EnergyMonthCollection energyMonths)
        {
            YearlyCostViewModel yearlyCostViewModel = new YearlyCostViewModel(year);

            yearlyCostViewModel = ComputeTotalUsage(yearlyCostViewModel, energyMonths);
            yearlyCostViewModel = ComputeUsagePrices(yearlyCostViewModel, energyMonths);
            yearlyCostViewModel = ComputeFixedCosts(yearlyCostViewModel, energyMonths);

            // Compute totals
            yearlyCostViewModel.TotalElectricityPrice = yearlyCostViewModel.NormPrice +
                                                        yearlyCostViewModel.LowPrice +
                                                        yearlyCostViewModel.StandingChargesElectricity +
                                                        yearlyCostViewModel.TransportCostElectricity +
                                                        yearlyCostViewModel.DiscountOnEnergyTax + 
                                                        yearlyCostViewModel.SolarCost;
            yearlyCostViewModel.TotalElectricityPrice = Math.Round(yearlyCostViewModel.TotalElectricityPrice, 2);

            yearlyCostViewModel.TotalGasPrice = yearlyCostViewModel.GasPrice + 
                                                yearlyCostViewModel.StandingChargesGas +
                                                yearlyCostViewModel.TransportCostGas;
            yearlyCostViewModel.TotalGasPrice = Math.Round(yearlyCostViewModel.TotalGasPrice, 2);

            yearlyCostViewModel.TotalPrice = yearlyCostViewModel.TotalElectricityPrice + 
                                             yearlyCostViewModel.TotalGasPrice;
            yearlyCostViewModel.TotalPrice = Math.Round(yearlyCostViewModel.TotalPrice, 2);

            return yearlyCostViewModel;
        }
        
        private static YearlyCostViewModel ComputeTotalUsage(YearlyCostViewModel yearlyCostViewModel,
            EnergyMonthCollection energyConsumptions)
        {
            yearlyCostViewModel.NormUsed = energyConsumptions.Get().Sum(energyMonth => energyMonth.Consumption.ElectricityHigh);
            yearlyCostViewModel.NormReturned = energyConsumptions.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityHigh);
            yearlyCostViewModel.LowUsed = energyConsumptions.Get().Sum(energyMonth => energyMonth.Consumption.ElectricityLow);
            yearlyCostViewModel.LowReturned = energyConsumptions.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityLow);
            yearlyCostViewModel.GasUsed = energyConsumptions.Get().Sum(energyMonth => energyMonth.Consumption.Gas);
            yearlyCostViewModel.SolarGenerated = energyConsumptions.Get().Sum(energyMonth => energyMonth.Consumption.SolarGeneration);

            return yearlyCostViewModel;
        }

        private static YearlyCostViewModel ComputeUsagePrices(YearlyCostViewModel yearlyCostViewModel,
            EnergyMonthCollection energyMonths)
        {
            yearlyCostViewModel.NormPrice = 0;
            yearlyCostViewModel.LowPrice = 0;
            
            int actualNormUsage = yearlyCostViewModel.NormUsed - yearlyCostViewModel.NormReturned;
            yearlyCostViewModel.NormPrice = actualNormUsage < 0
                ? energyMonths.Get().Average(energyMonth => energyMonth.Tariff.ReturnElectricityHigh) * actualNormUsage
                : energyMonths.Get().Average(energyMonth => Math.Min(energyMonth.Tariff.ElectricityHigh, energyMonth.Tariff.ElectricityCap)) * actualNormUsage;
            yearlyCostViewModel.NormPrice = Math.Round(yearlyCostViewModel.NormPrice, 2);


            int actualLowUsage = yearlyCostViewModel.LowUsed - yearlyCostViewModel.LowReturned;
            yearlyCostViewModel.LowPrice = actualLowUsage < 0
                ? energyMonths.Get().Average(energyMonth => energyMonth.Tariff.ReturnElectricityLow) * actualLowUsage
                : energyMonths.Get().Average(energyMonth => Math.Min(energyMonth.Tariff.ElectricityLow, energyMonth.Tariff.ElectricityCap)) * actualLowUsage;
            yearlyCostViewModel.LowPrice = Math.Round(yearlyCostViewModel.LowPrice, 2);

            yearlyCostViewModel.GasPrice = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.Gas * Math.Min(energyMonth.Tariff.Gas, energyMonth.Tariff.GasCap)), 2);

            return yearlyCostViewModel;
        }

        private static YearlyCostViewModel ComputeFixedCosts(YearlyCostViewModel yearlyCostViewModel, EnergyMonthCollection energyMonths)
        {
            yearlyCostViewModel.StandingChargesElectricity = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.StandingChargeElectricity)), 2);
            yearlyCostViewModel.StandingChargesGas = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.StandingChargeGas)), 2);
            yearlyCostViewModel.TransportCostElectricity = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.TransportCostElectricity)), 2);
            yearlyCostViewModel.TransportCostGas = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.TransportCostGas)), 2);
            yearlyCostViewModel.DiscountOnEnergyTax = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.DiscountOnEnergyTax)), 2);
            yearlyCostViewModel.PayedDeposits = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.MonthlyDeposit)), 2);
            yearlyCostViewModel.SolarCost = Math.Round(energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.SolarCost)), 2);

            return yearlyCostViewModel;
        }
    }
}
