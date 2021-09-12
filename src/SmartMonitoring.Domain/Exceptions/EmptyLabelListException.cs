namespace SmartMonitoring.Domain.Exceptions
{
    public class EmptyLabelListException : DomainException
    {
        private static readonly string MESSAGE = $"The list of labels must contain at least one item.";

        public EmptyLabelListException() : base(MESSAGE) { }
    }
}
