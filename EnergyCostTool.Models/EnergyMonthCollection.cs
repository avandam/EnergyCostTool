using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.Models.Exceptions;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace EnergyCostTool.Models
{
    public class EnergyMonthCollection
    {
        private List<EnergyMonth> energyMonths;

        public EnergyMonthCollection()
        {
            energyMonths = new List<EnergyMonth>();
        }

        public EnergyMonthCollection(List<EnergyMonth> energyMonths)
        {
            this.energyMonths = energyMonths;
        }

        public ReadOnlyCollection<EnergyMonth> Get()
        {
            return energyMonths.OrderByDescending(energyMonth => energyMonth.Month).ToList().AsReadOnly();
        }

        public void AddOrUpdateEnergyMonth(DateTime month, Consumption consumption)
        {
            if (!energyMonths.Exists(eMonth => eMonth.Month == month))
            {
                energyMonths.Add(new EnergyMonth(month, consumption));
            }
            else
            { 
                energyMonths.Find(eMonth => eMonth.Month == month).AddOrUpdate(consumption);
            }
            energyMonths.OrderByDescending(eMonth => eMonth.Month);
        }
        
        public void AddOrUpdateEnergyMonth(DateTime month, Tariff tariff)
        {
            if (!energyMonths.Exists(eMonth => eMonth.Month == month))
            {
                energyMonths.Add(new EnergyMonth(month, tariff));
            }
            else
            {
                energyMonths.Find(eMonth => eMonth.Month == month).AddOrUpdate(tariff);
            }
        }

        public void AddOrUpdateEnergyMonth(DateTime month, FixedPrice price)
        {
            if (!energyMonths.Exists(eMonth => eMonth.Month == month))
            {
                energyMonths.Add(new EnergyMonth(month, price));
            }
            else
            {
                energyMonths.Find(eMonth => eMonth.Month == month).AddOrUpdate(price);
            }
        }

        public void DeleteEnergyMonth(DateTime month)
        {
            if (!energyMonths.Exists(eMonth => eMonth.Month == month))
            {
                throw new EnergyMonthNotFoundException($"EnergyMonth {month.Month}:{month.Year} not found");
            }
            energyMonths.Remove(energyMonths.Find(eMonth => eMonth.Month == month));
        }
    }
}
