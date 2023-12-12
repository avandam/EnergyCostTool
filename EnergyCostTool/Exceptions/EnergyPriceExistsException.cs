namespace EnergyCostTool.Exceptions;

public class EnergyPriceExistsException : Exception
{
    public EnergyPriceExistsException(string message)
        : base(message)
    {
    }

    public EnergyPriceExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}