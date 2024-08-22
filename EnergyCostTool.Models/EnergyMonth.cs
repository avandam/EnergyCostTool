using EnergyCostTool.Models.Exceptions;

namespace EnergyCostTool.Models
{
    public class EnergyMonth
    {
        public DateTime Month { get; private set; }
        public Consumption Consumption { get; private set; }
        public Price Price { get; private set; }

        public EnergyMonth(DateTime month)
        {
            if (month.Day != 1)
            {
                throw new EnergyConsumptionException("The day for an energyConsumption must be 1");
            }
            Month = month;
        }

        public EnergyMonth(DateTime month, Consumption consumption) : this(month)
        {
            Consumption = consumption;
        }

        public EnergyMonth(DateTime month, Price price) : this(month)
        {
            Price = price;
        }

        public void AddOrUpdate(Price price)
        {
            Price = price;
        }

        public void AddOrUpdate(Consumption consumption)
        {
            Consumption = consumption;
        }
    }
}
