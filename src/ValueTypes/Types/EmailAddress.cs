using Smart.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Smart.ValueTypes.Types
{
    /// <summary>
    /// Value type for email addresses.
    /// </summary>
    /// <remarks>
    /// This implementation supports standard ASCII email addresses using the dot-atom format.
    /// It does not support quoted local parts, comments, or domain literals.
    /// </remarks>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidEmailAddressException"></exception>
    public readonly record struct EmailAddress : IValueType<string, EmailAddress>
    {
        #region fields

        private readonly string? _value;
        
        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            TooShort,
            TooLong,
            LocalPartStartsWithDot,
            LocalPartEndsWithDot,
            NoAtSign,
            LocalPartTooShort,
            LocalPartTooLong,
            LocalPartContainsTwoDotsTogether,
            DomainPartTooShort,
            LocalPartContainsIllegalCharacter,
            DomainPartContainsIllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;
        
        public string LocalPart => _value is not null ? _value[.._value.IndexOf('@')] : "";
        public string DomainPart => _value is not null ? _value[(_value.IndexOf('@') + 1)..] : "";
        public string Host => _value is not null ? DomainPart[..DomainPart.IndexOf('.')] : "";
        public string TopLevelDomain => _value is not null ? DomainPart[..] : "";
        public string SecondLevelDomain => _value is not null ? DomainPart[..] : "";
        public string DisplayName 
        { 
            get
            {
                if (_value is null) return "";
                if (!_value.StartsWith('[')) return "";
                
                var pos = _value.IndexOf(']');
                if (pos >1)
                    return _value[2..(pos - 1)];

                return "";
            } 
        }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidEmailAddressException"></exception>
        public EmailAddress(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.TooShort => new InvalidEmailAddressException($"The value '{value}' is too short!"),
                    Validation.TooLong => new InvalidEmailAddressException($"The value '{value}' is too long!"),
                    Validation.LocalPartStartsWithDot => new InvalidEmailAddressException($"The value '{value}' starts with a dot!"),
                    Validation.LocalPartEndsWithDot => new InvalidEmailAddressException($"The value '{value}' ends with a dot!"),
                    Validation.LocalPartContainsTwoDotsTogether => new InvalidEmailAddressException($"The value '{value}' contains two dots together!"),
                    Validation.LocalPartTooShort => new InvalidEmailAddressException($"The local part of the '{value}' is too short!"),
                    Validation.LocalPartTooLong => new InvalidEmailAddressException($"The local part of the '{value}' is too long!"),
                    Validation.LocalPartContainsIllegalCharacter => new InvalidEmailAddressException($"The local part of the '{value}' contains illegal characters!"),
                    Validation.DomainPartTooShort => new InvalidEmailAddressException($"The domain part of the '{value}' is too short!"),
                    Validation.DomainPartContainsIllegalCharacter => new InvalidEmailAddressException($"The domain part of the '{value}' contains illegal characters!"),
                    Validation.NoAtSign => new InvalidEmailAddressException($"The value '{value}' contains no @[at] sign!"),
                    _ => new InvalidEmailAddressException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private EmailAddress(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(EmailAddress left, string right) => left.Equals(right);
        public static bool operator !=(EmailAddress left, string right) => !left.Equals(right);

        public static implicit operator string(EmailAddress emailAddress) => emailAddress._value ?? "";
        public static implicit operator EmailAddress(string value) => new(value);

        public static explicit operator EmailAddress(MailAddress mail) => new(mail.Address);

        #endregion

        #region public methods
        
        public static EmailAddress From(string value) => new(value);
        
        public static Validation TryFrom(string value, out EmailAddress output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new EmailAddress(ref value);
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

        /// <summary>
        /// Determines whether the specified value is an email address.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static Validation ValidateFormat(ref string value)
        {
            if (value is null)
                return Validation.Null;

            var span = value.AsSpan();

            // ------------ Length ----------------
            if (span.Length == 0)
                return Validation.Empty;

            // Min length => 3 (a@b)
            if (span.Length < 3)
                return Validation.TooShort;

            // Max length (RFC 5321) => 254 Octets
            if (span.Length > 254)
                return Validation.TooLong;

            // ------------ @ sign  ---------------
            // check for @ sign
            var atPos = span.IndexOf('@');
            if (atPos == -1)
                return Validation.NoAtSign;
            
            // ------------ Local Part ------------
            // local part min length = 1
            if (atPos < 1)
                return Validation.LocalPartTooShort;

            // local part max length = 64
            if (atPos > 64)
                return Validation.LocalPartTooLong;

            // local part can not start with a dot
            if (span[0] == '.')
                return Validation.LocalPartStartsWithDot;
    
            // local part can not end with a dot
            if (span[atPos - 1] == '.')
                return Validation.LocalPartEndsWithDot;

            // local part can not contain two dots together
            if (span[..atPos].IndexOf("..") != -1)
                return Validation.LocalPartContainsTwoDotsTogether;
            
            // check local part characters
            foreach (var c in span[..atPos])
            {
                // RFC 5322
                if (c > 127) return Validation.LocalPartContainsIllegalCharacter;
                if (c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9') continue;
                if (c is '!' or '#' or '$' or '%' or '&' or '\'' or '*' or '+' or '-' or '/') continue;
                if (c is '=' or '?' or '^' or '_' or '`' or '{' or '|' or '}' or '~' or '.') continue;

                return Validation.LocalPartContainsIllegalCharacter;
            }
            
            // ------------ Domain Part ------------
            // domain part min length = 1
            if (span.Length - atPos < 2)
            // if (span[(atPos + 1)..].Length == 0)
                return Validation.DomainPartTooShort;

            // check domain part characters
            foreach (var c in span[(atPos + 1)..])
            {
                if (c is (< 'a' or > 'z') and (< 'A' or > 'Z') and (< '0' or > '9')
                    && c != '.'
                    && c != '-')
                {
                    return Validation.DomainPartContainsIllegalCharacter;
                }
            }

            return Validation.Ok;
        }
        
        #endregion
    }

    public class InvalidEmailAddressException : Exception
    {
        public InvalidEmailAddressException()
        {
        }

        public InvalidEmailAddressException(string message) : base(message)
        {
        }
    }
}
