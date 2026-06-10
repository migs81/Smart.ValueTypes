using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.Temperatures
{
    /// <summary>
    /// Value type for celsius temperatures.
    /// </summary>
    /// <seealso cref="ITemperature&lt;double, Reaumur&gt;" />
    /// <exception cref="InvalidReaumurException"></exception>
    public readonly partial record struct Reaumur : IValueType<double, Reaumur>, ITemperature
    {
        #region fields

        public const double MinValue = -218.52d;
        public const double MaxValue = double.MaxValue;
        public const double FreezingPoint = 0.0d;
        public const double BoilingPoint = 80.0d;
        public const string Unit = "°R";

        public enum Validation
        {
            OK = 0,
            TooLow,
            UnknownError
        }

        #endregion

        #region properties

        public double Value { get; }
        public static Reaumur Default => new();

        #endregion

        #region constructor

        public Reaumur() => Value = MinValue;
        public Reaumur(double value)
        {
            var result = Validate(ref value);
            if (result != Validation.OK)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidReaumurException($"The value '{value}' is too low for a {nameof(Reaumur)} temperature!"),
                    _ => new InvalidReaumurException(),
                };
            }

            Value = value;
        }
        public Reaumur(int value) : this((double)value) { }
        public Reaumur(Celsius celsius) : this(FromCelsius(celsius)) { }
        public Reaumur(Kelvin kelvin) : this(FromKelvin(kelvin)) { }
        public Reaumur(Fahrenheit fahrenheit) : this(FromFahrenheit(fahrenheit)) { }
        private Reaumur(ref double value) => Value = value;

        #endregion

        #region operator

        public static bool operator ==(Reaumur left, double right) => left.Equals(right);
        public static bool operator !=(Reaumur left, double right) => !left.Equals(right);

        public static implicit operator double(Reaumur reaumur) => reaumur.Value;
        public static implicit operator Reaumur(double value) => new(value);
        public static implicit operator Celsius(Reaumur reaumur) => new(reaumur.ToCelsius());
        public static implicit operator Kelvin(Reaumur reaumur) => new(reaumur.ToKelvin());
        public static implicit operator Fahrenheit(Reaumur reaumur) => new(reaumur.ToFahrenheit());

        #endregion

        #region public methods

        public bool Equals(double other) => EqualityComparer<double>.Default.Equals(Value, other);

        public static Reaumur From(int value) => new(value);
        public static Reaumur From(double value) => new(value);
        public static Reaumur From(Celsius celsius) => new(celsius);
        public static Reaumur From(Kelvin kelvin) => new(kelvin);
        public static Reaumur From(Fahrenheit fahrenheit) => new(fahrenheit);

        public static Validation TryFrom(double value, out Reaumur output)
        {
            try
            {
                var result = Validate(ref value);
                if (result == Validation.OK)
                {
                    output = new Reaumur(ref value);
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
        public static Validation TryFrom(int value, out Reaumur output) => TryFrom((double)value, out output);
        public static Validation TryFrom(Celsius celsius, out Reaumur output) => TryFrom(FromCelsius(celsius), out output);
        public static Validation TryFrom(Fahrenheit fahrenheit, out Reaumur output) => TryFrom(FromFahrenheit(fahrenheit), out output);
        public static Validation TryFrom(Kelvin kelvin, out Reaumur output) => TryFrom(FromKelvin(kelvin), out output);

        public double ToCelsius() => Value * 5 / 4;
        public double ToKelvin() => (Value * 9 / 8) + 273.15;
        public double ToFahrenheit() => (Value * 9 / 4) + 32;

        public static Validation Validate(double value) => Validate(ref value);

        #endregion

        #region private methods

        private static Validation Validate(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            return Validation.OK;
        }

        private static double FromCelsius(double celsius) => celsius * 4 / 5;
        private static double FromKelvin(double kelvin) => (kelvin - 273.15) * 4 / 5;
        private static double FromFahrenheit(double fahrenheit) => (fahrenheit - 32) * 4 / 9;

        #endregion
    }

    public class InvalidReaumurException : Exception
    {
        public InvalidReaumurException()
        {
        }

        public InvalidReaumurException(string message) : base(message)
        {
        }

        public InvalidReaumurException(double value)
            : base($"The value '{value}' is not a valid {nameof(Reaumur)} temperature!")
        {
        }
    }
}
