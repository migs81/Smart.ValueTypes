using System;
using System.Numerics;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Numeric
{
    /// <summary>
    /// Value type for a numeric value that is guaranteed to be within a specified minimum and maximum range
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidBoundedNumberException"></exception>
    public readonly record struct BoundedNumber<T> : IValueType<T, BoundedNumber<T>> 
        where T : INumber<T>
    {
        #region fields

        private readonly T _value;

        public enum Validation
        {
            Ok = 0,
            InvalidBounds,
            ValueTooLow,
            ValueTooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value == T.Zero && Min == T.Zero && Max == T.Zero;
        
        public T Min { get; }
        
        public T Max { get; }
        
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedNumber{T}"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <exception cref="InvalidBoundedNumberException"></exception>
        public BoundedNumber(T value, T minValue, T maxValue)
        {
            var result = ValidateFormat(ref value, ref minValue, ref maxValue);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.InvalidBounds => new InvalidBoundedNumberException($"The values 'Min: {Min}' and 'Max: {Max}' are no valid boundaries for a {nameof(BoundedNumber<T>)}!"),
                    Validation.ValueTooLow => new InvalidBoundedNumberException($"The value '{value}' is too low for a {nameof(BoundedNumber<T>)}!"),
                    Validation.ValueTooHigh => new InvalidBoundedNumberException($"The value '{value}' is too high for a {nameof(BoundedNumber<T>)}!"),
                    _ => new InvalidBoundedNumberException(),
                };
            }

            _value = value;
            Min = minValue;
            Max = maxValue;
        }
        
        // required for internal initialization
        private BoundedNumber(ref T value, ref T minValue, ref T maxValue)
        {
            _value = value;
            Min = minValue;
            Max = maxValue;
        }

        #endregion

        #region operator

        public static bool operator ==(BoundedNumber<T> left, T right) => left._value.Equals(right);
        public static bool operator !=(BoundedNumber<T> left, T right) => !left._value.Equals(right);

        public static implicit operator T(BoundedNumber<T> range) => range._value;

        #endregion

        #region public methods

        public static BoundedNumber<T> From(T value, T minValue, T maxValue) => new(value, minValue, maxValue);

        public static Validation TryFrom(T value, T minValue, T maxValue, out BoundedNumber<T> output)
        {
            try
            {
                var result = ValidateFormat(ref value, ref minValue, ref maxValue);
                if (result == Validation.Ok)
                {
                    output = new BoundedNumber<T>(ref value, ref minValue, ref maxValue);
                    return Validation.Ok;
                }

                output = default;
                return result;
            }
            catch (Exception)
            {
                output = default;
                return Validation.UnknownError;
            }
        }

        public static Validation ValidateFormat(T value, T minValue, T maxValue) => 
            ValidateFormat(ref value, ref minValue, ref maxValue);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref T value, ref T minValue, ref T maxValue)
        {
            if (minValue > maxValue)
                return Validation.InvalidBounds;
            
            if (value < minValue)
                return Validation.ValueTooLow;

            if (value > maxValue)
                return Validation.ValueTooHigh;

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidBoundedNumberException : Exception
    {
        public InvalidBoundedNumberException()
        {
        }

        public InvalidBoundedNumberException(string message) : base(message)
        {
        }
    }
}
