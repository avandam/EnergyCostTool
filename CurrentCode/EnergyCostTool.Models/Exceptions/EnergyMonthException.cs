namespace EnergyCostTool.Models.Exceptions;

public class EnergyMonthException : Exception
{
    public EnergyMonthException(string message)
        : base(message)
    {
    }

    public EnergyMonthException(string message, Exception inner)
        : base(message, inner)
    {
    }
}