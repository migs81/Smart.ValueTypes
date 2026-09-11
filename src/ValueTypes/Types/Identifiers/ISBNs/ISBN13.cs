using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Identifiers.ISBNs
{
    /// <summary>
    /// Value type for Universally Unique Lexicographically Sortable Identifier (ISBN-13).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIsbn13Exception"></exception>
    public readonly record struct ISBN13 : IValueType<string, ISBN13>
    {
        #region fields

        private readonly string? _value;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WrongLength,
            ContainsIllegalCharacter,
            InvalidChecksum,
            InvalidPrefix,
            TrailingSeparator,
            ConsecutiveSeparators,
            UnknownError,
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        public string Prefix => _value is not null ? _value[..3] : "";

        public int CheckDigit => _value is not null ? int.Parse(_value[^1].ToString()) : -1;

        public string Normalized => _value is not null ? _value.Replace("-", "").Replace(" ", "") : "";
        
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ISBN13"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidIsbn13Exception"></exception>
        public ISBN13(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.WrongLength => new InvalidIsbn13Exception($"The value '{value}' has the wrong length for an ISBN-13!"),
                    Validation.ContainsIllegalCharacter => new InvalidIsbn13Exception($"The value '{value}' contains an illegal character!"),
                    Validation.InvalidPrefix => new InvalidIsbn13Exception($"The value '{value}' contains an invalid prefix!"),
                    Validation.TrailingSeparator => new InvalidIsbn13Exception($"The value '{value}' contains a trailing separator!"),
                    Validation.ConsecutiveSeparators => new InvalidIsbn13Exception($"The value '{value}' contains consecutive separators!"),
                    Validation.InvalidChecksum => new InvalidIsbn13Exception($"The checksum of the value '{value}' is wrong!"),
                    _ => new InvalidIsbn13Exception(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private ISBN13(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(ISBN13 left, string right) => left.Equals(right);
        public static bool operator !=(ISBN13 left, string right) => !left.Equals(right);

        public static implicit operator string(ISBN13 isbn) => isbn._value ?? "";
        public static implicit operator ISBN13(string value) => new(value);

        #endregion

        #region public methods

        public static ISBN13 From(string value) => new(value);
        
        public static Validation TryFrom(string value, out ISBN13 output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new ISBN13(ref value);
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

        public static ISBN13 Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            
            if (value.Length == 0)
                throw new ArgumentException($"Argument can not be empty", nameof(value));
            
            // remove separators
            value = value.Replace("-", "").Replace(" ", "");
            
            // validate value
            return new ISBN13(value);
        }

        public static bool TryParse(string value, out ISBN13 output)
        {
            try
            {

                if (string.IsNullOrEmpty(value))
                {
                    output = default;
                    return false;
                }

                // remove separators
                value = value.Replace("-", "").Replace(" ", "");
            
                // validate value
                output = new ISBN13(value);
                return true;
            }
            catch
            {
                output = default;
                return false;
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
            
            // ----------- length -----------
            var count = span.Count('-') + span.Count(' ');
            if (span.Length - count != 13)
                return Validation.WrongLength;
            
            // ---------- prefix ----------
            if (span[0] is not '9' || span[1] is not '7')
                return Validation.InvalidPrefix;

            if (span[2] is not ('8' or '9'))
                return Validation.InvalidPrefix;
            
            // --------- separator ---------
            if (span[^1] is '-' or ' ')
                return Validation.TrailingSeparator;
            
            if (span.IndexOf("--") != -1 || span.IndexOf("  ") != -1)
                return Validation.ConsecutiveSeparators;
            
            // -------- characters --------
            if (!ValidateCharacters(ref span))
                return Validation.ContainsIllegalCharacter;
            
            // --------- checksum ---------
            if (!ValidateChecksum(ref span))
                return Validation.InvalidChecksum;
            
            return Validation.Ok;
        }

        private static bool ValidateChecksum(ref ReadOnlySpan<char> span)
        {
            var sum = 0;
            var weighting = 1;
            
            // multiply each number by its weight
            for (var i = 0; i < span.Length - 1; i++)
            {
                if (span[i] is < '0' or > '9') continue;
                
                sum += (span[i] - '0') * weighting;
                weighting = weighting == 1 ? 3 : 1;
            }

            // calculate checksum
            var remainder = sum % 10;
            
            // if remainder is 0, the check digit must be 0
            if (remainder == 0)
                return span[^1] is '0';
            
            // otherwise 10 minus remainder must equal the last digit
            return (10 - remainder) == (span[^1] - '0');
        }
        
        private static bool ValidateCharacters(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (c is >= '0' and <= '9') continue;
                if (c is '-' or ' ') continue;

                return false;
            }

            return true;
        }

        #endregion
    }

    public class InvalidIsbn13Exception : Exception
    {
        public InvalidIsbn13Exception()
        {
        }

        public InvalidIsbn13Exception(string message) : base(message)
        {
        }
    }
}
