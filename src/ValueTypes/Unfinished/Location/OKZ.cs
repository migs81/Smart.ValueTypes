using System;
using System.Collections.Generic;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Unfinished.Location
{
    /// <summary>
    /// Value type for the austrian location code (Ortskennzahl).
    /// </summary>
    /// <seealso cref="IValueType&lt;int, OKZ&gt;" />
    /// <exception cref="InvalidOKZException"></exception>
    public readonly record struct OKZ : IValueType<int, OKZ>
    {
        #region fields

        private readonly int _value;

        #endregion

        #region constructor

        public OKZ() => _value = 0;
        public OKZ(int value)
        {
            if (!ValidateFormat(ref value))
                throw new InvalidOKZException(value);

            _value = value;
        }
        private OKZ(ref int value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(OKZ left, int right) => left.Equals(right);
        public static bool operator !=(OKZ left, int right) => !left.Equals(right);

        public static implicit operator int(OKZ left) => left._value;
        public static implicit operator OKZ(int value) => new(value);

        #endregion

        #region public methods

        public static OKZ Parse(int value) => new(value);
        public static bool TryParse(int value, out OKZ? output)
        {
            try
            {
                if (ValidateFormat(ref value))
                {
                    output = new OKZ(ref value);
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

        public static bool ValidateFormat(int value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static bool ValidateFormat(ref int value) => value is >= 1 and <= 99999;

        #endregion
    }

    public class InvalidOKZException : Exception
    {
        public InvalidOKZException()
        {
        }

        public InvalidOKZException(int value)
            : base($"The value '{value}' is not a valid OKZ!")
        {
        }
    }
}
