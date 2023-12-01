namespace EnergyCostTool.Data
{
    public enum FixedCostType
    {
        Daily = 0,
        Monthly = 1,
    }

    public enum FixedCostName
    {
        // TODO: Fill in this enumeration
    }

    public class FixedCost
    {
        public FixedCostName Name { get; private set; }
        public decimal Price { get; private set; }
        public FixedCostType Type { get; private set; }

        public FixedCost(FixedCostName name, decimal price, FixedCostType type)
        {
            Name = name;
            Price = price;
            Type = type;
        }
    }
}
