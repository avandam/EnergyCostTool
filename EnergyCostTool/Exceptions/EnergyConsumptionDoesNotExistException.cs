namespace EnergyCostTool.Exceptions
{
    public class EnergyConsumptionDoesNotExistException : Exception
    {
        public EnergyConsumptionDoesNotExistException(string message)
            : base(message)
        {
        }

        public EnergyConsumptionDoesNotExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
