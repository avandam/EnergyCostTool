namespace EnergyCostTool.Data
{
    // TODO: Figure out how to solve tariff changes in the middle of the month (on new energy contract). Should we for the tariffs only store the changes?
    public class Tariff
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal ElectricityHigh { get; private set; }
        public decimal ReturnElectricityHigh { get; private set; }
        public decimal ElectricityLow { get; private set; }
        public decimal ReturnElectricityLow { get; private set; }
        public decimal Gas { get; private set; }

        public Tariff(int year, int month, decimal electricityHigh, decimal returnElectricityHigh, decimal electricityLow, decimal returnElectricityLow, decimal gas)
        {
            Year = year;
            Month = month;
            ElectricityHigh = electricityHigh;
            ReturnElectricityHigh = returnElectricityHigh;
            ElectricityLow = electricityLow;
            ReturnElectricityLow = returnElectricityLow;
            Gas = gas;
        }
    }
}
