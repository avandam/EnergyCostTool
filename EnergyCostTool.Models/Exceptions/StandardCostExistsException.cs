namespace EnergyCostTool.Models.Exceptions;

public class StandardCostExistsException : Exception
{
    public StandardCostExistsException(string message)
        : base(message)
    {
    }

    public StandardCostExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}