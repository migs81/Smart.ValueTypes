using System;
using System.Collections.Generic;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Types.Coordinates
{
    /// <summary>
    /// Value type for a latitude.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidLatitudeException"></exception>
    public readonly record struct Latitude : IValueType<double, Latitude>
    {
        #region fields

        private readonly double _value;
        private const double Default = 0;
        
        public const double MaxValue = 90;
        public const double MinValue = -90;

        public enum Validation
        {
            Ok = 0,
            TooLow,
            TooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static Latitude Empty => new();

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Latitude"/> struct.
        /// </summary>
        public Latitude() => _value = Default;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Latitude"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidLatitudeException"></exception>
        public Latitude(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidLatitudeException($"The value '{value}' is too low for a {nameof(Latitude)}!"),
                    Validation.TooHigh => new InvalidLatitudeException($"The value '{value}' is too high for a {nameof(Latitude)}!"),
                    _ => new InvalidLatitudeException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private Latitude(ref double value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Latitude left, double right) => left.Equals(right);
        public static bool operator !=(Latitude left, double right) => !left.Equals(right);

        public static implicit operator double(Latitude latitude) => latitude._value;
        public static implicit operator Latitude(double value) => new(value);

        #endregion

        #region public methods

        public static Latitude From(double value) => new(value);

        public static Validation TryFrom(double value, out Latitude output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Latitude(ref value);
                    return Validation.Ok;
                }

                output = Empty;
                return result;
            }
            catch (Exception)
            {
                output = Empty;
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

    public class InvalidLatitudeException : Exception
    {
        public InvalidLatitudeException()
        {
        }

        public InvalidLatitudeException(string message) : base(message)
        {
        }

        public InvalidLatitudeException(double value)
            : base($"The value '{value} is not a valid {nameof(Longitude)}!")
        {
        }
    }
}
