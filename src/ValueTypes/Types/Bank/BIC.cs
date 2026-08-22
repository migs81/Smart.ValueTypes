using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Bank
{
    /// <summary>
    /// Bank Identifier Code (ISO 9362)
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidBicException"></exception>
    public readonly record struct BIC : IValueType<string, BIC>
    {
        #region fields

        private readonly string? _value;

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

        public bool IsDefault => _value is null;

        #endregion

        #region construct
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BIC"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidBicException"></exception>
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
        
        // required for internal initialization
        private BIC(ref string value) => _value = value.ToUpper();

        #endregion

        #region operator

        public static bool operator ==(BIC left, string right) => left.Equals(right);
        public static bool operator !=(BIC left, string right) => !left.Equals(right);

        public static implicit operator string(BIC bic) => bic._value ?? "";
        public static implicit operator BIC(string value) => new(value);

        #endregion

        #region public methods

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

                output = default;
                return result;
            }
            catch (Exception)
            {
                output = default;
                return Validation.UnknownError;
            }
        }
        
        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            // ---------- general ----------
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;

            // ---------- length -----------
            if (value.Length != 8 && value.Length != 11)
                return Validation.WrongLength;

            var span = value.AsSpan();

            // --------- bank code ---------
            if (!ContainsValidBankCode(ref span))
                return Validation.InvalidBankCode;

            // -------- country code -------
            if (!ContainsValidCountryCode(ref span))
                return Validation.InvalidCountryCode;

            // --------- city code ---------
            if (!ContainsValidCityCode(ref span))
                return Validation.InvalidCityCode;

            // ---- optional branch code ---
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
            if (!IsLetter(span[6]) && (span[6] is < '2' or > '9'))
                return false;

            if ((!IsLetter(span[7]) && !IsDigit(span[7])) || span[7] is 'O' or 'o')
                return false;

            return true;
        }

        private static bool ContainsValidBranchCode(ref ReadOnlySpan<char> span)
        {
            // a branch code is only present if there are 11 characters
            if (span.Length != 11) return true;
            
            // a branch code can not start with 'X'
            if (span[8] is not ('X' or 'x')) return true;
            
            // unless it is "XXX"
            if (span[9] is 'X' or 'x' && span[10] is 'X' or 'x') return true;
            
            return false;
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
