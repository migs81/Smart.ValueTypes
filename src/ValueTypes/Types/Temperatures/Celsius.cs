using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.Temperatures
{
    /// <summary>
    /// Value type for celsius temperatures.
    /// </summary>
    /// <exception cref="InvalidCelsiusException"></exception>
    public readonly record struct Celsius : IValueType<double, Celsius>, ITemperature
    {
        #region fields

        public const double MinValue = -273.15d;
        public const double MaxValue = double.MaxValue;
        public const double FreezingPoint = 0.0d;
        public const double BoilingPoint = 100.0d;
        public const string Unit = "°C";

        public enum Validation
        {
            Ok = 0,
            TooLow,
            UnknownError
        }

        #endregion

        #region properties

        public double Value { get; }
        public static Celsius Zero => new(0d);

        #endregion

        #region constructor

        public Celsius() => Value = 0d;
        public Celsius(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidCelsiusException($"The value '{value}' is too low for a {nameof(Celsius)} temperature!"),
                    _ => new InvalidCelsiusException(),
                };
            }

            Value = value;
        }
        public Celsius(int value) : this((double)value) { }
        public Celsius(Kelvin kelvin) : this(FromKelvin(kelvin)) { }
        public Celsius(Fahrenheit fahrenheit) : this(FromFahrenheit(fahrenheit)) { }
        public Celsius(Reaumur reaumur) : this(FromReaumur(reaumur)) { }
        private Celsius(ref double value) => Value = value;

        #endregion
        
        #region operator

        public static bool operator ==(Celsius left, double right) => left.Equals(right);
        public static bool operator !=(Celsius left, double right) => !left.Equals(right);

        public static implicit operator double(Celsius celsius) => celsius.Value;
        public static implicit operator Celsius(double value) => new(value);
        public static implicit operator Kelvin(Celsius celsius) => new(celsius.ToKelvin());
        public static implicit operator Fahrenheit(Celsius celsius) => new(celsius.ToFahrenheit());
        public static implicit operator Reaumur(Celsius celsius) => new(celsius.ToReaumur());

        #endregion

        #region public methods

        public static Celsius From(int value) => new(value);
        public static Celsius From(double value) => new(value);
        public static Celsius From(Kelvin kelvin) => new(kelvin);
        public static Celsius From(Fahrenheit fahrenheit) => new(fahrenheit);
        public static Celsius From(Reaumur reaumur) => new(reaumur);

        public static Validation TryFrom(double value, out Celsius output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Celsius(ref value);
                    return Validation.Ok;
                }

                output = Zero;
                return result;
            }
            catch (Exception)
            {
                output = Zero;
                return Validation.UnknownError;
            }
        }
        public static Validation TryFrom(int value, out Celsius output) => TryFrom((double)value, out output);
        public static Validation TryFrom(Kelvin kelvin, out Celsius output) => TryFrom(FromKelvin(kelvin), out output);
        public static Validation TryFrom(Fahrenheit fahrenheit, out Celsius output) => TryFrom(FromFahrenheit(fahrenheit), out output);
        public static Validation TryFrom(Reaumur reaumur, out Celsius output) => TryFrom(FromReaumur(reaumur), out output);

        public double ToKelvin() => Value + 273.15d;
        public double ToFahrenheit() => (Value * 9d / 5d) + 32d;
        public double ToReaumur() => Value * 4 / 5;

        public static Validation Validate(double value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            return Validation.Ok;
        }

        private static double FromKelvin(double kelvin) => kelvin - 273.15d;
        private static double FromFahrenheit(double fahrenheit) => (fahrenheit - 32d) * 5d / 9d;
        private static double FromReaumur(double reaumur) => reaumur * 5 / 4;

        #endregion
    }

    public class InvalidCelsiusException : Exception
    {
        public InvalidCelsiusException()
        {
        }

        public InvalidCelsiusException(string message) : base(message)
        {
        }

        public InvalidCelsiusException(double value)
            : base($"The value '{value}' is not a valid {nameof(Celsius)} temperature!")
        {
        }
    }
}
