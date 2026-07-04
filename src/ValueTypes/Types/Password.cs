using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Types
{
    /// <summary>
    /// Value type for passwords.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, Password&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidPasswordException"></exception>
    public readonly record struct Password : IValueType<string, Password>
    {
        #region fields

        private readonly string _value;

        public enum Validation
        {
            Ok = 0,
            Null,
            TooShort,
            TooLong,
            LowercaseLettersMissing,
            UppercaseLettersMissing,
            NumbersMissing,
            SymbolsMissing,
            UnknownError,
        }

        [Flags]
        public enum Requirement
        {
            Nothing = 0,
            LowercaseLetters = 1,
            UppercaseLetters = 2,
            Numbers = 4,
            Symbols = 8,
        }

        #endregion

        #region properties

        public int MinLength { get; }
        public int MaxLength { get; }
        public Requirement Requirements { get; }
        public static Password Empty { get; } = new();

        #endregion

        #region constructor

        public Password()
        {
            _value = "";
            MinLength = 0;
            MaxLength = int.MaxValue;
            Requirements = Requirement.Nothing;
        }
        public Password(string value, int minLength = 0, int maxLength = int.MaxValue, Requirement requirements = Requirement.Nothing)
        {
            var result = ValidateFormat(ref value, ref minLength, ref maxLength, ref requirements);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new InvalidPasswordException($"The given password value was null!"),
                    Validation.TooShort => new InvalidPasswordException($"The given password '{value}' is shorter then the minimum length of {minLength}!"),
                    Validation.TooLong => new InvalidPasswordException($"The given password '{value}' is longer then the max length of {maxLength}!"),
                    Validation.LowercaseLettersMissing => new InvalidPasswordException($"The password '{value}' does not meet the '{Requirement.LowercaseLetters}' requirement!"),
                    Validation.UppercaseLettersMissing => new InvalidPasswordException($"The password '{value}' does not meet the '{Requirement.UppercaseLetters}' requirement!"),
                    Validation.NumbersMissing => new InvalidPasswordException($"The password '{value}' does not meet the '{Requirement.Numbers}' requirement!"),
                    Validation.SymbolsMissing => new InvalidPasswordException($"The password '{value}' does not meet the '{Requirement.Symbols}' requirement!"),
                    _ => new InvalidPasswordException()
                };
            }

            _value = value;
            MinLength = minLength;
            MaxLength = maxLength;
            Requirements = requirements;
        }
        private Password(ref string value, int minLength, int maxLength, Requirement requirements)
        {
            _value = value;
            MinLength = minLength;
            MaxLength = maxLength;
            Requirements = requirements;
        }

        #endregion

        #region operator

        public static bool operator ==(Password left, string right) => left.Equals(right);
        public static bool operator !=(Password left, string right) => !left.Equals(right);

        public static implicit operator string(Password password) => password._value;
        public static implicit operator Password(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(_value, value);

        public static Password From(string value, int minLength = 0, int maxLength = int.MaxValue, Requirement requirements = Requirement.Nothing) 
            => new(value, minLength, maxLength, requirements);

        public static Validation TryFrom(string value, int minLength, int maxLength, Requirement requirements, out Password password)
        {
            try
            {
                var result = ValidateFormat(ref value, ref minLength, ref maxLength, ref requirements);
                if (result == Validation.Ok)
                {
                    password = new Password(ref value, minLength, maxLength, requirements);
                    return Validation.Ok;
                }

                password = Empty;
                return result;
            }
            catch (Exception)
            {
                password = Empty;
                return Validation.UnknownError;
            }
        }

        public static Validation Validate(string value, int minLength, int maxLength, Requirement requirements) 
            => ValidateFormat(ref value, ref minLength, ref maxLength, ref requirements);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value, ref int minLength, ref int maxLength, ref Requirement requirements)
        {
            if (value is null)
                return Validation.Null;

            if (value.Length < minLength)
                return Validation.TooShort;

            if (value.Length > maxLength)
                return Validation.TooLong;

            var span = value.AsSpan();

            if (requirements.HasFlag(Requirement.LowercaseLetters) && !ContainsLowercaseLetters(ref span))
                return Validation.LowercaseLettersMissing;

            if (requirements.HasFlag(Requirement.UppercaseLetters) && !ContainsUppercaseLetters(ref span))
                return Validation.UppercaseLettersMissing;

            if (requirements.HasFlag(Requirement.Numbers) && !ContainsNumbers(ref span))
                return Validation.NumbersMissing;

            if (requirements.HasFlag(Requirement.Symbols) && !ContainsSymbols(ref span))
                return Validation.SymbolsMissing;

            return Validation.Ok;
        }

        private static bool ContainsLowercaseLetters(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (char.IsLower(c))
                    return true;
            }

            return false;
        }

        private static bool ContainsUppercaseLetters(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (char.IsUpper(c))
                    return true;
            }

            return false;
        }

        private static bool ContainsNumbers(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (char.IsDigit(c))
                    return true;
            }

            return false;
        }

        private static bool ContainsSymbols(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (char.IsSymbol(c))
                    return true;
            }

            return false;
        }

        #endregion
    }

    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}
