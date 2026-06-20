using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migs.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Secure Hash Algorithm 384 (SHA384).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSHA384Exception"></exception>
    public readonly record struct SHA384 : IValueType<string, SHA384>
    {
        #region fields

        private readonly string _value;
        private const int _hashLength = 96;
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

        public static SHA384 Default => new();
        public int Length => _value.Length;

        #endregion

        #region constructor

        public SHA384() => _value = _default;
        public SHA384(string value)
        {
            var result = Validate(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSHA384Exception($"The value '{value}' is not {_hashLength} characters long!"),
                    Validation.IllegalCharacter => new InvalidSHA384Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSHA384Exception(),
                };
            }

            _value = value;
        }
        private SHA384(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(SHA384 left, string right) => left.Equals(right);
        public static bool operator !=(SHA384 left, string right) => !left.Equals(right);

        public static implicit operator string(SHA384 hash) => hash._value;
        public static implicit operator SHA384(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static SHA384 From(string hash) => new(hash);
        public static Validation TryFrom(string hash, out SHA384 output)
        {
            try
            {
                var result = Validate(ref hash);
                if (result == Validation.Ok)
                {
                    output = new SHA384(ref hash);
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

        public static SHA384 Create(string value) => new(CreateHash(ref value));
        public static bool TryCreate(string value, out SHA384? output)
        {
            try
            {
                if (value is not null)
                {
                    string SHA384 = CreateHash(ref value);
                    if (Validate(ref SHA384) == Validation.Ok)
                    {
                        output = new SHA384(ref SHA384);
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
            byte[] bytes = System.Security.Cryptography.SHA384.HashData(Encoding.Default.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static Validation Validate(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 96 characters long
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

    public class InvalidSHA384Exception : Exception
    {
        public InvalidSHA384Exception()
        {
        }

        public InvalidSHA384Exception(string message) : base(message)
        {
        }
    }
}
