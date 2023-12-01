namespace EnergyCostTool.Data
{
    public class EnergyMonth
    {
        public int Year { get; private set; }
        public int Month { get; private set; }

        public List<FixedCost> FixedCosts { get; private set; }
        public Usage Usage { get; private set; }
        public Tariff Tariff { get; private set; }
        public TariffCap TariffCap { get; private set; }

        public EnergyMonth(int year, int month, List<FixedCost> fixedCosts, Usage usage, Tariff tariff, TariffCap tariffCap)
        {
            Year = year;
            Month = month;
            FixedCosts = fixedCosts;
            Usage = usage;
            Tariff = tariff;
            TariffCap = tariffCap;
        }

        public EnergyMonth(int year, int month, List<FixedCost> fixedCosts, Usage usage, Tariff tariff)
        {
            Year = year;
            Month = month;
            FixedCosts = fixedCosts;
            Usage = usage;
            Tariff = tariff;
            TariffCap = new TariffCap(decimal.MaxValue, decimal.MaxValue, int.MaxValue);
        }

    }
}
