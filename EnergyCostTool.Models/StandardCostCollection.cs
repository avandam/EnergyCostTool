using System.Collections.ObjectModel;
using EnergyCostTool.Models.Enumerations;
using EnergyCostTool.Models.Exceptions;

namespace EnergyCostTool.Models;

public class StandardCostCollection
{
    private List<StandardCost> standardCosts;

    public StandardCostCollection()
    {
        standardCosts = new List<StandardCost>();
    }

    public StandardCostCollection(List<StandardCost> standardCosts)
    {
        this.standardCosts = standardCosts;
    }

    public void AddOrUpdate(StandardCost standardCost)
    {
        if (standardCosts.Exists(price => price.StartDate == standardCost.StartDate && price.CostType == standardCost.CostType))
        {
            standardCosts.Remove(standardCosts.First(price => price.StartDate == standardCost.StartDate && price.CostType == standardCost.CostType));
        }

        standardCosts.Add(standardCost);
        standardCosts = standardCosts.OrderBy(cost => cost.CostType).ThenBy(cost => cost.StartDate).ToList();
    }

    public void Delete(StandardCost standardCost)
    {
        if (!standardCosts.Exists(price => price.StartDate == standardCost.StartDate && price.CostType == standardCost.CostType))
        {
            throw new StandardCostDoesNotExistException($"Cannot delete: FixedCost for date {standardCost.StartDate.ToShortDateString()} and type {standardCost.CostType} does not exist");
        }
        standardCosts.Remove(standardCosts.First(price => price.StartDate == standardCost.StartDate && price.CostType == standardCost.CostType));
    }

    public ReadOnlyCollection<StandardCost> Get()
    {
        return standardCosts.AsReadOnly();
    }

    public ReadOnlyCollection<StandardCost> Get(FixedCostType costType)
    {
        if (standardCosts.Count == 0)
        {
            return new List<StandardCost>().AsReadOnly();
        }
        return standardCosts.FindAll(standardCost => standardCost.CostType == costType).AsReadOnly();
    }

    public StandardCost Get(DateTime searchDate, FixedCostType costType)
    {
        if (standardCosts.Count == 0)
        {
            throw new StandardCostNotFoundException("There are no fixed costs available.");
        }
        if (searchDate < standardCosts.First(standardCost => standardCost.CostType == costType).StartDate)
        {
            throw new StandardCostNotFoundException($"The given date {searchDate.ToShortDateString()} is before the first available fixed cost.");
        }

        if (searchDate >= standardCosts.Last(standardCost => standardCost.CostType == costType).StartDate)
        {
            return standardCosts.Last(standardCost => standardCost.CostType == costType);
        }

        return standardCosts.Last(standardCost => standardCost.StartDate <= searchDate && standardCost.CostType == costType);
    }

    public List<StandardCost> Get(int year, FixedCostType costType)
    {
        List<StandardCost> result = new List<StandardCost>();
        if (standardCosts.Count == 0)
        {
            return result;
        }
        StandardCost oldCost = standardCosts.FindLast(standardCost => standardCost.StartDate.Year < year && standardCost.CostType == costType);
        if (oldCost != null)
        {
            result.Add(oldCost);
        }
        result.AddRange(standardCosts.FindAll(standardCost => standardCost.StartDate.Year == year && standardCost.CostType  == costType));
        return result;
    }

    public bool ContainsDataFor(DateTime date, FixedCostType costType)
    {
        return standardCosts.Exists(standardCost => standardCost.StartDate == date && standardCost.CostType == costType);
    }
}