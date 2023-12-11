using System.ComponentModel;

namespace EnergyCostTool.Data
{
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
        [Description("Korting op Energiebelasting")] DiscountOnEnergyTax
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

        public FixedCost(DateTime startDate, FixedCostType costType, double price, FixedCostTariffType type)
        {
            StartDate = startDate;
            CostType = costType;
            Price = price;
            TariffType = type;
        }
    }
}
