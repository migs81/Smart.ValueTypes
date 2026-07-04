using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.ID
{
    /// <summary>
    /// Value type for International Mobile Subscriber Identity (IMSI).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidImsiException"></exception>
    public readonly record struct IMSI : IValueType<string, IMSI>
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
            UnknownError
        }

        #endregion

        #region properties

        public static IMSI Empty => new();

        public string MobileCountryCode => _value[..3];

        #endregion

        #region constructor

        public IMSI() => _value = "000000";
        public IMSI(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.TooShort => new InvalidImsiException($"The value '{value}' is too short!"),
                    Validation.TooLong => new InvalidImsiException($"The value '{value}' is too long!"),
                    Validation.IllegalCharacter => new InvalidImsiException($"The value '{value}' contains an illegal character!"),
                    _ => new InvalidImsiException(),
                };
            }

            _value = value;
        }
        private IMSI(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(IMSI left, string right) => left.Equals(right);
        public static bool operator !=(IMSI left, string right) => !left.Equals(right);

        public static implicit operator string(IMSI imsi) => imsi._value;
        public static implicit operator IMSI(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static IMSI New() => new();

        public static IMSI From(string value) => new(value);
        public static Validation TryFrom(string value, out IMSI output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new IMSI(ref value);
                else
                    output = Empty;

                return result;
            }
            catch (Exception)
            {
                output = Empty;
                return Validation.UnknownError;
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

            if (value.Length < 6)
                return Validation.TooShort;

            if (value.Length > 15)
                return Validation.TooLong;

            if (!ValidateCharacters(ref value))
                return Validation.IllegalCharacter;

            return Validation.Ok;
        }

        private static bool ValidateCharacters(ref string value)
        {
            foreach (var c in value)
            {
                if (!IsDigit(c))
                    return false;
            }

            return true;
        }

        private static bool IsDigit(char c) => c is >= '0' and <= '9';

        #endregion
    }

    public class InvalidImsiException : Exception
    {
        public InvalidImsiException()
        {
        }

        public InvalidImsiException(string message) : base(message)
        {
        }
    }
}
