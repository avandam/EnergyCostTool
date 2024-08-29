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

            solarInformation.SolarGenerated = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.SolarGeneration);
            solarInformation.NrOfMonthsOfSolarPanels = energyMonths.Get().Count;
            solarInformation.SolarCostPrice = -energyMonths.Get().Sum(energyMonth => energyMonth.GetFixedPrice(FixedCostType.SolarCost));
            solarInformation.SolarGenerated = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.SolarGeneration);
            solarInformation.SolarDirectlyUsed = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.DirectlyUsed);
            solarInformation.SolarReturnedNorm = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityHigh);
            solarInformation.SolarReturnedLow = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityLow);
            solarInformation.SolarReturnedNormPrice = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityHigh * energyMonth.Tariff.ReturnElectricityHigh);
            solarInformation.SolarReturnedLowPrice = energyMonths.Get().Sum(energyMonth => energyMonth.Consumption.ReturnElectricityLow * energyMonth.Tariff.ReturnElectricityLow);
            solarInformation.SolarDirectlyUsedPrice = energyMonths.Get().Sum(energyMonth => energyMonth.GetDirectlyUsedPrice() + energyMonth.GetDirectlyUsedLowPrice());
            
            return solarInformation;
        }

    }
}
