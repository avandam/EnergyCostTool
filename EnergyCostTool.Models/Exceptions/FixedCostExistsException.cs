namespace EnergyCostTool.Models.Exceptions;

public class FixedCostExistsException : Exception
{
    public FixedCostExistsException(string message)
        : base(message)
    {
    }

    public FixedCostExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}