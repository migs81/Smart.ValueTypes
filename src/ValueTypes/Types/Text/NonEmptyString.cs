using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Text
{
    /// <summary>
    /// Value type for a none-empty string.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidNonEmptyStringException"></exception>
    public readonly record struct NonEmptyString : IValueType<int, NonEmptyString>
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
        /// Initializes a new instance of the <see cref="NonEmptyString"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidNonEmptyStringException"></exception>
        public NonEmptyString(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => throw new ArgumentNullException(nameof(value)),
                    Validation.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    _ => new InvalidNonEmptyStringException()
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private NonEmptyString(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(NonEmptyString left, string right) => left.Equals(right);
        public static bool operator !=(NonEmptyString left, string right) => !left.Equals(right);

        public static implicit operator string(NonEmptyString left) => left._value ?? "";
        public static implicit operator NonEmptyString(string value) => new(value);

        #endregion

        #region public methods

        public static NonEmptyString From(string value) => new(value);
        
        public static Validation TryFrom(string value, out NonEmptyString output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new NonEmptyString(ref value);
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

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidNonEmptyStringException : Exception
    {
        public InvalidNonEmptyStringException()
        {
        }

        public InvalidNonEmptyStringException(string message) : base(message)
        {
        }
    }
}
