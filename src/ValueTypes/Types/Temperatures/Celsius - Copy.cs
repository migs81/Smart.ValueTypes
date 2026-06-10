//using Migs.ValueTypes.Interfaces;
//using System;
//using System.Collections.Generic;

//namespace Migs.ValueTypes.Types.Temperatures
//{
//    /// <summary>
//    /// Value type for celsius temperatures.
//    /// </summary>
//    /// <seealso cref="ITemperature&lt;double, Celsius&gt;" />
//    /// <exception cref="InvalidCelsiusException"></exception>
//    public readonly partial record struct Celsius : IValueType<double, Celsius>, ITemperature
//    {
//        #region fields

//        private readonly double _value;
//        private const double _defaultValue = 0.0d;

//        public const double MinValue = -273.15d;
//        public const double MaxValue = double.MaxValue;
//        public const double FreezingPoint = 0.0d;
//        public const double BoilingPoint = 100.0d;
//        public const string Unit = "°C";

//        public enum Validation
//        {
//            OK = 0,
//            TooLow,
//            UnknownError
//        }

//        #endregion

//        #region properties

//        public double Value => _value;

//        public static Celsius Default => new();

//        #endregion

//        #region constructor

//        public Celsius() => _value = _defaultValue;
//        public Celsius(double value)
//        {
//            var result = Validate(ref value);
//            if (result != Validation.OK)
//            {
//                throw result switch
//                {
//                    Validation.TooLow => new InvalidCelsiusException($"The value '{value}' is too low for a {nameof(Celsius)} temperature!"),
//                    _ => new InvalidCelsiusException(),
//                };
//            }

//            _value = value;
//        }
//        public Celsius(int value) : this((double)value) { }
//        public Celsius(Kelvin kelvin) => _value = FromKelvin(kelvin);
//        public Celsius(Fahrenheit fahrenheit) => _value = FromFahrenheit(fahrenheit);
//        public Celsius(Reaumur reaumur) => _value = FromReaumur(reaumur);
//        private Celsius(ref double value) => _value = value;

//        #endregion
        
//        #region operator

//        public static bool operator ==(Celsius left, double right) => left.Equals(right);
//        public static bool operator !=(Celsius left, double right) => !left.Equals(right);

//        public static implicit operator double(Celsius left) => left._value;
//        public static implicit operator Celsius(double value) => new(value);
//        public static implicit operator Kelvin(Celsius celsius) => new(ToKelvin(celsius));
//        public static implicit operator Fahrenheit(Celsius celsius) => new(ToFahrenheit(celsius));
//        public static implicit operator Reaumur(Celsius celsius) => new(ToReaumur(celsius));

//        #endregion

//        #region public methods

//        public bool Equals(double other) => EqualityComparer<double>.Default.Equals(_value, other);

//        public static Celsius From(int value) => new(value);
//        public static Celsius From(double value) => new(value);
//        public static Celsius From(Kelvin kelvin) => new(kelvin);
//        public static Celsius From(Fahrenheit fahrenheit) => new(fahrenheit);
//        public static Celsius From(Reaumur reaumur) => new(reaumur);

//        public static Validation TryFrom(double value, out Celsius output)
//        {
//            try
//            {
//                var result = Validate(ref value);
//                if (result == Validation.OK)
//                {
//                    output = new Celsius(ref value);
//                    return Validation.OK;
//                }

//                output = Default;
//                return result;
//            }
//            catch (Exception)
//            {
//                output = Default;
//                return Validation.UnknownError;
//            }
//        }
//        public static Validation TryFrom(int value, out Celsius output) => TryFrom((double)value, out output);
//        public static Validation TryFrom(Kelvin kelvin, out Celsius output) => TryFrom(kelvin.ToCelsius(), out output);
//        public static Validation TryFrom(Fahrenheit fahrenheit, out Celsius output) => TryFrom(fahrenheit.ToCelsius(), out output);
//        public static Validation TryFrom(Reaumur reaumur, out Celsius output) => TryFrom(reaumur.ToCelsius(), out output);

//        public static Validation Validate(double value) => Validate(ref value);

//        #endregion

//        #region private methods

//        private static Validation Validate(ref double value)
//        {
//            if (value < MinValue)
//                return Validation.TooLow;

//            return Validation.OK;
//        }

//        private static double FromKelvin(double kelvin) => kelvin - 273.15d;
//        private static double FromFahrenheit(double fahrenheit) => (fahrenheit - 32d) * 5d / 9d;
//        private static double FromReaumur(double reaumur) => reaumur * 5 / 4;

//        private static double ToKelvin(double celsius) => celsius + 273.15d;
//        private static double ToFahrenheit(double celsius) => (celsius * 9d / 5d) + 32d;
//        private static double ToReaumur(double celsius) => celsius * 4 / 5;

//        #endregion
//    }

//    public static class CelsiusExtensions
//    {
//        public static double ToKelvin(this Celsius celsius) => celsius + 273.15;
//        public static double ToFahrenheit(this Celsius celsius) => (celsius * 9 / 5) + 32;
//        public static double ToReaumur(this Celsius celsius) => celsius * 4 / 5;
//    }

//    public class InvalidCelsiusException : Exception
//    {
//        public InvalidCelsiusException()
//        {
//        }

//        public InvalidCelsiusException(string message) : base(message)
//        {
//        }

//        public InvalidCelsiusException(double value)
//            : base($"The value '{value}' is not a valid {nameof(Celsius)} temperature!")
//        {
//        }
//    }
//}
