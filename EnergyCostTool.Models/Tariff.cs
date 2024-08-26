namespace EnergyCostTool.Models;

public class Tariff
{
    public double ElectricityHigh { get; set; }
    public double ReturnElectricityHigh { get; set; }
    public double ElectricityLow { get; set; }
    public double ReturnElectricityLow { get; set; }
    public double Gas { get; set; }
    public double ElectricityCap { get; set; }
    public double GasCap { get; set; }
        
    // By default there is no price cap. In that case, set them to an unrealisitcly large value, such that the regular price is always selected.
    public Tariff(double electricityHigh, double returnElectricityHigh, double electricityLow, double returnElectricityLow, double gas)
        : this (electricityHigh, returnElectricityHigh, electricityLow, returnElectricityLow, gas, 1000, 1000)
    {
         
    }

    public Tariff(double electricityHigh, double returnElectricityHigh, double electricityLow, double returnElectricityLow, double gas, double electricityCap, double gasCap)
    {
        ElectricityHigh = electricityHigh;
        ReturnElectricityHigh = returnElectricityHigh;
        ElectricityLow = electricityLow;
        ReturnElectricityLow = returnElectricityLow;
        Gas = gas;
        ElectricityCap = electricityCap;
        GasCap = gasCap;
    }
}