using System.IO;
using System.Text.Json;
using EnergyCostTool.Exceptions;

namespace EnergyCostTool.Data;

public class FixedCostCollection
{
    private List<FixedCost> fixedCosts;
    private const string filename = "fixedCosts.dat";

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

    public void Delete(DateTime startDate, FixedCostType costType)
    {
        if (!fixedCosts.Exists(price => price.StartDate == startDate && price.CostType == costType))
        {
            throw new FixedCostDoesNotExistException($"Cannot delete: FixedCost for date {startDate.ToShortDateString()} and type {costType} does not exist");
        }
        fixedCosts.Remove(Get(startDate, costType));
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

    public List<FixedCost> Get()
    {
        return fixedCosts;
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

    public List<FixedCost> Get(int year, FixedCostType costType)
    {
        List<FixedCost> result = new List<FixedCost>();
        if (fixedCosts.Count == 0)
        {
            return result;
        }
        FixedCost oldCost = fixedCosts.FindLast(fixedCost => fixedCost.StartDate.Year < year && fixedCost.CostType == costType);
        if (oldCost != null)
        {
            result.Add(oldCost);
        }
        result.AddRange(fixedCosts.FindAll(fixedCost => fixedCost.StartDate.Year == year && fixedCost.CostType  == costType));
        return result;
    }

    public bool ContainsDataFor(DateTime date, FixedCostType costType)
    {
        return fixedCosts.Exists(consumption => consumption.StartDate == date && consumption.CostType == costType);
    }


    internal int Count()
    {
        return fixedCosts.Count;
    }

    public void Save()
    {
        try
        {
            File.WriteAllText(filename, JsonSerializer.Serialize(fixedCosts));
        }
        catch (Exception e)
        {
            throw new FileException("Could not save fixed costs", e);
        }
    }

    private void Load()
    {
        if (File.Exists(filename))
        {
            try
            {
                fixedCosts = JsonSerializer.Deserialize<List<FixedCost>>(File.ReadAllText(filename)) ?? throw new FileException("File was empty");
            }
            catch (Exception e)
            {
                throw new FileException("Could not load fixed costs", e);
            }
        }
    }
}