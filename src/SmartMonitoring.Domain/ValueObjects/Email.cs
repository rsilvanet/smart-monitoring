using SmartMonitoring.Domain.Exceptions;
using System.Net.Mail;

namespace SmartMonitoring.Domain.ValueObjects
{
    public class Email
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

        private bool IsValid(string value)
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

        public override string ToString()
        {
            return _value;
        }
    }
}
