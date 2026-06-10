using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.Temperatures
{
    /// <summary>
    /// Value type for Fahrenheit temperatures.
    /// </summary>
    /// <seealso cref="ITemperature&lt;double, Fahrenheit&gt;" />
    /// <exception cref="InvalidFahrenheitException"></exception>
    public readonly record struct Fahrenheit : IValueType<double, Fahrenheit>, ITemperature
    {
        #region fields

        public const double MinValue = -459.67d;
        public const double MaxValue = double.MaxValue;
        public const double FreezingPoint = 32.0d;
        public const double BoilingPoint = 212.0d;
        public const string Unit = "°F";

        public enum Validation
        {
            OK = 0,
            TooLow,
            UnknownError
        }

        #endregion

        #region properties

        public double Value { get; }
        public static Fahrenheit Default => new();

        #endregion

        #region constructor

        public Fahrenheit() => Value = MinValue;
        public Fahrenheit(double value)
        {
            var result = Validate(ref value);
            if (result != Validation.OK)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidFahrenheitException($"The value '{value}' is too low for a {nameof(Fahrenheit)} temperature!"),
                    _ => new InvalidFahrenheitException(),
                };
            }

            Value = value;
        }
        public Fahrenheit(int value) : this((double)value) { }
        public Fahrenheit(Celsius celsius) : this(FromCelsius(celsius)) { }
        public Fahrenheit(Kelvin kelvin) : this(FromKelvin(kelvin)) { }
        public Fahrenheit(Reaumur reaumur) : this(FromReaumur(reaumur)) { }
        private Fahrenheit(ref double value) => Value = value;

        #endregion

        #region operator

        public static bool operator ==(Fahrenheit left, double right) => left.Equals(right);
        public static bool operator !=(Fahrenheit left, double right) => !left.Equals(right);

        public static implicit operator double(Fahrenheit fahrenheit) => fahrenheit.Value;
        public static implicit operator Fahrenheit(double value) => new(value);
        public static implicit operator Celsius(Fahrenheit fahrenheit) => new(fahrenheit.ToCelsius());
        public static implicit operator Kelvin(Fahrenheit fahrenheit) => new(fahrenheit.ToKelvin());
        public static implicit operator Reaumur(Fahrenheit fahrenheit) => new(fahrenheit.ToReaumur());

        #endregion

        #region public methods

        public bool Equals(double other) => EqualityComparer<double>.Default.Equals(Value, other);

        public static Fahrenheit From(int value) => new(value);
        public static Fahrenheit From(double value) => new(value);
        public static Fahrenheit From(Celsius celsius) => new(celsius);
        public static Fahrenheit From(Kelvin kelvin) => new(kelvin);
        public static Fahrenheit From(Reaumur reaumur) => new(reaumur);

        public static Validation TryFrom(double value, out Fahrenheit output)
        {
            try
            {
                var result = Validate(ref value);
                if (result == Validation.OK)
                {
                    output = new Fahrenheit(ref value);
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
        public static Validation TryFrom(int value, out Fahrenheit output) => TryFrom((double)value, out output);
        public static Validation TryFrom(Celsius celsius, out Fahrenheit output) => TryFrom(FromCelsius(celsius), out output);
        public static Validation TryFrom(Kelvin kelvin, out Fahrenheit output) => TryFrom(FromKelvin(kelvin), out output);
        public static Validation TryFrom(Reaumur reaumur, out Fahrenheit output) => TryFrom(FromReaumur(reaumur), out output);

        public double ToCelsius() => (Value - 32d) * 5d / 9d;
        public double ToKelvin() => (Value + 459.67d) * 5d / 9d;
        public double ToReaumur() => (Value - 32) * 4 / 9;

        public static Validation Validate(double value) => Validate(ref value);

        #endregion

        #region private methods

        private static Validation Validate(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            return Validation.OK;
        }

        private static double FromCelsius(double celsius) => (celsius * 9d / 5d) + 32d;
        private static double FromKelvin(double kelvin) => (kelvin - 273.15d) * 9d / 5d + 32d;
        private static double FromReaumur(double reaumur) => (reaumur * 9 / 4) + 32;

        #endregion
    }

    public class InvalidFahrenheitException : Exception
    {
        public InvalidFahrenheitException()
        {
        }

        public InvalidFahrenheitException(string message) : base(message)
        {
        }

        public InvalidFahrenheitException(double value)
            : base($"The value '{value}' is not a valid {nameof(Fahrenheit)} temperature!")
        {
        }
    }
}
