﻿using System.ComponentModel;

namespace EnergyCostTool.Data;

public enum FixedCostTariffType
{
    Daily = 0,
    Monthly = 1,
    Yearly = 3,
    MonthlyCanBeZero = 4
}

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

public class FixedCost
{
    public DateTime StartDate { get; set; }
    public FixedCostType CostType { get; set; }
    public double Price { get; set; }
    public FixedCostTariffType TariffType { get; set; }

    // For JSON Serialization
    public FixedCost()
    {

    }

    public FixedCost(DateTime startDate, FixedCostType costType, double price)
    {
        StartDate = startDate;
        CostType = costType;
        Price = price;
        TariffType = costType switch
        {
            FixedCostType.StandingChargeElectricity => FixedCostTariffType.Monthly,
            FixedCostType.StandingChargeGas => FixedCostTariffType.Monthly,
            FixedCostType.MonthlyDeposit => FixedCostTariffType.Monthly,
            FixedCostType.TransportCostElectricity => FixedCostTariffType.Daily,
            FixedCostType.TransportCostGas => FixedCostTariffType.Daily,
            FixedCostType.DiscountOnEnergyTax => FixedCostTariffType.Daily,
            FixedCostType.SolarCost => FixedCostTariffType.MonthlyCanBeZero,
            _ => throw new ArgumentOutOfRangeException(nameof(costType), costType, null)
        };
    }
}