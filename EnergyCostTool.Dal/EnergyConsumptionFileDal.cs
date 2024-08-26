using EnergyCostTool.Dal.DataModels;
using EnergyCostTool.Models.Exceptions;
using System.Text.Json;

namespace EnergyCostTool.Dal
{
    internal static class EnergyConsumptionFileDal
    {
        private const string filename = "energyConsumptions.dat";

        internal static void Save(List<EnergyConsumption> energyConsumptions)
        {
            try
            {
                File.WriteAllText(filename, JsonSerializer.Serialize(energyConsumptions));
            }
            catch (Exception e)
            {
                throw new FileException("Could not save energy consumptions", e);
            }
        }

        internal static List<EnergyConsumption> Load()
        {
            if (!File.Exists(filename))
            {
                throw new FileException("Could not find database file");
            }

            try
            {
                return JsonSerializer.Deserialize<List<EnergyConsumption>>(File.ReadAllText(filename)) ?? throw new FileException("File was empty");
            }
            catch (Exception e)
            {
                throw new FileException("Could not load energy consumptions", e);
            }
        }
    }
}
