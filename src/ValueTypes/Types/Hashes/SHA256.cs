using Smart.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Secure Hash Algorithm 256 (SHA256).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSha256Exception"></exception>
    public readonly record struct SHA256 : IValueType<string, SHA256>
    {
        #region fields

        private readonly string _value;
        private const string Default = "0000000000000000000000000000000000000000000000000000000000000000";
        
        public const int Length = 64;

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

        public static SHA256 Empty => new();

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SHA256"/> struct.
        /// </summary>
        public SHA256() => _value = Default;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SHA256"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidSha256Exception"></exception>
        public SHA256(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSha256Exception($"The value '{value}' is not {Length} characters long!"),
                    Validation.IllegalCharacter => new InvalidSha256Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSha256Exception(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private SHA256(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(SHA256 left, string right) => left.Equals(right);
        public static bool operator !=(SHA256 left, string right) => !left.Equals(right);

        public static implicit operator string(SHA256 hash) => hash._value;
        public static implicit operator SHA256(string value) => new(value);

        #endregion

        #region public methods

        public static SHA256 From(string hash) => new(hash);
        
        public static Validation TryFrom(string hash, out SHA256 output)
        {
            try
            {
                var result = ValidateFormat(ref hash);
                if (result == Validation.Ok)
                {
                    output = new SHA256(ref hash);
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

        public static SHA256 Create(string value) => new(CreateHash(ref value));
        
        public static bool TryCreate(string value, out SHA256? output)
        {
            try
            {
                if (value is not null)
                {
                    var hash = CreateHash(ref value);
                    if (ValidateFormat(ref hash) == Validation.Ok)
                    {
                        output = new SHA256(ref hash);
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
            var bytes = System.Security.Cryptography.SHA256.HashData(Encoding.Default.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        private static Validation ValidateFormat(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 64 characters long
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

    public class InvalidSha256Exception : Exception
    {
        public InvalidSha256Exception()
        {
        }

        public InvalidSha256Exception(string message) : base(message)
        {
        }
    }
}
