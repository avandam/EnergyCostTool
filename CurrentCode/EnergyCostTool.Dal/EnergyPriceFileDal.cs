using EnergyCostTool.Dal.DataModels;
using EnergyCostTool.Models.Exceptions;
using System.Text.Json;

namespace EnergyCostTool.Dal
{
    public class EnergyPriceFileDal
    {
        private const string filename = "energyPrices.dat";

        internal static void Save(List<EnergyPrice> energyPrices)
        {
            try
            {
                File.WriteAllText(filename, JsonSerializer.Serialize(energyPrices));
            }
            catch (Exception e)
            {
                throw new FileException("Could not save energy prices", e);
            }
        }

        internal static List<EnergyPrice> Load()
        {
            if (!File.Exists(filename))
            {
                throw new FileException("Could not find database file");
            }

            try
            {
                return JsonSerializer.Deserialize<List<EnergyPrice>>(File.ReadAllText(filename)) ?? throw new FileException("File was empty");
            }
            catch (Exception e)
            {
                throw new FileException("Could not load energy prices", e);
            }
        }
    }
}
