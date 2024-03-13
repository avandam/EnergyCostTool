namespace EnergyCostTool.ViewModels;

public class SolarInformationViewModel
{
    public int SolarGenerated { get; set; }
    public int SolarDirectlyUsed { get; set; }
    public double SolarDirectlyUsedPrice { get; set; }
    public int SolarReturnedNorm { get; set; }
    public double SolarReturnedNormPrice { get; set; }
    public int SolarReturnedLow { get; set; }
    public double SolarReturnedLowPrice { get; set; }
    public double SolarCostPrice { get; set; }

    public double TotalPrice
    {
        get
        {
            return SolarDirectlyUsedPrice + SolarReturnedLowPrice + SolarReturnedNormPrice + SolarCostPrice;
        }
    }

    public double PriceOfSolarPanels
    {
        get
        {
            return 5199.56;
        }
    }

    public double Difference
    {
        get
        {
            return TotalPrice - PriceOfSolarPanels;
        }
    }

    public int NrOfMonthsToReturnInvestment
    {
        get
        {
            double pricePerMonth = TotalPrice / NrOfMonthsOfSolarPanels;
            return Math.Max(0, Convert.ToInt32(PriceOfSolarPanels / pricePerMonth));
        }
    }
    public int NrOfMonthsOfSolarPanels { get; set; }

    public SolarInformationViewModel()
    {
        
    }
}