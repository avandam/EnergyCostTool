using System.ComponentModel;

namespace EnergyCostTool.Data;

public enum FixedCostTariffType
{
    Daily = 0,
    Monthly = 1,
    Yearly = 3
}

public enum FixedCostType
{
    [Description("Vastrecht Electra")] StandingChargeElectricity,
    [Description("Vastrecht Gas")] StandingChargeGas,
    [Description("Transportkosten Elektra")] TransportCostElectricity,
    [Description("Transportkosten Gas")] TransportCostGas,
    [Description("Korting op Energiebelasting")] DiscountOnEnergyTax,
}

public class FixedCost
{
    public DateTime StartDate { get; set; }
    public FixedCostType CostType { get; set; }
    public double Price { get; set; }
    public FixedCostTariffType TariffType { get; set; }

    // For JSON Serialization
    public FixedCost()
    {

    }

    public FixedCost(DateTime startDate, FixedCostType costType, double price)
    {
        StartDate = startDate;
        CostType = costType;
        Price = price;
        switch (costType)
        {
            case FixedCostType.StandingChargeElectricity:
            case FixedCostType.StandingChargeGas:
                TariffType = FixedCostTariffType.Monthly;
                break;
            case FixedCostType.TransportCostElectricity:
            case FixedCostType.TransportCostGas:
            case FixedCostType.DiscountOnEnergyTax:
                TariffType = FixedCostTariffType.Daily;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(costType), costType, null);
        }
    }
}