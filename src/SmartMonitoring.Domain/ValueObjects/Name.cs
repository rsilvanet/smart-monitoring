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

        public override bool Equals(object obj)
        {
            if (obj is Name name)
                return _value.Equals(name._value);

            if (obj is string stringName)
                return _value.Equals(stringName);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator Name(string value)
        {
            return new Name(value);
        }

        public static implicit operator string(Name name)
        {
            return name._value;
        }
    }
}
