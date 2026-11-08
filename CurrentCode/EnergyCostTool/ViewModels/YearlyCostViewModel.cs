namespace EnergyCostTool.ViewModels;

public class YearlyCostViewModel
{
    public int Year { get; set; }
    public int NormUsed { get; set; }
    public int NormReturned { get; set; }
    public int LowUsed { get; set; }
    public int LowReturned { get; set; }
    public int GasUsed { get; set; }
    public int SolarGenerated { get; set; }
    public double NormPrice { get; set; }
    public double LowPrice { get; set; }
    public double GasPrice { get; set; }
    public double StandingChargesElectricity { get; set; }
    public double TransportCostElectricity { get; set; }
    public double StandingChargesGas { get; set; }
    public double TransportCostGas { get; set; }
    public double DiscountOnEnergyTax { get; set; }
    public double SolarCost { get; set; }
    public double PayedDeposits { get; set; }
    public double TotalElectricityPrice { get; set; }
    public double TotalGasPrice { get; set; }
    public double TotalPrice { get; set; }

    public YearlyCostViewModel(int year)
    {
        this.Year = year;
    }
}