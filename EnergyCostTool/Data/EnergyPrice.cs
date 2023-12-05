namespace EnergyCostTool.Data
{
    public class EnergyPrice
    {
        public DateTime StartDate { get; internal set; }
        public double ElectricityHigh { get; internal set; }
        public double ReturnElectricityHigh { get; internal set; }
        public double ElectricityLow { get; internal set; }
        public double ReturnElectricityLow { get; internal set; }
        public double Gas { get; internal set; }
        public double ElectricityCap { get; internal set; }
        public double GasCap { get; internal set; }
        
        // For JSON Serialization
        public EnergyPrice()
        {

        }

        // By default there is no price cap. In that case, set them to the max value, such that the regular price is always selected.
        public EnergyPrice(DateTime startDate, double electricityHigh, double returnElectricityHigh, double electricityLow, double returnElectricityLow, double gas)
            : this (startDate, electricityHigh, returnElectricityHigh, electricityLow, returnElectricityLow, gas, double.MaxValue, double.MaxValue)
        {
         
        }

        public EnergyPrice(DateTime startDate, double electricityHigh, double returnElectricityHigh, double electricityLow, double returnElectricityLow, double gas, double electricityCap, double gasCap)
        {
            StartDate = startDate;
            ElectricityHigh = electricityHigh;
            ReturnElectricityHigh = returnElectricityHigh;
            ElectricityLow = electricityLow;
            ReturnElectricityLow = returnElectricityLow;
            Gas = gas;
            ElectricityCap = electricityCap;
            GasCap = gasCap;
        }
    }
}
