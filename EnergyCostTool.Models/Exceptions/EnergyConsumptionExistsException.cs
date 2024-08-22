namespace EnergyCostTool.Models.Exceptions;

public class EnergyConsumptionExistsException : Exception
{
    public EnergyConsumptionExistsException(string message)
        : base(message)
    {
    }

    public EnergyConsumptionExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}