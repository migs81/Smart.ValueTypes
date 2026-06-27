using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migs.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Secure Hash Algorithm 1 (SHA1).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSha1Exception"></exception>
    public readonly record struct SHA1 : IValueType<string, SHA1>
    {
        #region fields

        private readonly string _value;
        private const int HashLength = 40;
        private readonly string _default = new('0', HashLength);

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

        public static SHA1 Empty => new();
        public int Length => _value.Length;

        #endregion

        #region constructor

        public SHA1() => _value = _default;
        public SHA1(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSha1Exception($"The value '{value}' is not {HashLength} characters long!"),
                    Validation.IllegalCharacter => new InvalidSha1Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSha1Exception(),
                };
            }

            _value = value;
        }
        private SHA1(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(SHA1 left, string right) => left.Equals(right);
        public static bool operator !=(SHA1 left, string right) => !left.Equals(right);

        public static implicit operator string(SHA1 hash) => hash._value;
        public static implicit operator SHA1(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

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

                output = Empty;
                return result;
            }
            catch (Exception)
            {
                output = Empty;
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
                    var SHA1 = CreateHash(ref value);
                    if (ValidateFormat(ref SHA1) == Validation.Ok)
                    {
                        output = new SHA1(ref SHA1);
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
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static Validation ValidateFormat(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 40 characters long
            if (value.Length != HashLength)
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
