using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Identifiers
{
    /// <summary>
    /// Value type for identifying securities in the German market (Wertpapierkennnummer).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidWknException"></exception>
    public readonly record struct WKN : IValueType<string, WKN>
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
            UnknownError,
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WKN"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidWknException"></exception>
        public WKN(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.WrongLength => new InvalidWknException($"The value '{value}' has the wrong length for an ISBN-13!"),
                    Validation.ContainsIllegalCharacter => new InvalidWknException($"The value '{value}' contains an illegal character!"),
                    _ => new InvalidWknException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private WKN(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(WKN left, string right) => left.Equals(right);
        public static bool operator !=(WKN left, string right) => !left.Equals(right);

        public static implicit operator string(WKN isbn) => isbn._value ?? "";
        public static implicit operator WKN(string value) => new(value);

        #endregion

        #region public methods

        public static WKN From(string value) => new(value);
        
        public static Validation TryFrom(string value, out WKN output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new WKN(ref value);
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
            
            // ----------- length -----------
            if (span.Length != 6)
                return Validation.WrongLength;
            
            // --------- characters ---------
            if (!ValidateCharacters(ref span))
                return Validation.ContainsIllegalCharacter;
            
            return Validation.Ok;
        }

        private static bool ValidateCharacters(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (c is >= '0' and <= '9') continue;
                if (c is >= 'A' and <= 'Z') continue;
                if (c is >= 'a' and <= 'z') continue;
                
                return false;
            }

            return true;
        }

        #endregion
    }

    public class InvalidWknException : Exception
    {
        public InvalidWknException()
        {
        }

        public InvalidWknException(string message) : base(message)
        {
        }
    }
}
