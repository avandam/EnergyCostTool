namespace EnergyCostTool.Exceptions;

public class EnergyPriceNotFoundException : Exception
{
    public EnergyPriceNotFoundException(string message)
        : base(message)
    {
    }

    public EnergyPriceNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}