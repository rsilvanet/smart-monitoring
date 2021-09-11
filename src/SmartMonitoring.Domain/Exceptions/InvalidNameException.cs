using SmartMonitoring.Domain.ValueObjects;

namespace SmartMonitoring.Domain.Exceptions
{
    public class InvalidNameException : DomainException
    {
        private static readonly string MESSAGE = $"The service name must be a string from {Name.MIN_LENGTH} to {Name.MAX_LENGTH} characters.";

        public InvalidNameException() : base(MESSAGE) { }
    }
}
