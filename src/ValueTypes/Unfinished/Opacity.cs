using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes
{
    /// <summary>
    /// Value type for opacity.
    /// </summary>
    /// <seealso cref="IValueType&lt;double, Opacity&gt;" />
    /// <exception cref="InvalidOpacityException"></exception>
    public readonly record struct Opacity : IValueType<double, Opacity>
    {
        #region fields

        private readonly double _value;
        #endregion

        #region properties

        public static Opacity Default { get; } = new(0);

        #endregion

        #region constructor

        public Opacity() => _value = 0;
        public Opacity(double value)
        {
            if (!IsValid(ref value))
                throw new InvalidOpacityException($"The value '{value}' is not a valid opacity type!");

            _value = value;
        }
        private Opacity(ref double value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Opacity left, double right) => left.Equals(right);
        public static bool operator !=(Opacity left, double right) => !left.Equals(right);

        public static implicit operator double(Opacity left) => left._value;
        public static implicit operator Opacity(double value) => new(value);

        #endregion

        #region public methods

        public bool Equals(double other) => EqualityComparer<double>.Default.Equals(_value, other);

        public static Opacity Parse(double value) => new(value);
        public static bool TryParse(double value, out Opacity? output)
        {
            try
            {
                if (IsValid(ref value))
                {
                    output = new Opacity(ref value);
                    return true;
                }

                output = null;
                return false;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }

        #endregion

        #region private methods

        private static bool IsValid(ref double value) => value is >= 0 and <= 1;

        #endregion
    }

    public class InvalidOpacityException : Exception
    {
        public InvalidOpacityException()
        {
        }

        public InvalidOpacityException(string message)
        {
        }
    }
}
