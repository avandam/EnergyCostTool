using EnergyCostTool.Models.Enumerations;

namespace EnergyCostTool.Models;

public class StandardCost
{
    public DateTime StartDate { get; private set; }
    public FixedCostType CostType { get; private set; }
    public double Price { get; private set; }
    public FixedCostTariffType TariffType { get; private set; }

    public StandardCost(DateTime startDate, FixedCostType costType, double price)
    {
        StartDate = startDate;
        CostType = costType;
        Price = price;
        TariffType = costType switch
        {
            FixedCostType.StandingChargeElectricity => FixedCostTariffType.Monthly,
            FixedCostType.StandingChargeGas => FixedCostTariffType.Monthly,
            FixedCostType.MonthlyDeposit => FixedCostTariffType.Monthly,
            FixedCostType.TransportCostElectricity => FixedCostTariffType.Daily,
            FixedCostType.TransportCostGas => FixedCostTariffType.Daily,
            FixedCostType.DiscountOnEnergyTax => FixedCostTariffType.Daily,
            FixedCostType.SolarCost => FixedCostTariffType.MonthlyCanBeZero,
            _ => throw new ArgumentOutOfRangeException(nameof(costType), costType, null)
        };
    }
}