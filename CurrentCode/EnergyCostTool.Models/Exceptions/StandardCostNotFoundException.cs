namespace EnergyCostTool.Models.Exceptions;

public class StandardCostNotFoundException : Exception
{
    public StandardCostNotFoundException(string message)
        : base(message)
    {
    }

    public StandardCostNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}