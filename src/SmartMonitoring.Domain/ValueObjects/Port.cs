using SmartMonitoring.Domain.Exceptions;

namespace SmartMonitoring.Domain.ValueObjects
{
    public class Port
    {
        public const int MIN_PORT_NUMBER = 1;
        public const int MAX_PORT_NUMBER = 65535;

        private readonly int _value;

        public Port(int value)
        {
            if (!IsValid(value))
            {
                throw new InvalidPortException();
            }

            _value = value;
        }

        private bool IsValid(int value)
        {
            return value >= MIN_PORT_NUMBER && value <= MAX_PORT_NUMBER;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
