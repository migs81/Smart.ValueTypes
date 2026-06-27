using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migs.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Secure Hash Algorithm 512 (SHA512).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSha512Exception"></exception>
    public readonly record struct SHA512 : IValueType<string, SHA512>
    {
        #region fields

        private readonly string _value;
        private const int HashLength = 128;
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

        public static SHA512 Empty => new();
        public int Length => _value.Length;

        #endregion

        #region constructor

        public SHA512() => _value = _default;
        public SHA512(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSha512Exception($"The value '{value}' is not {HashLength} characters long!"),
                    Validation.IllegalCharacter => new InvalidSha512Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSha512Exception(),
                };
            }

            _value = value;
        }
        private SHA512(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(SHA512 left, string right) => left.Equals(right);
        public static bool operator !=(SHA512 left, string right) => !left.Equals(right);

        public static implicit operator string(SHA512 hash) => hash._value;
        public static implicit operator SHA512(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static SHA512 From(string hash) => new(hash);
        public static Validation TryFrom(string value, out SHA512 output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new SHA512(ref value);
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

        public static SHA512 Create(string value) => new(CreateHash(ref value));
        public static bool TryCreate(string value, out SHA512? output)
        {
            try
            {
                if (value is not null)
                {
                    string SHA512 = CreateHash(ref value);
                    if (ValidateFormat(ref SHA512) == Validation.Ok)
                    {
                        output = new SHA512(ref SHA512);
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
            byte[] bytes = System.Security.Cryptography.SHA512.HashData(Encoding.Default.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static Validation ValidateFormat(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 128 characters long
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

    public class InvalidSha512Exception : Exception
    {
        public InvalidSha512Exception()
        {
        }

        public InvalidSha512Exception(string message) : base(message)
        {
        }
    }
}
