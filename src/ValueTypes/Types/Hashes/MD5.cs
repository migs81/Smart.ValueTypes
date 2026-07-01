using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migs.ValueTypes.Types.Hashes
{
    /// <summary>
    /// Value type for Message-Digest Algorithm 5 (MD5).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidMd5Exception"></exception>
    public readonly record struct MD5 : IValueType<string, MD5>
    {
        #region fields

        private readonly string _value;
        private const string Default = "00000000000000000000000000000000";
        
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

        public static MD5 Empty => new();

        #endregion

        #region constructor

        public MD5() => _value = Default;
        public MD5(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.WrongLength => new InvalidMd5Exception($"The value '{value}' is not {Default.Length} characters long!"),
                    Validation.IllegalCharacter => new InvalidMd5Exception($"The value '{value}' contains illegal characters!"),
                    _ => new InvalidMd5Exception(),
                };
            }

            _value = value;
        }
        private MD5(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(MD5 left, string right) => left.Equals(right);
        public static bool operator !=(MD5 left, string right) => !left.Equals(right);

        public static implicit operator string(MD5 hash) => hash._value;
        public static implicit operator MD5(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static MD5 From(string hash) => new(hash);
        public static Validation TryFrom(string hash, out MD5 output)
        {
            try
            {
                var result = ValidateFormat(ref hash);
                if (result == Validation.Ok)
                {
                    output = new MD5(ref hash);
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

        public static MD5 Create(string value) => new(CreateHash(ref value));
        public static bool TryCreate(string value, out MD5? output)
        {
            try
            {
                if (value is not null)
                {
                    var md5 = CreateHash(ref value);
                    if (ValidateFormat(ref md5) == Validation.Ok)
                    {
                        output = new MD5(ref md5);
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
            var bytes = System.Security.Cryptography.MD5.HashData(Encoding.Default.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static Validation ValidateFormat(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be 32 characters long
            if (value.Length != 32)
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

    public class InvalidMd5Exception : Exception
    {
        public InvalidMd5Exception()
        {
        }

        public InvalidMd5Exception(string message) : base(message)
        {
        }
    }
}
