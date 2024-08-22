namespace EnergyCostTool.Models.Exceptions;

public class EnergyPriceDoesNotExistException : Exception
{
    public EnergyPriceDoesNotExistException(string message)
        : base(message)
    {
    }

    public EnergyPriceDoesNotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}