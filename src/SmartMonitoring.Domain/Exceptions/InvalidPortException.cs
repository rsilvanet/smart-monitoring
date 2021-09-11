using SmartMonitoring.Domain.ValueObjects;

namespace SmartMonitoring.Domain.Exceptions
{
    public class InvalidPortException : DomainException
    {
        private static readonly string MESSAGE = $"The port number must be a number from {Port.MIN_PORT_NUMBER} to {Port.MAX_PORT_NUMBER}.";

        public InvalidPortException() : base(MESSAGE) { }
    }
}
