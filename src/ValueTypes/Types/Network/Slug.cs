using System;
using System.Globalization;
using System.Text;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Network
{
    /// <summary>
    /// Represents a URL-friendly string used to identify a resource.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSlugException"></exception>
    public readonly record struct Slug : IValueType<string, Slug>
    {
        #region fields

        private readonly string _value;
        private const string Default = "n-a";
        
        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            InvalidHyphenPlacement,
            IllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public static Slug Empty => new();

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Slug"/> struct.
        /// </summary>
        public Slug() => _value = Default;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Slug"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidSlugException"></exception>
        public Slug(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.InvalidHyphenPlacement => new InvalidSlugException($"The value '{value}' contains illegal hyphen placement!"),
                    Validation.IllegalCharacter => new InvalidSlugException($"The value '{value}' contains an illegal character!"),
                    _ => new InvalidSlugException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private Slug(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Slug left, string right) => left.Equals(right);
        public static bool operator !=(Slug left, string right) => !left.Equals(right);

        public static implicit operator string(Slug oid) => oid._value;
        public static implicit operator Slug(string value) => new(value);

        #endregion

        #region public methods

        public static Slug New() => new();

        public static Slug From(string value) => new(value);
        
        public static Validation TryFrom(string value, out Slug output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new Slug(ref value);
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

        public static Slug Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            
            if (value.Length == 0)
                throw new ArgumentException($"Argument can not be empty", nameof(value));

            // --- general ---
            value = value.Trim().ToLowerInvariant();
                
            // --- replace umlauts ---
            value = value.Replace("ä", "ae")
                .Replace("ö", "oe")
                .Replace("ü", "ue")
                .Replace("ß", "ss");
            
            // --- normalize string ---
            var normalized = value.Normalize(NormalizationForm.FormD);
            var str = new StringBuilder(normalized.Length);
            foreach (var c in normalized)
            {
                // diacritics
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
                
                // allow specific chars only
                if (c is >= 'a' and <= 'z' or >= '0' and <= '9')
                    str.Append(c);
                else    
                    str.Append('-');
            }

            value = str.ToString();
            
            // --- replace duplicates ---
            while (value.Contains("--"))
                value = value.Replace("--", "-");
            
            // --- remove minus at start and end ---
            value = value.Trim('-');
            
            return new Slug(value);
        }
        
        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            // ----------- general ----------------
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;
            
            // --------- characters ---------------
            if (value[0] == '-' || value[^1] == '-')
                return Validation.InvalidHyphenPlacement;
            
            if (value.Contains("--"))
                return Validation.InvalidHyphenPlacement;
            
            foreach (var c in value.AsSpan())
            {
                if (c is >= 'a' and <= 'z') continue;
                if (c is >= '0' and <= '9') continue;
                if (c is '-') continue;
                
                return Validation.IllegalCharacter;
            }

            return Validation.Ok;
        }

        #endregion
    }
    
    public class InvalidSlugException : Exception
    {
        public InvalidSlugException()
        {
        }

        public InvalidSlugException(string message) : base(message)
        {
        }
    }
}
