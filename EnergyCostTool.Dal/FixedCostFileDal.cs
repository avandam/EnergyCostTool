using EnergyCostTool.Dal.DataModels;
using EnergyCostTool.Models.Exceptions;
using System.Text.Json;

namespace EnergyCostTool.Dal
{
    public class FixedCostFileDal
    {
        private const string filename = "fixedCosts.dat";

        internal static void Save(List<FixedCost> fixedCosts)
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

        internal static List<FixedCost> Load()
        {
            if (!File.Exists(filename))
            {
                throw new FileException("Could not find database file");
            }

            try
            {
                return JsonSerializer.Deserialize<List<FixedCost>>(File.ReadAllText(filename)) ?? throw new FileException("File was empty");
            }
            catch (Exception e)
            {
                throw new FileException("Could not load energy prices", e);
            }
        }
    }
}
