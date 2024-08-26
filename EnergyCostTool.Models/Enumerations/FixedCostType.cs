using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyCostTool.Models.Enumerations
{
    public enum FixedCostType
    {
        [Description("Vastrecht Electra")] StandingChargeElectricity,
        [Description("Vastrecht Gas")] StandingChargeGas,
        [Description("Transportkosten Elektra")] TransportCostElectricity,
        [Description("Transportkosten Gas")] TransportCostGas,
        [Description("Korting op Energiebelasting")] DiscountOnEnergyTax,
        [Description("Maandelijks voorschot")] MonthlyDeposit,
        [Description("Terugleverkosten")] SolarCost,
    }

}
