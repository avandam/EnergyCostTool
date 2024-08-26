using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyCostTool.Models.Enumerations
{
    public enum FixedCostTariffType
    {
        Daily = 0,
        Monthly = 1,
        Yearly = 3,
        MonthlyCanBeZero = 4
    }
}
