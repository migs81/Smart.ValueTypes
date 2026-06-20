using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes
{
    /// <summary>
    /// Value type for the austrian zip code (Postleitzahl).
    /// </summary>
    /// <seealso cref="IValueType&lt;int, Plz&gt;" />
    /// <exception cref="InvalidPlzException"></exception>
    public readonly record struct PLZ : IValueType<int, PLZ>
    {
        #region fields

        private readonly int _value;

        #endregion

        #region constructor

        public PLZ() => _value = 0;
        public PLZ(int value)
        {
            if (!Validate(ref value))
                throw new InvalidPlzException(value);

            _value = value;
        }
        private PLZ(ref int value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(PLZ left, int right) => left.Equals(right);
        public static bool operator !=(PLZ left, int right) => !left.Equals(right);

        public static implicit operator int(PLZ left) => left._value;
        public static implicit operator PLZ(int value) => new(value);

        #endregion

        #region public methods

        public static PLZ Parse(int value) => new(value);
        public static bool TryParse(int value, out PLZ? output)
        {
            try
            {
                if (Validate(ref value))
                {
                    output = new PLZ(ref value);
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

        public bool Equals(int other) => EqualityComparer<int>.Default.Equals(_value, other);

        public static bool Validate(int value) => Validate(ref value);

        #endregion

        #region private methods

        private static bool Validate(ref int value) => value is >= 1000 and <= 9999;

        #endregion
    }

    public class InvalidPlzException : Exception
    {
        public InvalidPlzException()
        {
        }

        public InvalidPlzException(int value)
            : base($"The value '{value}' is not a valid PLZ!")
        {
        }
    }
}
