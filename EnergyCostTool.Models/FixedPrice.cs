using EnergyCostTool.Models.Enumerations;

namespace EnergyCostTool.Models;

public class FixedPrice
{
    public FixedCostType FixedCostType { get; private set; }
    public double Price { get; private set; }

    public FixedPrice(FixedCostType fixedCostType, double price)
    {
        FixedCostType = fixedCostType;
        Price = price;        
    }
}