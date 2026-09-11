using Smart.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Smart.ValueTypes.Types.NationalInsuranceNumbers
{
    /// <summary>
    /// Value type for the austrian national insurance number (Sozialversicherungsnummer).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidSvnrException"></exception>
    public readonly record struct SVNR : IValueType<string, SVNR>
    {
        #region fields

        private readonly string? _value;
        
        public const int Length = 10;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WrongLength,
            ContainsIllegalCharacter,
            StartsWithZero,
            InvalidDayPart,
            InvalidMonthPart,
            WrongChecksum,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SVNR"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidSvnrException"></exception>
        public SVNR(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
                    Validation.WrongLength => new InvalidSvnrException($"The value '{value}' must be {Length} characters long!"),
                    Validation.ContainsIllegalCharacter => new InvalidSvnrException($"The value '{value}' contains illegal characters!"),
                    Validation.StartsWithZero => new InvalidSvnrException($"The first digit can not be zero!"),
                    Validation.InvalidDayPart => new InvalidSvnrException($"The value '{value}' contains an invalid number for the day part!"),
                    Validation.InvalidMonthPart => new InvalidSvnrException($"The value '{value}' contains an invalid number for the month part!"),
                    Validation.WrongChecksum => new InvalidSvnrException($"The checksum of the value '{value}' is wrong!"),
                    _ => new InvalidSvnrException(),
                };
            }

            _value = value.ToLower();
        }
        
        // required for internal initialization
        private SVNR(ref string value) => _value = value.ToLower();

        #endregion

        #region operator

        public static bool operator ==(SVNR left, string right) => left.Equals(right);
        public static bool operator !=(SVNR left, string right) => !left.Equals(right);

        public static implicit operator string(SVNR svnr) => svnr._value ?? "";
        public static implicit operator SVNR(string value) => new(value);

        #endregion

        #region public methods

        public static SVNR From(string value) => new(value);
        
        public static Validation TryFrom(string value, out SVNR output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new SVNR(ref value);
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
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;

            if (value.Length != Length)
                return Validation.WrongLength;

            var span = value.AsSpan();

            // check for illegal characters
            foreach (var c in span)
            {
                if (c is < '0' or > '9')
                    return Validation.ContainsIllegalCharacter;
            }

            if (span[0] == '0')
                return Validation.StartsWithZero;

            // day part
            if (span[4] == '0' && span[5] == '0')
                return Validation.InvalidDayPart;
            if (span[4] == '3' && span[5] > '1')
                return Validation.InvalidDayPart;

            // month part (can not be zero, but greater than 12!)
            if (span[6] == '0' && span[7] == '0')
                return Validation.InvalidMonthPart;

            // checksum
            var sum = (span[0] - 48) * 3;
            sum += (span[1] - 48) * 7;
            sum += (span[2] - 48) * 9;
            sum += (span[4] - 48) * 5;
            sum += (span[5] - 48) * 8;
            sum += (span[6] - 48) * 4;
            sum += (span[7] - 48) * 2;
            sum += span[8] - 48;
            sum += (span[9] - 48) * 6;

            if (sum % 11 != span[3] - 48)
                return Validation.WrongChecksum;

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidSvnrException : Exception
    {
        public InvalidSvnrException()
        {
        }

        public InvalidSvnrException(string message) : base(message)
        {
        }
    }
}
