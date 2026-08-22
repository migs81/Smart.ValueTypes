using System;
using System.Numerics;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Bank
{
    /// <summary>
    /// Value type for IBANs.l
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIbanException"></exception>
    public readonly record struct IBAN : IValueType<string, IBAN>
    {
        #region fields

        private readonly string? _value;

        #endregion

        #region properties

        public bool IsDefault => _value is null;
        public string CountryCode => _value is not null ? _value[..2] : "";
        public int Checksum => _value is not null ? int.Parse(_value[2..4]) : -1;
        public string AccountIdentifier => _value is not null ? _value[4..] : "";

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

            _value = value;
        }
        
        // required for internal initialization
        private IBAN(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(IBAN left, string right) => left.Equals(right);
        public static bool operator !=(IBAN left, string right) => !left.Equals(right);

        public static implicit operator string(IBAN iban) => iban._value ?? "";
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

                output = default;
                return result;
            }
            catch (Exception)
            {
                output = default;
                return Validation.UnknownError;
            }
        }

        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            // ---------- general ----------
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;

            // ---------- length ----------
            if (value.Length < 5)
                return Validation.TooShort;

            if (value.Length > 34)
                return Validation.TooLong   ;

            var span = value.AsSpan();

            // ---------- country ----------
            for (var i = 0; i < 2; i++)
            {
                if (span[i] is (< 'a' or > 'z') and (< 'A' or > 'Z'))
                    return Validation.InvalidCountryCode;
            }

            // ------ checksum format ------
            if (!ValidateChecksumFormat(ref span))
                return Validation.InvalidChecksum;
            
            // ---------- account ----------
            if (!ValidateAccount(ref span))
                return Validation.InvalidAccountIdentifier;

            // ----- validate checksum -----
            if (!ValidateChecksum(ref span))
                return Validation.InvalidChecksum;

            return Validation.Ok;
        }

        private static bool ValidateChecksumFormat(ref ReadOnlySpan<char> span)
        {
            if (span[2] is < '0' or > '9' || span[3] is < '0' or > '9') // must be 2 digits!
                return false;

            return true;
        }
        
        private static bool ValidateAccount(ref ReadOnlySpan<char> span)
        {
            for (var i = 4; i < span.Length; i++)
            {
                if (span[i] is (< '0' or > '9') and (< 'a' or > 'z') and (< 'A' or > 'Z'))
                    return false;
            }

            return true;
        }
        
        private static bool ValidateChecksum(ref ReadOnlySpan<char> span)
        {
            // calculate the remainder of modulo 97
            var moduloResult = 0;
            
            // first, the characters starting from the 4th position
            moduloResult = CalculateModulo(ref span, 4, span.Length, moduloResult);
            
            // then the first 4 characters
            moduloResult = CalculateModulo(ref span, 0, 4, moduloResult);

            // a valid checksum must be 1!
            if (moduloResult != 1)
                return false;
            
            return true;
        }

        private static int CalculateModulo(ref ReadOnlySpan<char> span, int from, int to, int initialValue)
        {
            var num = 0;
            var result = initialValue;
            for (var i = from; i < to; i++)
            {
                // digit?
                if (span[i] is >= '0' and <= '9')
                {
                    result = (result * 10 + (span[i] - '0')) % 97;
                    continue;
                }
                
                // If letter => get position in the alphabet plus 10
                if (span[i] is >= 'a' and <= 'z') num = span[i] - 87;
                else if (span[i] is >= 'A' and <= 'Z') num = span[i] - 55;
                
                // need to process both digits
                var firstDigit = num / 10;
                var secondDigit = num % 10;
                result = (result * 10 + firstDigit) % 97;
                result = (result * 10 + secondDigit) % 97;
            }

            return result;
        }
        
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
