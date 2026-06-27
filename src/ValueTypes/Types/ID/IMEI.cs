using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.ID
{
    /// <summary>
    /// Value type for International Mobile Equipment Identity (IMEI).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidImeiException"></exception>
    public readonly record struct IMEI : IValueType<string, IMEI>
    {
        #region fields

        private readonly string _value;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            TooShort,
            TooLong,
            IllegalCharacter,
            IncorrectCheckDigit,
            UnknownError
        }

        #endregion

        #region properties

        public static IMEI Default => new();

        public string TypeAllocationCode => _value[..8];
        public string SerialNumber => _value[9..14];
        public string CheckDigit => _value[14..15];

        #endregion

        #region constructor

        public IMEI() => _value = "000000000000000";
        public IMEI(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.TooShort => new InvalidImeiException($"The value '{value}' is too short!"),
                    Validation.TooLong => new InvalidImeiException($"The value '{value}' is too long!"),
                    Validation.IllegalCharacter => new InvalidImeiException($"The value '{value}' contains an illegal character!"),
                    Validation.IncorrectCheckDigit => new InvalidImeiException($"The checksum of the IMEI '{value}' is incorrect!"),
                    _ => new InvalidImeiException(),
                };
            }

            _value = value;
        }
        private IMEI(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(IMEI left, string right) => left.Equals(right);
        public static bool operator !=(IMEI left, string right) => !left.Equals(right);

        public static implicit operator string(IMEI imei) => imei._value;
        public static implicit operator IMEI(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static IMEI New() => new();

        public static IMEI From(string value) => new(value);
        public static Validation TryFrom(string value, out IMEI output)
        {
            var result = Validation.Ok;

            try
            {
                result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new IMEI(ref value);
                else
                    output = Default;

                return result;
            }
            catch (Exception)
            {
                output = Default;
                return result;
            }
        }

        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;

            if (value.Length < 15)
                return Validation.TooShort;

            if (value.Length > 15)
                return Validation.TooLong;

            if (!ValidateCharacters(ref value))
                return Validation.IllegalCharacter;

            if (!ValidateCheckDigit(ref value))
                return Validation.IncorrectCheckDigit;

            return Validation.Ok;
        }

        private static bool ValidateCharacters(ref string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigit(value[i]))
                    return false;
            }

            return true;
        }

        private static bool ValidateCheckDigit(ref string value)
        {
            int sum = 0;
            for (int i = 0; i <= 13; i += 1)
            {
                int digit = value[i] - '0';
                
                if ((i & 1) == 1)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
            }

            sum += value[14] - '0';
            return sum % 10 == 0;
        }

        private static bool IsDigit(char c) => c is >= '0' and <= '9';

        #endregion
    }

    public class InvalidImeiException : Exception
    {
        public InvalidImeiException()
        {
        }

        public InvalidImeiException(string message) : base(message)
        {
        }
    }
}
