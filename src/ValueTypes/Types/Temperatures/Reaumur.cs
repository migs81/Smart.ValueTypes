using Smart.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Smart.ValueTypes.Types.Temperatures
{
    /// <summary>
    /// Value type for celsius temperatures.
    /// </summary>
    /// <exception cref="InvalidReaumurException"></exception>
    public readonly record struct Reaumur : IValueType<double, Reaumur>, ITemperature
    {
        #region fields

        public const double MinValue = -218.52d;
        public const double MaxValue = double.MaxValue;
        public const double FreezingPoint = 0.0d;
        public const double BoilingPoint = 80.0d;
        public const string Unit = "°R";

        public enum Validation
        {
            Ok = 0,
            TooLow,
            UnknownError
        }

        #endregion

        #region properties

        public double Value { get; }
        public static Reaumur Zero => new(0d);

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Reaumur"/> struct.
        /// </summary>
        public Reaumur() => Value = 0d;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Reaumur"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidReaumurException"></exception>
        public Reaumur(double value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.TooLow => new InvalidReaumurException($"The value '{value}' is too low for a {nameof(Reaumur)} temperature!"),
                    _ => new InvalidReaumurException(),
                };
            }

            Value = value;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Reaumur"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidReaumurException"></exception>
        public Reaumur(int value) : this((double)value) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Reaumur"/> struct.
        /// </summary>
        /// <param name="celsius"></param>
        /// <exception cref="InvalidReaumurException"></exception>
        public Reaumur(Celsius celsius) : this(FromCelsius(celsius)) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Reaumur"/> struct.
        /// </summary>
        /// <param name="kelvin"></param>
        /// <exception cref="InvalidReaumurException"></exception>
        public Reaumur(Kelvin kelvin) : this(FromKelvin(kelvin)) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Reaumur"/> struct.
        /// </summary>
        /// <param name="fahrenheit"></param>
        /// <exception cref="InvalidReaumurException"></exception>
        public Reaumur(Fahrenheit fahrenheit) : this(FromFahrenheit(fahrenheit)) { }
        
        // required for internal initialization
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

        public static Reaumur From(int value) => new(value);
        public static Reaumur From(double value) => new(value);
        public static Reaumur From(Celsius celsius) => new(celsius);
        public static Reaumur From(Kelvin kelvin) => new(kelvin);
        public static Reaumur From(Fahrenheit fahrenheit) => new(fahrenheit);

        public static Validation TryFrom(double value, out Reaumur output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Reaumur(ref value);
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
        public static Validation TryFrom(int value, out Reaumur output) => TryFrom((double)value, out output);
        public static Validation TryFrom(Celsius celsius, out Reaumur output) => TryFrom(FromCelsius(celsius), out output);
        public static Validation TryFrom(Fahrenheit fahrenheit, out Reaumur output) => TryFrom(FromFahrenheit(fahrenheit), out output);
        public static Validation TryFrom(Kelvin kelvin, out Reaumur output) => TryFrom(FromKelvin(kelvin), out output);

        public double ToCelsius() => Value * 5 / 4;
        public double ToKelvin() => (Value * 9 / 8) + 273.15;
        public double ToFahrenheit() => (Value * 9 / 4) + 32;

        public static Validation Validate(double value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref double value)
        {
            if (value < MinValue)
                return Validation.TooLow;

            return Validation.Ok;
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
