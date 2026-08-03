using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Graphics
{
    /// <summary>
    /// Value type for opacity.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidOpacityException"></exception>
    public readonly record struct Opacity : IValueType<double, Opacity>
    {
        #region fields

        private readonly double _value;
        
        public const double MinValue = 0d;
        public const double MaxValue = 1d;
        
        public enum Validation
        {
            Ok = 0,
            TooLow,
            TooHigh,
            UnknownError
        }
        
        #endregion

        #region properties

        public bool IsDefault => _value == 0d;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Opacity"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidOpacityException"></exception>
        public Opacity(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidOpacityException($"The value '{value}' is too low for an {nameof(Opacity)}!"),
                    Validation.TooHigh => new InvalidOpacityException($"The value '{value}' is too high for an {nameof(Opacity)}!"),
                    _ => new InvalidOpacityException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private Opacity(ref double value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Opacity left, double right) => left.Equals(right);
        public static bool operator !=(Opacity left, double right) => !left.Equals(right);

        public static implicit operator double(Opacity left) => left._value;
        public static implicit operator Opacity(double value) => new(value);

        #endregion

        #region public methods

        public static Opacity From(double value) => new(value);
        
        public static Validation TryFrom(double value, out Opacity output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Opacity(ref value);
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

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref double value)
        {
            if (value < 0)
                return Validation.TooLow;

            if (value > 1)
                return Validation.TooHigh;

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidOpacityException : Exception
    {
        public InvalidOpacityException()
        {
        }

        public InvalidOpacityException(string message)
        {
        }
        
        public InvalidOpacityException(double value)
            : base($"The value '{value} is not a valid {nameof(Opacity)}!")
        {
        }
    }
}
