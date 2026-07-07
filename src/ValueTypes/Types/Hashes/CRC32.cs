using Smart.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Smart.ValueTypes.Types.Hashes
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
        private const string Default = "00000000";
        
        public const int Length = 8;
        
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

        public static CRC32 Empty => new();

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC32"/> struct.
        /// </summary>
        public CRC32() => _value = Default;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CRC32"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCrc32Exception"></exception>
        public CRC32(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidCrc32Exception($"The value '{value}' is not {Length} characters long!"),
                    Validation.IllegalCharacter => new InvalidCrc32Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidCrc32Exception(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private CRC32(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(CRC32 left, string right) => left.Equals(right);
        public static bool operator !=(CRC32 left, string right) => !left.Equals(right);

        public static implicit operator string(CRC32 hash) => hash._value;
        public static implicit operator CRC32(string value) => new(value);

        #endregion

        #region public methods

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

                output = Empty;
                return result;
            }
            catch (Exception)
            {
                output = Empty;
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

            if (value.Length != Length)
                return Validation.WrongLength;

            var span = value.AsSpan();
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
