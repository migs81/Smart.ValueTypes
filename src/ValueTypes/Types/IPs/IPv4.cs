using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Migs.ValueTypes.Types.IPs
{
    /// <summary>
    /// Value type for IPv4 addresses.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, IPv4&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIPv4Exception"></exception>
    public readonly record struct IPv4 : IValueType<string, IPv4>
    {
        #region fields

        private readonly string _value;
        private const string Default = "0.0.0.0";

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            TooShort,
            TooLong,
            ContainsIllegalCharacter,
            InvalidSegmentNumber,
            InvalidSegmentCount,
            StartsWithDot,
            EndsWithDot,
            UnknownError
        }

        #endregion

        #region properties

        public static IPv4 Empty => new();

        #endregion

        #region constructor

        public IPv4() => _value = Default;
        public IPv4(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.TooShort => new InvalidIPv4Exception($"The value '{value}' is too short!"),
                    Validation.TooLong => new InvalidIPv4Exception($"The value '{value}' is too long!"),
                    Validation.EndsWithDot => new InvalidIPv4Exception($"The value '{value}' ends with a dot!"),
                    Validation.InvalidSegmentNumber => new InvalidIPv4Exception($"A segment of the value '{value}' is not a valid number!"),
                    Validation.InvalidSegmentCount => new InvalidIPv4Exception($"The value '{value}' has a wrong segment count!"),
                    _ => new InvalidIPv4Exception(),
                };
            }

            _value = value;
        }
        private IPv4(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(IPv4 left, string right) => left.Equals(right);
        public static bool operator !=(IPv4 left, string right) => !left.Equals(right);

        public static implicit operator string(IPv4 ip) => ip._value;
        public static implicit operator IPv4(string value) => new(value);
        public static implicit operator IPv4(IPAddress ipAdress) => new(ipAdress.MapToIPv4().ToString());

        #endregion

        #region public methods

        public IPAddress GetIpAddress() => IPAddress.Parse(_value);

        public static IPv4 From(string value) => new(value);
        
        public static Validation TryFrom(string value, out IPv4 output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new IPv4(ref value);
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

            if (value.Length < 7)
                return Validation.TooShort;

            if (value.Length > 15)
                return Validation.TooLong;

            if (value.StartsWith('.'))
                return Validation.StartsWithDot;

            if (value.EndsWith('.'))
                return Validation.EndsWithDot;

            var span = value.AsSpan();

            // check for illegal characters and parse segment numbers
            var last = 0;
            var parts = 0;
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i] >= '0' && span[i] <= '9') continue;
                if (span[i] == '.')
                {
                    // check segment number
                    if (!byte.TryParse(span[last..i], out _))
                        return Validation.InvalidSegmentNumber;

                    last = i + 1;
                    parts++;
                }
                else
                {
                    // illegal character
                    return Validation.ContainsIllegalCharacter;
                }
            }

            // check last segment
            if (!byte.TryParse(span[last..], out _))
                return Validation.InvalidSegmentNumber;
            parts++;

            // invalid segment count?
            if (parts != 4)
                return Validation.InvalidSegmentCount;

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidIPv4Exception : Exception
    {
        public InvalidIPv4Exception()
        {
        }

        public InvalidIPv4Exception(string message) : base(message)
        {
        }
    }
}
