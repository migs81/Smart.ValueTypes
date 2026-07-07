using System;
using System.Collections.Generic;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Unfinished
{
    /// <summary>
    /// Value type for Url's.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, Url&gt;" />
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UriFormatException"></exception>
    public readonly record struct Url : IValueType<string, Url>
    {
        #region fields

        private readonly string _value;

        #endregion

        #region constructor

        public Url() => _value = "/";
        public Url(string value)
        {
            if (!Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out _))
                throw new ArgumentException(nameof(value));
            //_ = new Uri(value,UriKind.RelativeOrAbsolute);
            _value = value;
        }
        private Url(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Url left, string right) => left.Equals(right);
        public static bool operator !=(Url left, string right) => !left.Equals(right);

        public static implicit operator string(Url address) => address._value;
        public static implicit operator Url(string value) => new(value);

        #endregion

        #region public methods

        public Uri GetUri() => new(_value);

        public static Url Parse(string value) => new(value);
        
        public static bool TryParse(string value, out Url? output)
        {
            try
            {
                if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out _))
                {
                    output = new Url(ref value);
                    return true;
                }

                output = null;
                return true;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }

        #endregion
    }
}
