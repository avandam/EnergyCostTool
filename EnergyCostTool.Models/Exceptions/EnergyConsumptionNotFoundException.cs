namespace EnergyCostTool.Models.Exceptions;

public class EnergyConsumptionNotFoundException : Exception
{
    public EnergyConsumptionNotFoundException(string message)
        : base(message)
    {
    }

    public EnergyConsumptionNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}