using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.GeoCoordinate
{
    /// <summary>
    /// Value type for a longitude.
    /// </summary>
    /// <seealso cref="IValueType&lt;double, Longitude&gt;" />
    /// <exception cref="InvalidLongitudeException"></exception>
    public readonly record struct Longitude : IValueType<double, Longitude>
    {
        #region fields

        private readonly double _value;
        private readonly double _defaultValue = 0;

        public const double MaxValue = 180;
        public const double MinValue = -180;

        public enum Validation
        {
            Ok = 0,
            TooLow,
            TooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static Longitude Default => new();

        #endregion

        #region constructor

        public Longitude() => _value = _defaultValue;
        public Longitude(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidLongitudeException($"The value '{value}' is too low for a {nameof(Longitude)}!"),
                    Validation.TooHigh => new InvalidLongitudeException($"The value '{value}' is too high for a {nameof(Longitude)}!"),
                    _ => new InvalidLongitudeException(),
                };
            }

            _value = value;
        }
        private Longitude(ref double value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Longitude left, double right) => left.Equals(right);
        public static bool operator !=(Longitude left, double right) => !left.Equals(right);

        public static implicit operator double(Longitude longitude) => longitude._value;
        public static implicit operator Longitude(double value) => new(value);

        #endregion

        #region public methods

        public bool Equals(double value) => EqualityComparer<double>.Default.Equals(_value, value);

        public static Longitude From(double value) => new(value);

        public static Validation TryFrom(double value, out Longitude output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Longitude(ref value);
                    return Validation.Ok;
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

    public class InvalidLongitudeException : Exception
    {
        public InvalidLongitudeException()
        {
        }

        public InvalidLongitudeException(string message) : base(message)
        {
        }

        public InvalidLongitudeException(double value)
            : base($"The value '{value}' is not a valid {nameof(Longitude)}]!")
        {
        }
    }
}
