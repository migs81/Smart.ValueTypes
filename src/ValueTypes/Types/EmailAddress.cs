using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Migs.ValueTypes.Types
{
    /// <summary>
    /// Value type for email addresses (RFC 5322/5321)
    /// 
    /// Does not support UTF8, Display name
    ///     
    /// </summary>
    /// <seealso cref="IValueType&lt;string, EmailAddress&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidEmailAddressException"></exception>
    public readonly record struct EmailAddress : IValueType<string, EmailAddress>
    {
        #region fields

        private readonly string _value;
        
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
            DomainPartTooShort,
            LocalPartContainsIllegalCharacter,
            DomainPartContainsIllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public static EmailAddress Default { get; } = new EmailAddress();
        
        public string LocalPart => _value[.._value.IndexOf('@')];
        public string DomainPart => _value[(_value.IndexOf('@') + 1)..];
        public string Host => DomainPart[..DomainPart.IndexOf('.')];
        public string TopLevelDomain => DomainPart[..];
        public string SecondLevelDomain => DomainPart[..];
        public string DisplayName 
        { 
            get
            {
                if (_value.StartsWith('['))
                {
                    int pos = _value.IndexOf(']');
                    if (pos >1)
                        return _value[2..(pos - 1)];
                }

                return "";
            } 
        }

        #endregion

        #region constructor

        public EmailAddress() => _value = "user@host";
        public EmailAddress(string value)
        {
            var result = Validate(ref value);
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
        private EmailAddress(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(EmailAddress left, string right) => left.Equals(right);
        public static bool operator !=(EmailAddress left, string right) => !left.Equals(right);

        public static implicit operator string(EmailAddress emailAddress) => emailAddress._value;
        public static implicit operator EmailAddress(string value) => new(value);

        public static explicit operator EmailAddress(MailAddress mail) => new(mail.Address);

        #endregion

        #region public methods

        public MailAddress GetMailAddress() => new(_value);

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static EmailAddress From(string value) => new(value);
        public static Validation TryFrom(string value, out EmailAddress output)
        {
            try
            {
                var result = Validate(ref value);
                if (result == Validation.Ok)
                {
                    output = new EmailAddress(ref value);
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

        public static Validation Validate(string value) => Validate(ref value);

        #endregion

        #region private methods

        /// <summary>
        /// Determines whether the specified value is an email address.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static Validation Validate(ref string value)
        {
            if (value is null)
                return Validation.Null;

            ReadOnlySpan<char> span = value.AsSpan();

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
            int atPos = span.IndexOf('@');
            if (atPos == -1)
                return Validation.NoAtSign;

            // ----------- Display Name -----------
            //int start = 0;
            //if (span.StartsWith("["))
            //{
            //    start = span.IndexOf("]") + 1;
            //    if (start > 0)
            //}

            // ------------ Local Part ------------
            // local part min length = 1
            if (span[..atPos].Length == 0)
                return Validation.LocalPartTooShort;

            // local part max length = 64
            if (span[..atPos].Length > 64)
                return Validation.LocalPartTooLong;

            // local part can not start with a dot
            if (span.StartsWith("."))
                return Validation.LocalPartStartsWithDot;

            // local part can not end with a dot
            if (span[..atPos].EndsWith("."))
                return Validation.LocalPartEndsWithDot;

            // check local part characters
            foreach (var c in span[0..atPos])
            {
                // RFC 5322
                if (c > 127)
                    return Validation.LocalPartContainsIllegalCharacter;

                if ((c < 'a' || c > 'z')
                    && (c < 'A' || c > 'Z')
                    && (c < '0' || c > '9')
                    && c != '.'
                    && c != '_'
                    && c != '-')
                {
                    return Validation.LocalPartContainsIllegalCharacter;
                }
            }

            // ------------ Domain Part ------------
            // domain part min length = 1
            if (span[(atPos + 1)..].Length == 0)
                return Validation.DomainPartTooShort;

            // check domain part characters
            foreach (char c in span[(atPos + 1)..])
            {
                if ((c < 'a' || c > 'z')
                    && (c < 'A' || c > 'Z')
                    && (c < '0' || c > '9')
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
