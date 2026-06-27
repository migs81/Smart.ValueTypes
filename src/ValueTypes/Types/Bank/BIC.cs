using System;
using System.Collections.Generic;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Types.Bank
{
    /// <summary>
    /// Bank Identifier Code (ISO 9362)
    /// </summary>
    public readonly record struct BIC : IValueType<string, BIC>
    {
        #region fields

        private readonly string _value;
        private const string Default = "AAAABBCC";

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WrongLength,
            InvalidBankCode,
            InvalidCountryCode,
            InvalidCityCode,
            InvalidBranchCode,
            UnknownError
        }
        
        #endregion

        #region properties

        public static BIC Empty => new();

        #endregion

        #region construct

        public BIC() => _value = Default;
        public BIC(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.WrongLength => new InvalidBicException($"The BIC '{value}' must have a length of 8 or 11 characters!"),
                    Validation.InvalidBankCode => new InvalidBicException($"The BIC '{value}' has no valid bank code part!"),
                    Validation.InvalidCountryCode => new InvalidBicException($"The BIC '{value}' has no valid country code part!"),
                    Validation.InvalidCityCode => new InvalidBicException($"The BIC '{value}' has no valid city code part!"),
                    Validation.InvalidBranchCode => new InvalidBicException($"The BIC '{value}' has no valid branch code part!"),
                    _ => new InvalidBicException(),
                };
            }

            _value = value.ToUpper();
        }
        private BIC(ref string value) => _value = value.ToUpper();

        #endregion

        #region operator

        public static bool operator ==(BIC left, string right) => left.Equals(right);
        public static bool operator !=(BIC left, string right) => !left.Equals(right);

        public static implicit operator string(BIC bic) => bic._value;
        public static implicit operator BIC(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(_value, value);

        public static BIC From(string value) => new(value);
        public static Validation TryFrom(string value, out BIC output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new BIC(ref value);
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

            if (value.Length == 0)
                return Validation.Empty;

            if (value.Length != 8 && value.Length != 11)
                return Validation.WrongLength;

            var span = value.AsSpan();

            // bank code
            if (!ContainsValidBankCode(ref span))
                return Validation.InvalidBankCode;

            // country code
            if (!ContainsValidCountryCode(ref span))
                return Validation.InvalidCountryCode;

            // city code
            if (!ContainsValidCityCode(ref span))
                return Validation.InvalidCityCode;

            // optional branch code
            if (!ContainsValidBranchCode(ref span))
                return Validation.InvalidBranchCode;

            return Validation.Ok;
        }

        private static bool ContainsValidBankCode(ref ReadOnlySpan<char> span)
        {
            for (var i = 0; i < 4; i++)
            {
                if (!IsLetter(span[i]) && !IsDigit(span[i]))
                    return false;
            }

            return true;
        }

        private static bool ContainsValidCountryCode(ref ReadOnlySpan<char> span)
        {
            for (var i = 4; i < 6; i++)
            {
                if (!IsLetter(span[i]))
                    return false;
            }

            return true;
        }

        private static bool ContainsValidCityCode(ref ReadOnlySpan<char> span)
        {
            if (!IsLetter(span[6]) && (span[6] < '2' || span[6] > '9'))
                return false;

            if ((!IsLetter(span[7]) && !IsDigit(span[7])) || span[7] == 'O' || span[7] == 'o')
                return false;

            return true;
        }

        private static bool ContainsValidBranchCode(ref ReadOnlySpan<char> span)
        {
            if (span.Length == 11)
            {
                if (span[8] == 'X' || span[8] == 'x')
                {
                    if ((span[9] != 'X' && span[9] != 'x') || (span[10] != 'X' && span[10] != 'x'))
                        return false;
                }
            }

            return true;
        }

        private static bool IsLetter(char c) => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
        
        private static bool IsDigit(char c) => c is >= '0' and <= '9';

        #endregion
    }

    public class InvalidBicException : Exception
    {
        public InvalidBicException()
        {
        }

        public InvalidBicException(string message) : base(message)
        {
        }
    }
}
