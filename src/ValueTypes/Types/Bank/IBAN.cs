using System;
using System.Collections.Generic;
using System.Numerics;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Types.Bank
{
    /// <summary>
    /// Value type for IBANs.l
    /// </summary>
    /// <seealso cref="IValueType&lt;string, IBAN&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIbanException"></exception>
    public readonly record struct IBAN : IValueType<string, IBAN>
    {
        #region fields

        private readonly string _value;
        private const string Default = "XY000";

        #endregion

        #region properties

        public static IBAN Empty => new();
        public string CountryCode => _value[..2];
        public int Checksum => int.Parse(_value[2..4]);
        public string AccountIdentifier => _value[4..];

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            TooShort,
            TooLong,
            InvalidCountryCode,
            InvalidChecksum,
            InvalidAccountIdentifier,
            UnknownError
        }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IBAN"/> struct.
        /// </summary>
        public IBAN() => _value = Default;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IBAN"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidIbanException"></exception>
        public IBAN(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.TooShort => new InvalidIbanException($"The value '{value}' is too short for an IBAN!"),
                    Validation.TooLong => new InvalidIbanException($"The value '{value}' is too long for an IBAN!"),
                    Validation.InvalidCountryCode => new InvalidIbanException($"The IBAN '{value}' has no valid country code part!"),
                    Validation.InvalidAccountIdentifier => new InvalidIbanException($"The IBAN '{value}' has no valid account number part!"),
                    _ => new InvalidIbanException(),
                };
            }

            _value = value.ToUpper();
        }
        
        // required for internal initialization
        private IBAN(ref string value) => _value = value.ToUpper();

        #endregion

        #region operator

        public static bool operator ==(IBAN left, string right) => left.Equals(right);
        public static bool operator !=(IBAN left, string right) => !left.Equals(right);

        public static implicit operator string(IBAN iban) => iban._value;
        public static implicit operator IBAN(string value) => new(value);

        #endregion

        #region public methods

        public static IBAN From(string value) => new(value);
        
        public static Validation TryFrom(string value, out IBAN output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new IBAN(ref value);
                    return Validation.Ok;
                }

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

            if (value.Length < 5)
                return Validation.TooShort;

            if (value.Length > 34)
                return Validation.TooLong   ;

            var span = value.AsSpan();

            if (!ContainsValidCountryCode(ref span))
                return Validation.InvalidCountryCode;

            if (!ContainsValidChecksum(ref span))
                return Validation.InvalidChecksum;

            if (!ContainsValidAccountIdentifier(ref span))
                return Validation.InvalidAccountIdentifier;

            if (!ValidateChecksum(ref span))
                return Validation.InvalidChecksum;

            return Validation.Ok;
        }

        private static bool ContainsValidCountryCode(ref ReadOnlySpan<char> span)
        {
            for (var i = 0; i < 2; i++)
            {
                if (!IsLetter(span[i]))
                    return false;
            }

            return true;
        }

        private static bool ContainsValidChecksum(ref ReadOnlySpan<char> span)
        {
            for (var i = 2; i < 4; i++)
            {
                if (!IsDigit(span[i]))
                    return false;
            }

            return true;
        }

        private static bool ContainsValidAccountIdentifier(ref ReadOnlySpan<char> span)
        {
            for (var i = 4; i < span.Length; i++)
            {
                if (!IsLetter(span[i]) && !IsDigit(span[i]))
                    return false;
            }

            return true;
        }

        private static bool ValidateChecksum(ref ReadOnlySpan<char> span)
        {
            // 1. move first 4 characters to the end of the string
            var iban = (span[4..].ToString() + span[0..4].ToString()).ToUpper();

            // 2. loop through chars and replace letters with alphabet order number + 10
            var temp = "";
            var length = 0;
            for (var i = 0; i < iban.Length; i++)
            {
                if (IsUppercaseLetter(iban[i]))
                {
                    temp += iban.Substring(i - length, length);
                    temp += (iban[i] - 55).ToString();
                    length = 0;
                }
                else
                {
                    length++;
                }
            }
            if (length > 0)
            {
                temp += iban.Substring(iban.Length - length, length);
            }

            // 3. cast to integer
            if (!BigInteger.TryParse(temp, out var number))
                return false;

            // 4. modulo 97 must be 1!
            if (number % 97 != 1)
                return false;

            return true;
        }

        private static bool IsUppercaseLetter(char c) => c is >= 'A' and <= 'Z';

        private static bool IsLowercaseLetter(char c) => c is >= 'a' and <= 'z';

        private static bool IsLetter(char c) => IsLowercaseLetter(c) || IsUppercaseLetter(c);

        private static bool IsDigit(char c) => c is >= '0' and <= '9';

        #endregion
    }

    public class InvalidIbanException : Exception
    {
        public InvalidIbanException()
        {
        }

        public InvalidIbanException(string message) : base(message)
        {
        }
    }
}
