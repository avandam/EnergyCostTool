using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyCostTool.Data
{
    internal class EnergyViewModel
    {
        public EnergyConsumptionCollection EnergyConsumptionCollection { get; private set; }
        public EnergyPriceCollection EnergyPriceCollection { get; private set; }
        public FixedCostCollection FixedCostCollection { get; private set; }

        public EnergyViewModel()
        {
            EnergyConsumptionCollection = new EnergyConsumptionCollection();
            EnergyPriceCollection = new EnergyPriceCollection();
            FixedCostCollection = new FixedCostCollection();
        }
    }
}
