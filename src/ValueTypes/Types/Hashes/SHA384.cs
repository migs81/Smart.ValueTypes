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
    /// <exception cref="InvalidSha384Exception"></exception>
    public readonly record struct SHA384 : IValueType<string, SHA384>
    {
        #region fields

        private readonly string _value;
        private const string Default = "000000000000000000000000000000000000000000000000" +
                                       "000000000000000000000000000000000000000000000000";
        
        public const int Length = 96;

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

        public static SHA384 Empty => new();

        #endregion

        #region constructor

        public SHA384() => _value = Default;
        public SHA384(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidSha384Exception($"The value '{value}' is not {Length} characters long!"),
                    Validation.IllegalCharacter => new InvalidSha384Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidSha384Exception(),
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

        public static SHA384 From(string hash) => new(hash);
        
        public static Validation TryFrom(string hash, out SHA384 output)
        {
            try
            {
                var result = ValidateFormat(ref hash);
                if (result == Validation.Ok)
                {
                    output = new SHA384(ref hash);
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

        public static SHA384 Create(string value) => new(CreateHash(ref value));
        
        public static bool TryCreate(string value, out SHA384? output)
        {
            try
            {
                if (value is not null)
                {
                    var hash = CreateHash(ref value);
                    if (ValidateFormat(ref hash) == Validation.Ok)
                    {
                        output = new SHA384(ref hash);
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
            var bytes = System.Security.Cryptography.SHA384.HashData(Encoding.Default.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        private static Validation ValidateFormat(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 96 characters long
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

    public class InvalidSha384Exception : Exception
    {
        public InvalidSha384Exception()
        {
        }

        public InvalidSha384Exception(string message) : base(message)
        {
        }
    }
}
