using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Identifiers.ISBNs
{
    /// <summary>
    /// Value type for Universally Unique Lexicographically Sortable Identifier (ISBN10).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIsbn10Exception"></exception>
    public readonly record struct ISBN10 : IValueType<string, ISBN10>
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
            LeadingSeparator,
            TrailingSeparator,
            ConsecutiveSeparators,
            UnknownError,
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;
        
        public int CheckDigit
        {
            get
            {
                if (_value is null) 
                    return -1;
                 
                if (_value[^1] is 'x' or 'X') 
                    return 10;
                
                return int.Parse(_value[^1].ToString());
            }
        }

        public string Normalized => _value is not null ? _value.Replace("-", "").Replace(" ", "") : "";
        
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ISBN10"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidIsbn10Exception"></exception>
        public ISBN10(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.WrongLength => new InvalidIsbn10Exception($"The value '{value}' has the wrong length for an ISBN-10!"),
                    Validation.ContainsIllegalCharacter => new InvalidIsbn10Exception($"The value '{value}' contains an illegal character!"),
                    Validation.LeadingSeparator => new InvalidIsbn10Exception($"The value '{value}' contains a leading separator!"),
                    Validation.TrailingSeparator => new InvalidIsbn10Exception($"The value '{value}' contains a trailing separator!"),
                    Validation.ConsecutiveSeparators => new InvalidIsbn10Exception($"The value '{value}' contains consecutive separators!"),
                    Validation.InvalidChecksum => new InvalidIsbn10Exception($"The checksum of the value '{value}' is wrong!"),
                    _ => new InvalidIsbn10Exception(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private ISBN10(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(ISBN10 left, string right) => left.Equals(right);
        public static bool operator !=(ISBN10 left, string right) => !left.Equals(right);

        public static implicit operator string(ISBN10 isbn) => isbn._value ?? "";
        public static implicit operator ISBN10(string value) => new(value);

        #endregion

        #region public methods

        public static ISBN10 From(string value) => new(value);
        
        public static Validation TryFrom(string value, out ISBN10 output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new ISBN10(ref value);
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

        public static ISBN10 Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            
            if (value.Length == 0)
                throw new ArgumentException($"Argument can not be empty", nameof(value));
            
            // remove separators
            value = value.Replace("-", "").Replace(" ", "");
            
            // validate value
            return new ISBN10(value);
        }

        public static bool TryParse(string value, out ISBN10 output)
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
                output = new ISBN10(value);
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
            if (span.Length - count != 10)
                return Validation.WrongLength;
            
            // --------- separator ---------
            if (span[0] is '-' or ' ')
                return Validation.LeadingSeparator;
            
            if (span[^1] is '-' or ' ')
                return Validation.TrailingSeparator;
            
            if (span.IndexOf("--") != -1 || span.IndexOf("  ") != -1)
                return Validation.ConsecutiveSeparators;
            
            // --------- characters ---------
            if (!ValidateCharacters(ref span))
                return Validation.ContainsIllegalCharacter;
            
            // ---------- checksum ----------
            if (!ValidateChecksum(ref span))
                return Validation.InvalidChecksum;
            
            return Validation.Ok;
        }

        private static bool ValidateChecksum(ref ReadOnlySpan<char> span)
        {
            var sum = 0;
            var weight = 0;
            
            // multiply all digits, except the last one, by their weighting and add them all up
            for (var i = 0; i < span.Length - 1; i++)
            {
                if (span[i] is < '0' or > '9') continue;
                
                sum += (span[i] - '0') * (weight + 1);
                weight++;
            }
            
            // calculate checksum
            var mod = sum % 11;
            
            // last digit i 'x'? Then the modulo result must bei 10
            if (span[^1] is 'x' or 'X' && mod == 10)
                return true;
            
            // last digit equals modulo result?
            return (span[^1] - '0') == mod;
        }
        
        private static bool ValidateCharacters(ref ReadOnlySpan<char> span)
        {
            // check all characters except the last one
            for (var i = 0; i < span.Length - 1; i++)
            {
                if (span[i] is >= '0' and <= '9') continue;
                if (span[i] is '-' or ' ') continue;

                return false;
            }

            // last character can be a digit or x (=10)
            if (span[^1] is (< '0' or > '9') and not ('x' or 'X'))
                return false;

            return true;
        }
        
        #endregion
    }

    public class InvalidIsbn10Exception : Exception
    {
        public InvalidIsbn10Exception()
        {
        }

        public InvalidIsbn10Exception(string message) : base(message)
        {
        }
    }
}
