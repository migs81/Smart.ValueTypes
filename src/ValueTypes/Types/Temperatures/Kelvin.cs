using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.Temperatures
{
    /// <summary>
    /// Value type for Kelvin temperatures.
    /// </summary>
    /// <exception cref="InvalidKelvinException"></exception>
    public readonly record struct Kelvin : IValueType<double, Kelvin>, ITemperature
    {
        #region fields

        public const double MinValue = 0d;
        public const double MaxValue = double.MaxValue;
        public const double FreezingPoint = 273.15d;
        public const double BoilingPoint = 373.15d;
        public const string Unit = "°K";

        public enum Validation
        {
            Ok = 0,
            TooLow,
            UnknownError
        }

        #endregion

        #region properties

        public double Value { get; }

        public static Kelvin Zero => new(0d);

        #endregion

        #region constructor

        public Kelvin() => Value = 0d;
        public Kelvin(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidKelvinException($"The value '{value}' is too low for a {nameof(Kelvin)} temperature!"),
                    _ => new InvalidKelvinException(),
                };
            }

            Value = value;
        }
        public Kelvin(int value) : this((double)value) { }
        public Kelvin(Celsius celsius) => Value = FromCelsius(celsius);
        public Kelvin(Fahrenheit fahrenheit) => Value = FromFahrenheit(fahrenheit);
        public Kelvin(Reaumur reaumur) => Value = FromReaumur(reaumur);
        private Kelvin(ref double value) => Value = value;

        #endregion

        #region operator

        public static bool operator ==(Kelvin left, double right) => left.Equals(right);
        public static bool operator !=(Kelvin left, double right) => !left.Equals(right);

        public static implicit operator double(Kelvin kelvin) => kelvin.Value;
        public static implicit operator Kelvin(double value) => new(value);
        public static implicit operator Celsius(Kelvin kelvin) => new(ToCelsius(kelvin));
        public static implicit operator Fahrenheit(Kelvin kelvin) => new(ToFahrenheit(kelvin));
        public static implicit operator Reaumur(Kelvin kelvin) => new(ToReaumur(kelvin));

        #endregion

        #region public methods

        public static Kelvin From(int value) => new(value);
        public static Kelvin From(double value) => new(value);
        public static Kelvin From(Celsius celsius) => new(celsius);
        public static Kelvin From(Fahrenheit fahrenheit) => new(fahrenheit);
        public static Kelvin From(Reaumur reaumur) => new(reaumur);

        public static Validation TryFrom(double value, out Kelvin output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Kelvin(ref value);
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
        public static Validation TryFrom(int value, out Kelvin output) => TryFrom((double)value, out output);
        public static Validation TryFrom(Celsius celsius, out Kelvin output) => TryFrom(celsius.ToKelvin(), out output);
        public static Validation TryFrom(Fahrenheit fahrenheit, out Kelvin output) => TryFrom(fahrenheit.ToKelvin(), out output);
        public static Validation TryFrom(Reaumur reaumur, out Kelvin output) => TryFrom(reaumur.ToKelvin(), out output);

        public static Validation Validate(double value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            return Validation.Ok;
        }

        private static double FromCelsius(double celsius) => celsius + 273.15d;
        private static double FromFahrenheit(double fahrenheit) => (fahrenheit + 459.67d) * 5d / 9d;
        private static double FromReaumur(double reaumur) => (reaumur * 9 / 8) + 273.15;

        private static double ToCelsius(double kelvin) => kelvin - 273.15d;
        private static double ToFahrenheit(double kelvin) => (kelvin - 273.15d) * 9d / 5d + 32d;
        private static double ToReaumur(double kelvin) => (kelvin - 273.15) * 4 / 5;

        #endregion
    }

    public static class KelvinExtensions
    {
        public static double ToCelsius(this Kelvin kelvin) => kelvin - 273.15d;
        public static double ToFahrenheit(this Kelvin kelvin) => (kelvin - 273.15d) * 9d / 5d + 32d;
        public static double ToReaumur(this Kelvin kelvin) => (kelvin - 273.15) * 4 / 5;
    }

    public class InvalidKelvinException : Exception
    {
        public InvalidKelvinException()
        {
        }

        public InvalidKelvinException(string message) : base(message)
        {
        }

        public InvalidKelvinException(double value)
            : base($"The value '{value}' is not a valid {nameof(Kelvin)} temperature!")
        {
        }
    }
}
