using System.IO;
using System.Text.Json;
using EnergyCostTool.Exceptions;

namespace EnergyCostTool.Data
{
    public class FixedCostCollection
    {
        private List<FixedCost> fixedCosts;
        private const string Filename = "fixedCosts.dat";

        public FixedCostCollection()
        {
            fixedCosts = new List<FixedCost>();
            Load();
        }

        public void Add(FixedCost fixedCost)
        {
            if (fixedCosts.Exists(price => price.StartDate == fixedCost.StartDate && price.CostType == fixedCost.CostType))
            {
                throw new FixedCostExistsException($"This fixed cost for {fixedCost.CostType} and date {fixedCost.StartDate.ToShortDateString()} already exists");
            }

            fixedCosts.Add(fixedCost);
            fixedCosts = fixedCosts.OrderBy(cost => cost.CostType).ThenBy(cost => cost.StartDate).ToList();
        }

        public void Delete(FixedCost fixedCost)
        {
            if (!fixedCosts.Exists(price => price.StartDate == fixedCost.StartDate && price.CostType == fixedCost.CostType))
            {
                throw new FixedCostDoesNotExistException($"Cannot delete: FixedCost for date {fixedCost.StartDate.ToShortDateString()} does not exist");
            }
            fixedCosts.Remove(fixedCost);
        }

        public void Update(FixedCost fixedCost)
        {
            if (!fixedCosts.Exists(price => price.StartDate == fixedCost.StartDate && price.CostType == fixedCost.CostType))
            {
                throw new FixedCostDoesNotExistException($"Cannot update: FixedCost for date {fixedCost.StartDate.ToShortDateString()} does not exist");
            }

            fixedCosts.Remove(fixedCosts.First(price => price.StartDate == fixedCost.StartDate && price.CostType == fixedCost.CostType));
            Add(fixedCost);
        }

        public FixedCost Get(DateTime searchDate, FixedCostType costType)
        {
            if (fixedCosts.Count == 0)
            {
                throw new FixedCostNotFoundException("There are no fixed costs available.");
            }
            if (searchDate < fixedCosts.First(fixedCost => fixedCost.CostType == costType).StartDate)
            {
                throw new FixedCostNotFoundException($"The given date {searchDate.ToShortDateString()} is before the first available fixed cost.");
            }

            if (searchDate >= fixedCosts.Last(fixedCost => fixedCost.CostType == costType).StartDate)
            {
                return fixedCosts.Last(fixedCost => fixedCost.CostType == costType);
            }

            return fixedCosts.Last(price => price.StartDate <= searchDate && price.CostType == costType);
        }

        internal int Count()
        {
            return fixedCosts.Count;
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(Filename, JsonSerializer.Serialize(fixedCosts));
            }
            catch (Exception e)
            {
                throw new FileException("Could not save fixed costs", e);
            }
        }

        private void Load()
        {
            if (File.Exists(Filename))
            {
                try
                {
                    fixedCosts = JsonSerializer.Deserialize<List<FixedCost>>(File.ReadAllText(Filename)) ?? throw new FileException("File was empty");
                }
                catch (Exception e)
                {
                    throw new FileException("Could not load fixed costs", e);
                }
            }
        }
    }
}
