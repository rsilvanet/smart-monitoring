using SmartMonitoring.Domain.ValueObjects;

namespace SmartMonitoring.Domain.Exceptions
{
    public class InvalidLabelException : DomainException
    {
        private static readonly string MESSAGE = $"The labels must follow the convention key:value ({Label.REGEX}).";

        public InvalidLabelException() : base(MESSAGE) { }
    }
}
