namespace EnergyCostTool.Models.Exceptions;

public class StandardCostDoesNotExistException : Exception
{
    public StandardCostDoesNotExistException(string message)
        : base(message)
    {
    }

    public StandardCostDoesNotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}