using EnergyCostTool.Dal.DataModels;
using EnergyCostTool.Models;

namespace EnergyCostTool.Dal
{
    public static class Database
    {
        public static EnergyMonthCollection GetEnergyConsumptions()
        {
            List<EnergyConsumption> energyConsumptions;
            try
            {
                energyConsumptions = EnergyConsumptionFileDal.Load();
            }
            catch // No Consumption file yet -> bypass exception
            {
                return new EnergyMonthCollection();
            }
            List<EnergyPrice> energyPrices = [];

            return ConvertToEnergyMonthCollection(energyConsumptions, energyPrices);
        }

        public static EnergyMonthCollection GetEnergyTariffs()
        {
            List<EnergyPrice> energyPrices;
            try
            {
                energyPrices = EnergyPriceFileDal.Load();
            }
            catch // No Price file yet -> bypass exception
            {
                return new EnergyMonthCollection();
            }
            List<EnergyConsumption> energyConsumptions = [];

            return ConvertToEnergyMonthCollection(energyConsumptions, energyPrices);
        }


        public static EnergyMonthCollection GetEnergyConsumptionForYear(int year)
        {
            List<EnergyConsumption> energyConsumptions;
            try
            {
                energyConsumptions = EnergyConsumptionFileDal.Load().Where(energyConsumption => energyConsumption.Month.Year == year).ToList();
            }
            catch // No Consumption file yet -> bypass exception
            {
                return new EnergyMonthCollection(); 
            }   
            List<EnergyPrice> energyPrices = [];

            return ConvertToEnergyMonthCollection(energyConsumptions, energyPrices);
        }

        public static EnergyMonthCollection GetEnergyMonths() 
        {
            List<EnergyConsumption> energyConsumptions = EnergyConsumptionFileDal.Load();
            List<EnergyPrice> energyPrices = EnergyPriceFileDal.Load();
            
            return ConvertToEnergyMonthCollection(energyConsumptions, energyPrices);
        }

        public static EnergyMonthCollection GetSolarEnergyMonths()
        {
            List<EnergyConsumption> energyConsumptions = EnergyConsumptionFileDal.Load().Where(energyConsumption => energyConsumption.SolarGeneration > 0).ToList();
            List<EnergyPrice> energyPrices = EnergyPriceFileDal.Load();

            return ConvertToSolarEnergyMonthCollection(energyConsumptions, energyPrices);
        }


        public static void SaveEnergyMonths(EnergyMonthCollection energyMonths)
        {
            (List<EnergyConsumption> consumptions, List<EnergyPrice> prices) energyInformation = ConvertFromEnergyMonthCollection(energyMonths);
            List<EnergyConsumption> energyConsumptions = energyInformation.consumptions;
            List<EnergyPrice> energyPrices = energyInformation.prices;

            EnergyConsumptionFileDal.Save(energyConsumptions);
            EnergyPriceFileDal.Save(energyPrices);
        }

        public static void SaveConsumptions(EnergyMonthCollection energyMonths)
        {
            (List<EnergyConsumption> consumptions, List<EnergyPrice> prices) energyInformation = ConvertFromEnergyMonthCollection(energyMonths);
            List<EnergyConsumption> energyConsumptions = energyInformation.consumptions;

            EnergyConsumptionFileDal.Save(energyConsumptions);
        }

        public static void SaveTariffs(EnergyMonthCollection energyMonths)
        {
            (List<EnergyConsumption> consumptions, List<EnergyPrice> prices) energyInformation = ConvertFromEnergyMonthCollection(energyMonths);
            List<EnergyPrice> energyPrices = energyInformation.prices;

            EnergyPriceFileDal.Save(energyPrices);
        }


        internal static EnergyMonthCollection ConvertToEnergyMonthCollection(List<EnergyConsumption> energyConsumptions, List<EnergyPrice> energyPrices)
        {
            EnergyMonthCollection result = new EnergyMonthCollection();

            foreach (EnergyConsumption energyConsumption in energyConsumptions)
            {
                result.AddOrUpdateEnergyMonth(energyConsumption.Month, energyConsumption.ConvertToConsumption());
            }

            foreach (EnergyPrice energyPrice in energyPrices)
            {
                result.AddOrUpdateEnergyMonth(energyPrice.StartDate, energyPrice.ConvertToTariff());
            }

            return result;
        }

        internal static EnergyMonthCollection ConvertToSolarEnergyMonthCollection(List<EnergyConsumption> energyConsumptions, List<EnergyPrice> energyPrices)
        {
            EnergyMonthCollection result = new EnergyMonthCollection();

            foreach (EnergyConsumption energyConsumption in energyConsumptions)
            {
                if (energyConsumption.SolarGeneration > 0)
                {
                    result.AddOrUpdateEnergyMonth(energyConsumption.Month, energyConsumption.ConvertToConsumption());
                }
            }

            foreach (EnergyPrice energyPrice in energyPrices)
            {
                result.AddTariff(energyPrice.StartDate, energyPrice.ConvertToTariff());
            }

            return result;
        }



        internal static (List<EnergyConsumption> consumptions, List<EnergyPrice> prices) ConvertFromEnergyMonthCollection(EnergyMonthCollection energyMonths)
        {
            List<EnergyConsumption> energyConsumptions = new List<EnergyConsumption>();
            List<EnergyPrice> energyPrices = new List<EnergyPrice>();

            foreach (EnergyMonth energyMonth in energyMonths.Get())
            {
                if (energyMonth.Consumption != null)
                {
                    energyConsumptions.Add(new EnergyConsumption(energyMonth.Month, energyMonth.Consumption));
                }
                if (energyMonth.Tariff != null)
                {
                    energyPrices.Add(new EnergyPrice(energyMonth.Month, energyMonth.Tariff));
                }    
            }

            return (energyConsumptions, energyPrices);
        }

        public static StandardCostCollection GetStandardCosts()
        {
            List<FixedCost> fixedCosts = FixedCostFileDal.Load();
            
            StandardCostCollection standardCostCollection = new StandardCostCollection();

            foreach (FixedCost fixedCost in fixedCosts)
            {
                standardCostCollection.AddOrUpdate(fixedCost.ConvertToStandardCost());
            }

            return standardCostCollection;
        }

        public static void SaveStandardCosts(StandardCostCollection standardCosts)
        {
            List<FixedCost> fixedCosts = new List<FixedCost>();

            foreach (StandardCost standardCost in standardCosts.Get()) 
            {
                fixedCosts.Add(new FixedCost(standardCost));
            }

            FixedCostFileDal.Save(fixedCosts);
        }

    }
}
