//using Migs.ValueTypes.Interfaces;
//using System;
//using System.Collections.Generic;

//namespace Migs.ValueTypes.Types.Tempreratures
//{
//    /// <summary>
//    /// Value type for celsius temperatures.
//    /// </summary>
//    /// <seealso cref="ITemprerature&lt;double, Tempr&gt;" />
//    /// <exception cref="InvalidTemprException"></exception>
//    public readonly partial record struct Tempr : IValueType<double, Tempr>, ITemperature
//    {
//        #region fields

//        private readonly double _value;

//        public enum Validation
//        {
//            OK = 0,
//            TooLow,
//            UnknownError
//        }

//        public enum UnitType
//        {
//            Celsius = 0,
//            Kelvin,
//            Fahrenheit,
//            Reaumur
//        }

//        #endregion

//        #region properties

//        public double Value => _value;
//        public static double MinValue => 0.0d;
//        public static double MaxValue => double.MaxValue;
//        public static double FreezingPoint => 0.0d;
//        public static double BoilingPoint => 0.0d;
//        public static UnitType Unit => UnitType.Celsius;

//        #endregion

//        #region constructor

//        private Tempr(double value, UnitType unit)
//        {
//            var result = Validate(ref value, unit);
//            if (result != Validation.OK)
//            {
//                throw result switch
//                {
//                    Validation.TooLow => new InvalidTemprException($"The value '{value}' is too low for a {unit} temperature!"),
//                    _ => new InvalidTemprException(),
//                };
//            }

//            _value = value;
//        }
//        private Tempr(ref double value) => _value = value;

//        #endregion

//        #region operator

//        public static bool operator ==(Tempr left, double right) => left.Equals(right);
//        public static bool operator !=(Tempr left, double right) => !left.Equals(right);

//        public static implicit operator double(Tempr left) => left._value;
//        public static implicit operator Tempr(double value) => new(value, Unit);

//        #endregion

//        #region public methods

//        public bool Equals(double other) => EqualityComparer<double>.Default.Equals(_value, other);

//        public static Tempr From(UnitType unit, double value) => new(value, unit);

//        public static Validation TryFrom(UnitType unit, double value, out Tempr output)
//        {
//            try
//            {
//                var result = Validate(ref value, unit);
//                if (result == Validation.OK)
//                {
//                    output = new Tempr(ref value);
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
        
//        #endregion

//        #region private methods

//        private static Validation Validate(ref double value, UnitType unit)
//        {
//            if (value < MinValue)
//                return Validation.TooLow;

//            return Validation.OK;
//        }

//        private static double From(UnitType unit,double kelvin) => kelvin - 273.15d;

//        private static double To(UnitType unit, double celsius) => celsius + 273.15d;

//        #endregion
//    }

//    public static class TemprExtensions
//    {
//        public static double ToKelvin(this Tempr celsius) => celsius + 273.15;
//        public static double ToFahrenheit(this Tempr celsius) => (celsius * 9 / 5) + 32;
//        public static double ToReaumur(this Tempr celsius) => celsius * 4 / 5;
//    }

//    public class InvalidTemprException : Exception
//    {
//        public InvalidTemprException()
//        {
//        }

//        public InvalidTemprException(string message) : base(message)
//        {
//        }

//        public InvalidTemprException(double value)
//            : base($"The value '{value}' is not a valid {nameof(Tempr)} temperature!")
//        {
//        }
//    }
//}
