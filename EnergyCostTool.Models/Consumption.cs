namespace EnergyCostTool.Models;

public class Consumption
{
    public int SolarGeneration { get; private set; }
    public int ElectricityHigh { get; private set; }
    public int ReturnElectricityHigh { get; private set; }
    public int ElectricityLow { get; private set; }
    public int ReturnElectricityLow { get; private set; }
    public int Gas { get; private set; }

    // For JSON Serialization
    public Consumption()
    {

    }

    public Consumption(int solarGeneration, int electricityHigh, int returnElectricityHigh, int electricityLow, int returnElectricityLow, int gas)
    {
        SolarGeneration = solarGeneration;
        ElectricityHigh = electricityHigh;
        ReturnElectricityHigh = returnElectricityHigh;
        ElectricityLow = electricityLow;
        ReturnElectricityLow = returnElectricityLow;
        Gas = gas;
    }
}