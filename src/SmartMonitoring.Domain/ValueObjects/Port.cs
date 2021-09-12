using SmartMonitoring.Domain.Exceptions;

namespace SmartMonitoring.Domain.ValueObjects
{
    public struct Port
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

        private static bool IsValid(int value)
        {
            return value >= MIN_PORT_NUMBER && value <= MAX_PORT_NUMBER;
        }

        public static implicit operator Port(int value)
        {
            return new Port(value);
        }

        public static implicit operator int(Port port)
        {
            return port._value;
        }
    }
}
