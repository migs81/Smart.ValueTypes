using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Network
{
    /// <summary>
    /// Value type for a port number.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidPortException"></exception>
    public readonly record struct Port : IValueType<double, Port>
    {
        #region fields

        private readonly double _value;

        public const double MinValue = 0;
        public const double MaxValue = 65_535;

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
        /// Initializes a new instance of the <see cref="Port"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidPortException"></exception>
        public Port(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidPortException($"The value '{value}' is too low for a {nameof(Port)}!"),
                    Validation.TooHigh => new InvalidPortException($"The value '{value}' is too high for a {nameof(Port)}!"),
                    _ => new InvalidPortException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private Port(ref double value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Port left, double right) => left.Equals(right);
        public static bool operator !=(Port left, double right) => !left.Equals(right);

        public static implicit operator double(Port latitude) => latitude._value;
        public static implicit operator Port(double value) => new(value);

        #endregion

        #region public methods

        public static Port From(double value) => new(value);

        public static Validation TryFrom(double value, out Port output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Port(ref value);
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

        public static Validation ValidateFormat(double value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            if (value > MaxValue)
                return Validation.TooHigh;

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidPortException : Exception
    {
        public InvalidPortException()
        {
        }

        public InvalidPortException(string message) : base(message)
        {
        }

        public InvalidPortException(double value)
            : base($"The value '{value} is not a valid {nameof(Port)}!")
        {
        }
    }
}
