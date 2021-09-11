using SmartMonitoring.Domain.Exceptions;

namespace SmartMonitoring.Domain.ValueObjects
{
    public struct Name
    {
        public const short MIN_LENGTH = 4;
        public const short MAX_LENGTH = 30;

        private readonly string _value;

        public Name(string value)
        {
            value = value?.Trim();

            if (!IsValid(value))
            {
                throw new InvalidNameException();
            }

            _value = value.Trim();
        }

        private static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
