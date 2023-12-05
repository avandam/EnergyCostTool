using EnergyCostTool.Exceptions;

namespace EnergyCostTool.Data
{
    public class EnergyConsumption
    {
        public DateTime Month { get; private set; }
        public int SolarGeneration { get; internal set; }
        public int ElectricityHigh { get; internal set; }
        public int ReturnElectricityHigh { get; internal set; }
        public int ElectricityLow { get; internal set; }
        public int ReturnElectricityLow { get; internal set; }
        public int Gas { get; internal set; }

        // For JSON Serialization
        public EnergyConsumption()
        {

        }

        public EnergyConsumption(DateTime month, int solarGeneration, int electricityHigh, int returnElectricityHigh, int electricityLow, int returnElectricityLow, int gas)
        {
            if (month.Day != 1)
            {
                throw new EnergyConsumptionException("The day for an energyConsumption must be 1");
            }
            Month = month;
            SolarGeneration = solarGeneration;
            ElectricityHigh = electricityHigh;
            ReturnElectricityHigh = returnElectricityHigh;
            ElectricityLow = electricityLow;
            ReturnElectricityLow = returnElectricityLow;
            Gas = gas;
        }
    }
}
