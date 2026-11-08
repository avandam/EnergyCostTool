namespace EnergyCostTool.ViewModels;

public class SolarInformationViewModel
{
    public int Generation { get; set; }
    public int DirectlyUsed { get; set; }
    public double DirectlyUsedPrice { get; set; }
    public int ReturnedNorm { get; set; }
    public double ReturnedNormPrice { get; set; }
    public int ReturnedLow { get; set; }
    public double ReturnedLowPrice { get; set; }
    public double FixedCostPrice { get; set; }

    public double TotalPrice
    {
        get
        {
            return DirectlyUsedPrice + ReturnedLowPrice + ReturnedNormPrice + FixedCostPrice;
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
            double pricePerMonth = TotalPrice / NrOfMonthsWithPanels;
            return Math.Max(0, Convert.ToInt32(PriceOfSolarPanels / pricePerMonth));
        }
    }
    public int NrOfMonthsWithPanels { get; set; }

    public SolarInformationViewModel()
    {
        
    }
}