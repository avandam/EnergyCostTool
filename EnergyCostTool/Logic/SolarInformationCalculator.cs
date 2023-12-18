using EnergyCostTool.Data;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool.Logic
{
    public static class SolarInformationCalculator
    {
        public static SolarInformationViewModel ComputeSolarInformation(EnergyViewModel energyData)
        {
            SolarInformationViewModel solarInformation = new SolarInformationViewModel();

            solarInformation.SolarGenerated = energyData.EnergyConsumptionCollection.Get().Sum(consumption => consumption.SolarGeneration);
            solarInformation.NrOfMonthsOfSolarPanels = energyData.EnergyConsumptionCollection.Get().Count(consumption => consumption.SolarGeneration > 0);

            // Compute usage and prices
            solarInformation = ComputeDetailedInformation(solarInformation, energyData);

            return solarInformation;
        }

        private static SolarInformationViewModel ComputeDetailedInformation(SolarInformationViewModel solarInformation, EnergyViewModel energyData)
        {
            solarInformation.SolarDirectlyUsed = 0;
            solarInformation.SolarDirectlyUsedPrice = 0.0;
            solarInformation.SolarReturnedNorm = 0;
            solarInformation.SolarReturnedNormPrice = 0.0;
            solarInformation.SolarReturnedLow = 0;
            solarInformation.SolarReturnedLowPrice = 0.0;

            foreach (EnergyConsumption energyConsumption in energyData.EnergyConsumptionCollection.Get().FindAll(consumption => consumption.SolarGeneration > 0))
            {
                EnergyPrice energyPrice = energyData.EnergyPriceCollection.Get(energyConsumption.Month);
                int solarDirectlyUsed = energyConsumption.SolarGeneration - energyConsumption.ReturnElectricityHigh - energyConsumption.ReturnElectricityLow;
                double normFactor = (double)energyConsumption.ReturnElectricityHigh / (energyConsumption.ReturnElectricityHigh + energyConsumption.ReturnElectricityLow);
                int directlyUsedNorm = Convert.ToInt32(normFactor * solarDirectlyUsed);
                int directlyUsedLow = solarDirectlyUsed - directlyUsedNorm;

                solarInformation.SolarDirectlyUsed += solarDirectlyUsed;
                solarInformation.SolarReturnedNorm += energyConsumption.ReturnElectricityHigh;
                solarInformation.SolarReturnedLow += energyConsumption.ReturnElectricityLow;
                solarInformation.SolarDirectlyUsedPrice += directlyUsedNorm * Math.Min(energyPrice.ElectricityHigh, energyPrice.ElectricityCap);
                solarInformation.SolarDirectlyUsedPrice += directlyUsedLow * Math.Min(energyPrice.ElectricityLow, energyPrice.ElectricityCap);
                solarInformation.SolarReturnedNormPrice += energyConsumption.ReturnElectricityHigh * energyPrice.ReturnElectricityHigh;
                solarInformation.SolarReturnedLowPrice += energyConsumption.ReturnElectricityLow * energyPrice.ReturnElectricityLow;
            }

            return solarInformation;
        }
    }
}
