using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Address.Austria
{
    /// <summary>
    /// Value type for the austrian municipality code (GKZ/Gemeindekennziffer).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidAustrianAddressCodeException"></exception>
    public readonly record struct AustrianAddressCode : IValueType<int, AustrianAddressCode>
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
        /// Initializes a new instance of the <see cref="AustrianAddressCode"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidAustrianAddressCodeException"></exception>
        public AustrianAddressCode(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => throw new ArgumentNullException(nameof(value)),
                    Validation.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    Validation.InvalidLength => throw new InvalidAustrianAddressCodeException($"The value '{value}' must be exactly 5 characters long!"),
                    Validation.ContainsIllegalCharacter => throw new InvalidAustrianAddressCodeException($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidAustrianAddressCodeException()
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private AustrianAddressCode(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(AustrianAddressCode left, string right) => left.Equals(right);
        public static bool operator !=(AustrianAddressCode left, string right) => !left.Equals(right);

        public static implicit operator string(AustrianAddressCode left) => left._value ?? "";
        public static implicit operator AustrianAddressCode(string value) => new(value);

        #endregion

        #region public methods

        public static AustrianAddressCode From(string value) => new(value);
        
        public static Validation TryFrom(string value, out AustrianAddressCode output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new AustrianAddressCode(ref value);
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

    public class InvalidAustrianAddressCodeException : Exception
    {
        public InvalidAustrianAddressCodeException()
        {
        }

        public InvalidAustrianAddressCodeException(string message) : base(message)
        {
        }
    }
}
