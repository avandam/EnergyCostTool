namespace EnergyCostTool.Exceptions
{
    public class FixedCostNotFoundException : Exception
    {
        public FixedCostNotFoundException(string message)
            : base(message)
        {
        }

        public FixedCostNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
