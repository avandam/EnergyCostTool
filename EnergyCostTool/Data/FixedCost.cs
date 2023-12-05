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
        StandingChargeElectricity,
        StandingChargeGas,
        TransportCostElectricity,
        TransportCostGas,
        DiscountOnEnergyTax
    }

    // TODO: Create a factory for instantiation (maybe reintroduce the enum)
    public class FixedCost
    {
        public DateTime StartDate { get; internal set; }
        public FixedCostType CostType { get; internal set; }
        public double Price { get; internal set; }
        public FixedCostTariffType TariffType { get; internal set; }

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
