﻿using System.IO;
using EnergyCostTool.Exceptions;
using System.Text.Json;

namespace EnergyCostTool.Data
{
    public class EnergyConsumptionCollection
    {
        private List<EnergyConsumption> energyConsumptions;
        private const string Filename = "energyConsumptions.dat";

        public EnergyConsumptionCollection()
        {
            energyConsumptions = new List<EnergyConsumption>();
            Load();
        }

        public void Add(EnergyConsumption energyConsumption)
        {
            if (energyConsumptions.Exists(consumption => consumption.Month == energyConsumption.Month))
            {
                throw new EnergyConsumptionExistsException($"Cannot add: EnergyConsumption for {energyConsumption.Month.ToShortDateString()} already exists");
            }

            energyConsumptions.Add(energyConsumption);
            energyConsumptions = energyConsumptions.OrderBy(consumption => consumption.Month).ToList();
        }

        public void Delete(EnergyConsumption energyConsumption)
        {
            if (!energyConsumptions.Exists(consumption => consumption.Month == energyConsumption.Month))
            {
                throw new EnergyConsumptionDoesNotExistException($"Cannot delete: EnergyConsumption for month {energyConsumption.Month.ToShortDateString()} does not exist");
            }
            energyConsumptions.Remove(energyConsumption);
        }

        public void Update(EnergyConsumption energyConsumption)
        {
            if (!energyConsumptions.Exists(consumption => consumption.Month == energyConsumption.Month))
            {
                throw new EnergyConsumptionDoesNotExistException($"Cannot update: EnergyConsumption for month {energyConsumption.Month.ToShortDateString()} does not exist");
            }

            energyConsumptions.Remove(energyConsumptions.First(consumption => consumption.Month == energyConsumption.Month));
            Add(energyConsumption);
        }

        public EnergyConsumption Get(DateTime month)
        {
            if (energyConsumptions.Count == 0)
            {
                throw new EnergyConsumptionNotFoundException("There are no energy consumptions available.");
            }

            if (!energyConsumptions.Exists(consumption => consumption.Month == month))
            {
                throw new EnergyConsumptionNotFoundException($"Energy Consumption for month {month.ToShortDateString()} is not available.");
            }

            return energyConsumptions.First(consumption => consumption.Month == month);
        }

        internal int Count()
        {
            return energyConsumptions.Count;
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(Filename, JsonSerializer.Serialize(energyConsumptions));
            }
            catch (Exception e)
            {
                throw new FileException("Could not save energy consumptions", e);
            }
        }

        private void Load()
        {
            if (File.Exists(Filename))
            {
                try
                {
                    energyConsumptions = JsonSerializer.Deserialize<List<EnergyConsumption>>(File.ReadAllText(Filename)) ?? throw new FileException("File was empty");
                }
                catch (Exception e)
                {
                    throw new FileException("Could not load energy consumptions", e);
                }
            }
        }

    }
}
