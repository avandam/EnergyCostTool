namespace EnergyCostTool.Exceptions;

public class FixedCostDoesNotExistException : Exception
{
    public FixedCostDoesNotExistException(string message)
        : base(message)
    {
    }

    public FixedCostDoesNotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}