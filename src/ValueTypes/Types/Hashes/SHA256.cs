using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migs.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Secure Hash Algorithm 256 (SHA256).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSHA256Exception"></exception>
    public readonly record struct SHA256 : IValueType<string, SHA256>
    {
        #region fields

        private readonly string _value;
        private const int _hashLength = 64;
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

        public static SHA256 Default => new();
        public int Length => _value.Length;

        #endregion

        #region constructor

        public SHA256() => _value = _default;
        public SHA256(string value)
        {
            var result = Validate(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSHA256Exception($"The value '{value}' is not {_hashLength} characters long!"),
                    Validation.IllegalCharacter => new InvalidSHA256Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSHA256Exception(),
                };
            }

            _value = value;
        }
        private SHA256(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(SHA256 left, string right) => left.Equals(right);
        public static bool operator !=(SHA256 left, string right) => !left.Equals(right);

        public static implicit operator string(SHA256 hash) => hash._value;
        public static implicit operator SHA256(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static SHA256 From(string hash) => new(hash);
        public static Validation TryFrom(string hash, out SHA256 output)
        {
            try
            {
                var result = Validate(ref hash);
                if (result == Validation.Ok)
                {
                    output = new SHA256(ref hash);
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

        public static SHA256 Create(string value) => new(CreateHash(ref value));
        public static bool TryCreate(string value, out SHA256? output)
        {
            try
            {
                if (value is not null)
                {
                    string SHA256 = CreateHash(ref value);
                    if (Validate(ref SHA256) == Validation.Ok)
                    {
                        output = new SHA256(ref SHA256);
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

        public static Validation Validate(string value) => Validate(ref value);

        #endregion

        #region private methods

        private static string CreateHash(ref string input)
        {
            byte[] bytes = System.Security.Cryptography.SHA256.HashData(Encoding.Default.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static Validation Validate(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 64 characters long
            if (value.Length != _hashLength)
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

    public class InvalidSHA256Exception : Exception
    {
        public InvalidSHA256Exception()
        {
        }

        public InvalidSHA256Exception(string message) : base(message)
        {
        }
    }
}
