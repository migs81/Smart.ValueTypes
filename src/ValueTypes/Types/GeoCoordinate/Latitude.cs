using Migs.ValueTypes.Interfaces;
using Migs.ValueTypes.Types.Hashes;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.GeoCoordinate
{
    /// <summary>
    /// Value type for a latitude.
    /// </summary>
    /// <seealso cref="IValueType&lt;double, Latitude&gt;" />
    /// <exception cref="InvalidLatitudeException"></exception>
    public readonly record struct Latitude : IValueType<double, Latitude>
    {
        #region fields

        private readonly double _value;
        private readonly double _defaultValue = 0;

        public const double MaxValue = 90;
        public const double MinValue = -90;

        public enum Validation
        {
            OK = 0,
            TooLow,
            TooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static Latitude Default => new();

        #endregion

        #region constructor

        public Latitude() => _value = _defaultValue;
        public Latitude(double value)
        {
            var result = Validate(ref value);
            if (result != Validation.OK)
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
        private Latitude(ref double value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Latitude left, double right) => left.Equals(right);
        public static bool operator !=(Latitude left, double right) => !left.Equals(right);

        public static implicit operator double(Latitude latitude) => latitude._value;
        public static implicit operator Latitude(double value) => new(value);

        #endregion

        #region public methods

        public bool Equals(double value) => EqualityComparer<double>.Default.Equals(_value, value);

        public static Latitude From(double value) => new(value);

        public static Validation TryFrom(double value, out Latitude output)
        {
            try
            {
                var result = Validate(ref value);
                if (result == Validation.OK)
                {
                    output = new Latitude(ref value);
                    return Validation.OK;
                }

                output = Default;
                return result;
            }
            catch (Exception)
            {
                output = Default;
                return Validation.UnknownError;
            }
        }

        public static Validation Validate(double value) => Validate(ref value);

        #endregion

        #region private methods

        private static Validation Validate(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            if (value > MaxValue)
                return Validation.TooHigh;

            return Validation.OK;
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
