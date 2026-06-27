using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Cyclic Redundancy Check 32 (CRC32).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidCrc32Exception"></exception>
    public readonly record struct CRC32 : IValueType<string, CRC32>
    {
        #region fields

        private readonly string _value;
        private const int _hashLength = 8;
        private readonly string _default = new('0', _hashLength);

        public enum Validation
        {
            Ok = 0,
            Null,
            WrongLength,
            IllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public static CRC32 Default => new();
        public int Length => _value.Length;

        #endregion

        #region constructor

        public CRC32() => _value = _default;
        public CRC32(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidCrc32Exception($"The value '{value}' is not {_hashLength} characters long!"),
                    Validation.IllegalCharacter => new InvalidCrc32Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidCrc32Exception(),
                };
            }

            _value = value;
        }
        private CRC32(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(CRC32 left, string right) => left.Equals(right);
        public static bool operator !=(CRC32 left, string right) => !left.Equals(right);

        public static implicit operator string(CRC32 hash) => hash._value;
        public static implicit operator CRC32(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(_value, value);

        #endregion

        #region public static methods

        public static CRC32 From(string value) => new(value);
        public static Validation TryFrom(string value, out CRC32 output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new CRC32(ref value);
                    return Validation.Ok;
                }

                output = Default;
                return result;
            }
            catch (Exception)
            {
                output = Default;
                return Validation.UnknownError;
            }
        }

        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            if (value is null)
                return Validation.Null;

            if (value.Length != _hashLength)
                return Validation.WrongLength;

            ReadOnlySpan<char> span = value.AsSpan();
            foreach (var c in span)
            {
                if (c is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                    return Validation.IllegalCharacter;
            }

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidCrc32Exception : Exception
    {
        public InvalidCrc32Exception()
        {
        }

        public InvalidCrc32Exception(string message) : base(message)
        {
        }
    }
}
