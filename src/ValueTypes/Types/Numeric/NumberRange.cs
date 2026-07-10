using System;
using System.Numerics;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Numeric
{
    /// <summary>
    /// Value type for a numeric range
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidNumberRangeException"></exception>
    public readonly record struct NumberRange<T> : IValueType<T, NumberRange<T>> 
        where T : INumber<T>
    {
        #region fields

        public enum Validation
        {
            Ok = 0,
            InvalidRange,
            UnknownError
        }

        #endregion

        #region properties

        public static NumberRange<T> Empty => new();

        public T Min { get; }
        
        public T Max { get; }
        
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberRange{T}"/> struct.
        /// </summary>
        public NumberRange()
        {
            Min = T.Zero;
            Max = T.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberRange{T}"/> struct.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <exception cref="InvalidNumberRangeException"></exception>
        public NumberRange(T minValue, T maxValue)
        {
            var result = ValidateFormat(ref minValue, ref maxValue);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.InvalidRange => new InvalidNumberRangeException($"The values 'Min: {Min}' and 'Max: {Max}' are not a valid {nameof(NumberRange<T>)}!"),
                    _ => new InvalidNumberRangeException(),
                };
            }

            Min = minValue;
            Max = maxValue;
        }
        
        // required for internal initialization
        private NumberRange(ref T minValue, ref T maxValue)
        {
            Min = minValue;
            Max = maxValue;
        }

        #endregion

        #region public methods

        public static NumberRange<T> From(T minValue, T maxValue) => new(minValue, maxValue);

        public static Validation TryFrom(T minValue, T maxValue, out NumberRange<T> output)
        {
            try
            {
                var result = ValidateFormat(ref minValue, ref maxValue);
                if (result == Validation.Ok)
                {
                    output = new NumberRange<T>(ref minValue, ref maxValue);
                    return Validation.Ok;
                }

                output = Empty;
                return result;
            }
            catch (Exception)
            {
                output = Empty;
                return Validation.UnknownError;
            }
        }

        public static Validation ValidateFormat(T minValue, T maxValue) => 
            ValidateFormat(ref minValue, ref maxValue);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref T minValue, ref T maxValue)
        {
            if (minValue > maxValue)
                return Validation.InvalidRange;

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidNumberRangeException : Exception
    {
        public InvalidNumberRangeException()
        {
        }

        public InvalidNumberRangeException(string message) : base(message)
        {
        }
    }
}
