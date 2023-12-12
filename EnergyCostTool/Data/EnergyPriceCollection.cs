using System.IO;
using EnergyCostTool.Exceptions;
using System.Text.Json;

namespace EnergyCostTool.Data;

public class EnergyPriceCollection
{
    private List<EnergyPrice> energyPrices;
    private const string filename = "energyPrices.dat";

    public EnergyPriceCollection()
    {
        energyPrices = new List<EnergyPrice>();
        Load();
    }

    public void Add(EnergyPrice energyPrice)
    {
        if (energyPrices.Exists(price => price.StartDate == energyPrice.StartDate))
        {
            throw new EnergyPriceExistsException($"Cannot add: EnergyPrice for date {energyPrice.StartDate.ToShortDateString()} already exists");
        }

        energyPrices.Add(energyPrice);
        energyPrices = energyPrices.OrderBy(price => price.StartDate).ToList();
    }

    public void Delete(DateTime startDate)
    {
        if (!energyPrices.Exists(price => price.StartDate == startDate))
        {
            throw new EnergyPriceDoesNotExistException($"Cannot delete: EnergyPrice for date {startDate.ToShortDateString()} does not exist");
        }
        energyPrices.Remove(Get(startDate));
    }

    public void Update(EnergyPrice energyPrice)
    {
        if (!energyPrices.Exists(price => price.StartDate == energyPrice.StartDate))
        {
            throw new EnergyPriceDoesNotExistException($"Cannot update: EnergyPrice for date {energyPrice.StartDate.ToShortDateString()} does not exist");
        }

        energyPrices.Remove(energyPrices.First(price => price.StartDate == energyPrice.StartDate));
        Add(energyPrice);
    }


    public List<EnergyPrice> Get()
    {
        return energyPrices;
    }

    public EnergyPrice Get(DateTime searchDate)
    {
        if (energyPrices.Count == 0)
        {
            throw new EnergyPriceNotFoundException("There are no energy prices available.");
        }
        if (searchDate < energyPrices.First().StartDate)
        {
            throw new EnergyPriceNotFoundException($"The given date {searchDate.ToShortDateString()} is before the first available energy price.");
        }

        if (searchDate >= energyPrices.Last().StartDate)
        {
            return energyPrices.Last();
        }

        return energyPrices.Last(price => price.StartDate <= searchDate);
    }

    public bool ContainsDataFor(DateTime date)
    {
        return energyPrices.Exists(consumption => consumption.StartDate == date);
    }


    internal int Count()
    {
        return energyPrices.Count;
    }

    public void Save()
    {
        try
        {
            File.WriteAllText(filename, JsonSerializer.Serialize(energyPrices));
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
                energyPrices = JsonSerializer.Deserialize<List<EnergyPrice>>(File.ReadAllText(filename)) ?? throw new FileException("File was empty");
            }
            catch (Exception e)
            {
                throw new FileException("Could not load energy prices", e);
            }
        }
    }

}