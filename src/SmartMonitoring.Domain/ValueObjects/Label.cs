﻿using SmartMonitoring.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SmartMonitoring.Domain.ValueObjects
{
    public struct Label
    {
        public const string REGEX = "^[a-zA-Z0-9]+:[a-zA-Z0-9]+$";

        private readonly string _value;

        public Label(string value)
        {
            value = value?.Trim();

            if (!IsValid(value))
            {
                throw new InvalidLabelException();
            }

            _value = value;
        }

        private static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var regex = new Regex(REGEX);
            var match = regex.Match(value);

            return match.Success;
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator Label(string value)
        {
            return new Label(value);
        }

        public static implicit operator string(Label label)
        {
            return label._value;
        }
    }
}
