namespace SmartMonitoring.Domain.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        private const string MESSAGE = "The email address must be valid.";

        public InvalidEmailException() : base(MESSAGE) { }
    }
}
