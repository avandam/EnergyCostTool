namespace EnergyCostTool.Data
{
    public class Usage
    { 
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int ElectricityHigh { get; private set; }
        public int ReturnElectricityHigh { get; private set; }
        public int ElectricityLow { get; private set; }
        public int ReturnElectricityLow { get; private set; }
        public int Gas { get; private set; }

        public Usage(int year, int month, int electricityHigh, int returnElectricityHigh, int electricityLow, int returnElectricityLow, int gas)
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
