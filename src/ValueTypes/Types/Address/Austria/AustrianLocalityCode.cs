using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Address.Austria
{
    /// <summary>
    /// Value type for the austrian location code (OKZ/Ortschaftskennziffer).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidAustrianLocalityCodeException"></exception>
    public readonly record struct AustrianLocalityCode : IValueType<int, AustrianLocalityCode>
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
        
        #endregion
        
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AustrianLocalityCode"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidAustrianLocalityCodeException"></exception>
        public AustrianLocalityCode(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => throw new ArgumentNullException(nameof(value)),
                    Validation.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    Validation.InvalidLength => throw new InvalidAustrianLocalityCodeException($"The value '{value}' must be exactly 5 characters long!"),
                    Validation.ContainsIllegalCharacter => throw new InvalidAustrianLocalityCodeException($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidAustrianLocalityCodeException()
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private AustrianLocalityCode(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(AustrianLocalityCode left, string right) => left.Equals(right);
        public static bool operator !=(AustrianLocalityCode left, string right) => !left.Equals(right);

        public static implicit operator string(AustrianLocalityCode left) => left._value ?? "";
        public static implicit operator AustrianLocalityCode(string value) => new(value);

        #endregion

        #region public methods

        public static AustrianLocalityCode From(string value) => new(value);
        
        public static Validation TryFrom(string value, out AustrianLocalityCode output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new AustrianLocalityCode(ref value);
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
            // ---------- general -----------
            if (value is null)
                return Validation.Null;
            
            if (value.Length == 0)
                return Validation.Empty;

            if (value.Length != 5)
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

    public class InvalidAustrianLocalityCodeException : Exception
    {
        public InvalidAustrianLocalityCodeException()
        {
        }

        public InvalidAustrianLocalityCodeException(string message) : base(message)
        {
        }
    }
}
