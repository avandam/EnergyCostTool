namespace EnergyCostTool.Data
{
    public class TariffCap
    {
        public decimal ElectricityHigh { get; private set; }
        public decimal ElectricityLow { get; private set; }
        public decimal Gas { get; private set; }

        public TariffCap(decimal electricityHigh, decimal electricityLow, decimal gas)
        {
            ElectricityHigh = electricityHigh;
            ElectricityLow = electricityLow;
            Gas = gas;
        }
    }
}
