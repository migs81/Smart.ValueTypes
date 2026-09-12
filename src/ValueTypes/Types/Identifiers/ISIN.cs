using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Identifiers
{
    /// <summary>
    /// Value type for International Securities Identification Number (ISIN - ISO 6166).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIsinException"></exception>
    public readonly record struct ISIN : IValueType<string, ISIN>
    {
        #region fields

        private readonly string? _value;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WrongLength,
            InvalidCountryCode,
            ContainsIllegalCharacter,
            InvalidCheckDigit,
            InvalidChecksum,
            UnknownError,
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        public string CountryCode => _value is not null ? _value[..2] : "";
        
        public string BasicNumber => _value is not null ? _value[2..11] : "";

        public int CheckDigit => _value is not null ? int.Parse(_value[^1].ToString()) : -1;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ISIN"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidIsinException"></exception>
        public ISIN(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.WrongLength => new InvalidIsinException($"The value '{value}' has the wrong length for an ISBN-13!"),
                    Validation.InvalidCountryCode => new InvalidIsinException($"The the value '{value}' has no valid country code!"),
                    Validation.ContainsIllegalCharacter => new InvalidIsinException($"The value '{value}' contains an illegal character!"),
                    Validation.InvalidCheckDigit => new InvalidIsinException($"The check digit of the value '{value}' is invalid!"),
                    Validation.InvalidChecksum => new InvalidIsinException($"The checksum of the value '{value}' is wrong!"),
                    _ => new InvalidIsinException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private ISIN(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(ISIN left, string right) => left.Equals(right);
        public static bool operator !=(ISIN left, string right) => !left.Equals(right);

        public static implicit operator string(ISIN isbn) => isbn._value ?? "";
        public static implicit operator ISIN(string value) => new(value);

        #endregion

        #region public methods

        public static ISIN From(string value) => new(value);
        
        public static Validation TryFrom(string value, out ISIN output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new ISIN(ref value);
                else
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
            
            var span = value.AsSpan();
            
            // DE0007130800
            // ----------- length -----------
            if (span.Length != 12)
                return Validation.WrongLength;
            
            // ---------- country -----------
            if (!ValidateCountryCode(ref span))
                return Validation.InvalidCountryCode;
            
            // -------- basic number --------
            if (!ValidateBasicNumber(ref span))
                return Validation.ContainsIllegalCharacter;
            
            // -------- check digit ---------
            if (!ValidateCheckDigit(ref span))
                return Validation.InvalidCheckDigit;
            
            // --------- checksum ---------
            if (!ValidateChecksum(ref span))
                return Validation.InvalidChecksum;
            
            return Validation.Ok;
        }

        private static bool ValidateCountryCode(ref ReadOnlySpan<char> span)
        {
            for (var i = 0; i < 2; i++)
            {
                if (span[i] is < 'A' or > 'Z' && span[i] is < 'a' or > 'z') 
                    return false;
            }

            return true;
        }
        
        private static bool ValidateBasicNumber(ref ReadOnlySpan<char> span)
        {
            for (var i = 2; i < span.Length - 1; i++)
            {
                if (span[i] is >= '0' and <= '9') continue;
                if (span[i] is >= 'A' and <= 'Z') continue;
                if (span[i] is >= 'a' and <= 'z') continue;
                
                return false;
            }

            return true;
        }

        private static bool ValidateCheckDigit(ref ReadOnlySpan<char> span) => span[^1] is >= '0' and <= '9';

        private static bool ValidateChecksum(ref ReadOnlySpan<char> span)
        {
            var sum = 0;
            var doubleIt = false;
            
            // loop backwards
            for (var i = span.Length - 1; i >= 0; i--)
            {
                var digit = GetDigit(ref span, i);

                var divider = digit / 10;
                var remainder = digit % 10;

                if (doubleIt) remainder *= 2;
                else divider *= 2;
                
                sum += remainder <= 9 ? remainder : remainder - 9;
                
                if (divider != 0)
                {
                    sum += divider;
                    doubleIt = !doubleIt;
                }
                
                doubleIt = !doubleIt;
            }

            var mod = sum % 10;
            if (mod == 0) return true;

            return span[^1] - '0' == mod;
        }

        private static int GetDigit(ref ReadOnlySpan<char> span, int pos)
        {
            // digit
            if (span[pos] is >= '0' and <= '9') return span[pos] - '0';
            
            // uppercase letter
            if (span[pos] is >= 'A' and <= 'Z') return span[pos] - 'A' + 10;
            
            // lowercase letter
            return span[pos] - 'a' + 10;
        }
        
        #endregion
    }

    public class InvalidIsinException : Exception
    {
        public InvalidIsinException()
        {
        }

        public InvalidIsinException(string message) : base(message)
        {
        }
    }
}
