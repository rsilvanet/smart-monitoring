using SmartMonitoring.Domain.Exceptions;
using System.Net.Mail;

namespace SmartMonitoring.Domain.ValueObjects
{
    public struct Email
    {
        private readonly string _value;

        public Email(string value)
        {
            value = value?.Trim();

            if (!IsValid(value))
            {
                throw new InvalidEmailException();
            }

            _value = value.Trim();
        }

        private static bool IsValid(string value)
        {
            try
            {
                return new MailAddress(value).Address == value;
            }
            catch
            {
                return false;
            }
        }

        public static implicit operator Email(string value)
        {
            return new Email(value);
        }

        public static implicit operator string(Email email)
        {
            return email._value;
        }
    }
}
