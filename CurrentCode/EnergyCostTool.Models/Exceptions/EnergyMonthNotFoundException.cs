namespace EnergyCostTool.Models.Exceptions;

public class EnergyMonthNotFoundException : Exception
{
    public EnergyMonthNotFoundException(string message)
        : base(message)
    {
    }

    public EnergyMonthNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}