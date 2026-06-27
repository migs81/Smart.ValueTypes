using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Migs.ValueTypes.Types.IPs
{
    /// <summary>
    /// Value type for IPv6 addresses.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, IPv6&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIPv6Exception"></exception>
    public readonly record struct IPv6 : IValueType<string, IPv6>
    {
        #region fields

        private readonly string _value;
        private const string _defaultValue = "0:0:0:0:0:0:0:0";

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            TooShort,
            TooLong,
            MultipleColons,
            SegmentTooLong,
            SegmentNotHex,
            EndsWithColon,
            UnknownError
        }

        #endregion

        #region properties

        public static IPv6 Default => new();

        #endregion

        #region constructor

        public IPv6() => _value = _defaultValue;
        public IPv6(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.TooShort => new InvalidIPv6Exception($"The value '{value}' is too short!"),
                    Validation.TooLong => new InvalidIPv6Exception($"The value '{value}' is too long!"),
                    Validation.MultipleColons => new InvalidIPv6Exception($"The value '{value}' contains a sequence of multiple colons!"),
                    Validation.SegmentTooLong => new InvalidIPv6Exception($"A segment of the value '{value}' is too long!"),
                    Validation.SegmentNotHex => new InvalidIPv6Exception($"A segment of the value '{value}' is not hexadecimal!"),
                    Validation.EndsWithColon => new InvalidIPv6Exception($"The value '{value}' ends with a colon!"),
                    _ => new InvalidIPv6Exception(),
                };
            }

            _value = value.ToLower();
        }
        private IPv6(ref string value) => _value = value.ToLower();

        #endregion

        #region operator

        public static bool operator ==(IPv6 left, string right) => left.Equals(right);
        public static bool operator !=(IPv6 left, string right) => !left.Equals(right);

        public static implicit operator string(IPv6 ip) => ip._value;
        public static implicit operator IPv6(string value) => new(value);
        public static implicit operator IPv6(IPAddress address) => new(address.MapToIPv6().ToString());

        #endregion

        #region public methods

        public IPAddress GetIpAddress() => IPAddress.Parse(_value);

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(_value, value);

        public static IPv6 From(string value) => new(value);
        public static Validation TryFrom(string value, out IPv6 output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new IPv6(ref value);
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

        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static bool IsHex(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
                if (c is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                    return false;

            return true;
        }

        private static Validation ValidateFormat(ref string value)
        {
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;

            if (value.Length < 2)
                return Validation.TooShort;

            if (value.Length > 39)
                return Validation.TooLong;

            ReadOnlySpan<char> span = value.AsSpan();

            int last = 0;
            byte colons = 0;
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == ':')
                {
                    // count colons to avoid ":::" => faster then Contains(":::")
                    if (colons == 0)
                    {
                        colons++;
                    }
                    else if (last == i)
                    {
                        if (colons == 2)
                            return Validation.MultipleColons;

                        colons++;
                    }
                    else
                    {
                        colons = 0;
                    }

                    var segment = span[last..i];
                    if (segment.Length > 4)
                        return Validation.SegmentTooLong;

                    if (segment.Length > 0 && !IsHex(ref segment))
                        return Validation.SegmentNotHex;

                    last = i + 1;
                }
            }

            if (last == span.Length)
            {
                return Validation.EndsWithColon;
            }
            else if (last < span.Length)
            {
                var segment = span[last..];
                if (segment.Length > 4)
                    return Validation.SegmentTooLong;

                if (segment.Length > 0 && !IsHex(ref segment))
                    return Validation.SegmentNotHex;
            }

            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidIPv6Exception : Exception
    {
        public InvalidIPv6Exception()
        {
        }

        public InvalidIPv6Exception(string message) : base(message)
        {
        }
    }
}
