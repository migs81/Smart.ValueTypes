using Smart.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Secure Hash Algorithm 1 (SHA1).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSha1Exception"></exception>
    public readonly record struct SHA1 : IValueType<string, SHA1>
    {
        #region fields

        private readonly string? _value;

        public const int Length = 40;
        
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

        public bool IsDefault => _value is null;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SHA1"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidSha1Exception"></exception>
        public SHA1(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSha1Exception($"The value '{value}' is not {Length} characters long!"),
                    Validation.IllegalCharacter => new InvalidSha1Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSha1Exception(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private SHA1(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(SHA1 left, string right) => left.Equals(right);
        public static bool operator !=(SHA1 left, string right) => !left.Equals(right);

        public static implicit operator string(SHA1 hash) => hash._value ?? "";
        public static implicit operator SHA1(string value) => new(value);

        #endregion

        #region public methods

        public static SHA1 From(string hash) => new(hash);

        public static Validation TryFrom(string hash, out SHA1 output)
        {
            try
            {
                var result = ValidateFormat(ref hash);
                if (result == Validation.Ok)
                {
                    output = new SHA1(ref hash);
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

        public static SHA1 Create(string value) => new(CreateHash(ref value));
        
        public static bool TryCreate(string value, out SHA1? output)
        {
            try
            {
                if (value is not null)
                {
                    var hash = CreateHash(ref value);
                    if (ValidateFormat(ref hash) == Validation.Ok)
                    {
                        output = new SHA1(ref hash);
                        return true;
                    }
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

        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static string CreateHash(ref string input)
        {
            var bytes = System.Security.Cryptography.SHA1.HashData(Encoding.Default.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        private static Validation ValidateFormat(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 40 characters long
            if (value.Length != Length)
                return Validation.WrongLength;

            // must be hex
            foreach (var c in value.AsSpan())
            {
                if (c is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                    return Validation.IllegalCharacter;
            }

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidSha1Exception : Exception
    {
        public InvalidSha1Exception()
        {
        }

        public InvalidSha1Exception(string message) : base(message)
        {
        }
    }
}
