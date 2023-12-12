namespace EnergyCostTool.Data;

public class EnergyPrice
{
    public DateTime StartDate { get; set; }
    public double ElectricityHigh { get; set; }
    public double ReturnElectricityHigh { get; set; }
    public double ElectricityLow { get; set; }
    public double ReturnElectricityLow { get; set; }
    public double Gas { get; set; }
    public double ElectricityCap { get; set; }
    public double GasCap { get; set; }
        
    // For JSON Serialization
    public EnergyPrice()
    {

    }

    // By default there is no price cap. In that case, set them to the max value, such that the regular price is always selected.
    public EnergyPrice(DateTime startDate, double electricityHigh, double returnElectricityHigh, double electricityLow, double returnElectricityLow, double gas)
        : this (startDate, electricityHigh, returnElectricityHigh, electricityLow, returnElectricityLow, gas, 1000, 1000)
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