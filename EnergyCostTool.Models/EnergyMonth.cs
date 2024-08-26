using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.Models.Exceptions;
using System.Collections.ObjectModel;

namespace EnergyCostTool.Models
{
    public class EnergyMonth
    {
        public DateTime Month { get; private set; }
        public Consumption Consumption { get; private set; }
        public Tariff Tariff { get; private set; }

        private List<FixedPrice> fixedPrices;

        public EnergyMonth(DateTime month)
        {
            if (month.Day != 1)
            {
                throw new EnergyMonthException("The day for an energyConsumption must be 1");
            }
            Month = month;
            fixedPrices = [];
        }

        public EnergyMonth(DateTime month, Consumption consumption) : this(month)
        {
            Consumption = consumption;
        }

        public EnergyMonth(DateTime month, Tariff tariff) : this(month)
        {
            Tariff = tariff;
        }

        public EnergyMonth(DateTime month, FixedPrice fixedPrice) : this(month)
        {
            fixedPrices.Add(fixedPrice);
        }

        public void AddOrUpdate(Tariff tariff)
        {
            Tariff = tariff;
        }

        public void AddOrUpdate(Consumption consumption)
        {
            Consumption = consumption;
        }

        public void AddOrUpdate(FixedPrice fixedPrice)
        {
            Delete(fixedPrice);
            fixedPrices.Add(fixedPrice);
        }

        public void Delete(FixedPrice fixedPrice)
        {
            if (fixedPrices.Exists(price => price.FixedCostType == fixedPrice.FixedCostType))
            {
                fixedPrices.Remove(fixedPrices.Find(price => price.FixedCostType == fixedPrice.FixedCostType));
            }
        }

        public ReadOnlyCollection<FixedPrice> GetFixedPrices()
        {
            return fixedPrices.AsReadOnly();
        }

        public double GetTotalPrice()
        {
            double result = 0.0;
            if (Consumption != null && Tariff != null)
            {
                result += Consumption.ElectricityHigh * (Math.Min(Tariff.ElectricityHigh, Tariff.ElectricityCap));
                result -= Consumption.ReturnElectricityHigh * Tariff.ReturnElectricityHigh;
                result += Consumption.ElectricityLow * (Math.Min(Tariff.ElectricityLow, Tariff.ElectricityCap));
                result -= Consumption.ReturnElectricityLow * Tariff.ReturnElectricityLow;
                result += Consumption.Gas * (Math.Min(Tariff.Gas, Tariff.GasCap));
            }

            result += Math.Round(fixedPrices.Sum(fixedPrice => fixedPrice.Price), 2);

            return result;
        }
    }
}
