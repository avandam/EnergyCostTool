using EnergyCostTool.Exceptions;
using System.ComponentModel;

namespace EnergyCostTool.Data;

public class EnergyConsumption : INotifyPropertyChanged
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

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}