namespace EnergyCostTool.Models.Exceptions;

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