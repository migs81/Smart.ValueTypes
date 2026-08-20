using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Address.Austria
{
    /// <summary>
    /// Value type for the austrian postal code (Postleitzahl).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidAustrianPostalCodeException"></exception>
    public readonly record struct AustrianPostalCode : IValueType<int, AustrianPostalCode>
    {
        #region fields

        private readonly string? _value;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            InvalidLength,
            ContainsIllegalCharacter,
            UnknownError,
        }
        
        #endregion

        #region properties

        public bool IsDefault => _value is null;

        public int Zone => _value is not null ? int.Parse(_value[0].ToString()) : 0;
        
        public int District => _value is not null ? int.Parse(_value[0..1]) : 0;
        
        public int Route => _value is not null ? int.Parse(_value[0..2]) : 0;
        
        #endregion
        
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AustrianPostalCode"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidAustrianPostalCodeException"></exception>
        public AustrianPostalCode(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => throw new ArgumentNullException(nameof(value)),
                    Validation.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    Validation.InvalidLength => throw new InvalidAustrianPostalCodeException($"The value '{value}' must be exactly 4 characters long!"),
                    Validation.ContainsIllegalCharacter => throw new InvalidAustrianPostalCodeException($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidAustrianPostalCodeException()
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private AustrianPostalCode(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(AustrianPostalCode left, string right) => left.Equals(right);
        public static bool operator !=(AustrianPostalCode left, string right) => !left.Equals(right);

        public static implicit operator string(AustrianPostalCode left) => left._value ?? "";
        public static implicit operator AustrianPostalCode(string value) => new(value);

        #endregion

        #region public methods

        public static AustrianPostalCode From(string value) => new(value);
        
        public static Validation TryFrom(string value, out AustrianPostalCode output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new AustrianPostalCode(ref value);
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

        public static Validation Validate(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            // ---------- general -----------
            if (value is null)
                return Validation.Null;
            
            if (value.Length == 0)
                return Validation.Empty;

            if (value.Length != 4)
                return Validation.InvalidLength;

            // ----- illegal characters -----
            var span = value.AsSpan();
            foreach (var c in span)
            {
                if (c is < '0' or > '9')
                    return Validation.ContainsIllegalCharacter;
            }
            
            return Validation.Ok;
        }
        
        #endregion
    }

    public class InvalidAustrianPostalCodeException : Exception
    {
        public InvalidAustrianPostalCodeException()
        {
        }

        public InvalidAustrianPostalCodeException(string message) : base(message)
        {
        }
    }
}
