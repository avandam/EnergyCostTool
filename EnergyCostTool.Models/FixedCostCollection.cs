using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using EnergyCostTool.Models.Exceptions;

namespace EnergyCostTool.Models;

public class FixedCostCollection
{
    private List<FixedCost> fixedCosts;

    public FixedCostCollection()
    {
        fixedCosts = new List<FixedCost>();
    }

    public FixedCostCollection(List<FixedCost> fixedCosts)
    {
        this.fixedCosts = fixedCosts;
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
            throw new FixedCostDoesNotExistException($"Cannot delete: FixedCost for date {fixedCost.StartDate.ToShortDateString()} and type {fixedCost.CostType} does not exist");
        }
        fixedCosts.Remove(fixedCosts.First(price => price.StartDate == fixedCost.StartDate && price.CostType == fixedCost.CostType));
    }

    public void Update(FixedCost fixedCost)
    {
        Delete(fixedCost);
        Add(fixedCost);
    }

    public ReadOnlyCollection<FixedCost> Get()
    {
        return fixedCosts.AsReadOnly();
    }

    public ReadOnlyCollection<FixedCost> Get(FixedCostType costType)
    {
        if (fixedCosts.Count == 0)
        {
            return new List<FixedCost>().AsReadOnly();
        }
        return fixedCosts.FindAll(fixedCost => fixedCost.CostType == costType).AsReadOnly();
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
}