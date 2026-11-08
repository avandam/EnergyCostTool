using EnergyCostTool.Models;
using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool.Logic
{
    public static class SolarInformationCalculator
    {
        public static SolarInformationViewModel ComputeSolarInformation(EnergyMonthCollection energyMonths)
        {
            SolarInformationViewModel solarInformation = new SolarInformationViewModel();

            solarInformation.Generation = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.SolarGeneration);
            solarInformation.NrOfMonthsWithPanels = energyMonths.Get().Count;
            solarInformation.FixedCostPrice = -energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.SolarCost));
            solarInformation.DirectlyUsed = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.DirectlyUsed);
            solarInformation.DirectlyUsedPrice = energyMonths.Get().Sum(energyMonth => energyMonth.GetDirectlyUsedPrice() + energyMonth.GetDirectlyUsedLowPrice());
            solarInformation.ReturnedNorm = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityHigh);
            solarInformation.ReturnedLow = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityLow);
            solarInformation.ReturnedNormPrice = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityHigh * energyMonth.Tariff.ReturnElectricityHigh);
            solarInformation.ReturnedLowPrice = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityLow * energyMonth.Tariff.ReturnElectricityLow);
            
            return solarInformation;
        }

    }
}
