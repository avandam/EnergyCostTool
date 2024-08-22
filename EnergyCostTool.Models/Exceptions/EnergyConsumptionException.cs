namespace EnergyCostTool.Models.Exceptions;

public class EnergyConsumptionException : Exception
{
    public EnergyConsumptionException(string message)
        : base(message)
    {
    }

    public EnergyConsumptionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}