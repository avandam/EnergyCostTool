using EnergyCostTool.Models;

namespace EnergyCostTool.Dal.DataModels;

public class EnergyConsumption
{
    public DateTime Month { get; set; }
    public int SolarGeneration { get; set; }
    public int ElectricityHigh { get; set; }
    public int ReturnElectricityHigh { get; set; }
    public int ElectricityLow { get; set; }
    public int ReturnElectricityLow { get; set; }
    public int Gas { get; set; }

    // For JSON Serialization
    public EnergyConsumption()
    {

    }

    public EnergyConsumption(DateTime month, int solarGeneration, int electricityHigh, int returnElectricityHigh, int electricityLow, int returnElectricityLow, int gas)
    {
        Month = month;
        SolarGeneration = solarGeneration;
        ElectricityHigh = electricityHigh;
        ReturnElectricityHigh = returnElectricityHigh;
        ElectricityLow = electricityLow;
        ReturnElectricityLow = returnElectricityLow;
        Gas = gas;
    }

    public EnergyConsumption(DateTime month, Consumption consumption)
    {
        Month = month;
        SolarGeneration = consumption.SolarGeneration;
        ElectricityHigh = consumption.ElectricityHigh;
        ReturnElectricityHigh = consumption.ReturnElectricityHigh;
        ElectricityLow = consumption.ElectricityLow;
        ReturnElectricityLow = consumption.ReturnElectricityLow;
        Gas = consumption.Gas;
    }

    public Consumption ConvertToConsumption()
    {
        return new Consumption(SolarGeneration, ElectricityHigh, ReturnElectricityHigh, ElectricityLow, ReturnElectricityLow, Gas);
    }
}