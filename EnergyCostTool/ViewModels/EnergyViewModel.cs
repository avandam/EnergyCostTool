using EnergyCostTool.Data;

namespace EnergyCostTool.ViewModels;

public class EnergyViewModel
{
    public EnergyConsumptionCollection EnergyConsumptionCollection { get; private set; }
    public EnergyPriceCollection EnergyPriceCollection { get; private set; }
    public FixedCostCollection FixedCostCollection { get; private set; }

    public EnergyViewModel()
    {
        EnergyConsumptionCollection = new EnergyConsumptionCollection();
        EnergyPriceCollection = new EnergyPriceCollection();
        FixedCostCollection = new FixedCostCollection();
    }
}