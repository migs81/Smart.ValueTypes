using System;
using System.Collections.Generic;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Unfinished
{
    public readonly record struct Text : IValueType<string, Text>
    {
        #region fields

        private readonly string _value;

        public enum Validation
        {
            Ok = 0,
            ValueIsNull,
            UnknownError
        }

        #endregion

        #region properties

        public static Text Empty { get; } = new Text("");

        #endregion

        #region constructor

        public Text() => _value = Empty;
        public Text(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.ValueIsNull => new ArgumentNullException(nameof(value)),
                    _ => new InvalidTextException(),
                };
            }

            _value = value;
        }
        private Text(ref string value) => _value = value;

        #endregion

        #region operators

        public static bool operator ==(Text left, string right) => left.Equals(right);
        public static bool operator !=(Text left, string right) => !left.Equals(right);

        public static implicit operator string(Text text) => text._value;
        public static implicit operator Text(string value) => new(value);

        #endregion

        #region public methods

        public static Text From(string value) => new(value);

        public static Validation TryFrom(string value, out Text text)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if ( result == Validation.Ok)
                {
                    text = new Text(ref value);
                    return Validation.Ok;
                }

                text = Empty;
                return result;
            }
            catch (Exception)
            {
                text = Empty;
                return Validation.UnknownError;
            }
        }

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value) => value is null
            ? Validation.ValueIsNull
            : Validation.Ok;

        #endregion
    }

    public class InvalidTextException : Exception
    {
        public InvalidTextException()
        {
        }

        public InvalidTextException(string message) : base(message)
        {
        }
    }
}
