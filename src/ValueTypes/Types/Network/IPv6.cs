using System;
using System.Net;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Network
{
    /// <summary>
    /// Value type for IPv6 addresses.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIPv6Exception"></exception>
    public readonly record struct IPv6 : IValueType<string, IPv6>
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
            MultipleColons,
            SegmentTooLong,
            SegmentNotHex,
            EndsWithColon,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IPv6"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidIPv6Exception"></exception>
        public IPv6(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be empty!", nameof(value)),
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
        
        // required for internal initialization
        private IPv6(ref string value) => _value = value.ToLower();

        #endregion

        #region operator

        public static bool operator ==(IPv6 left, string right) => left.Equals(right);
        public static bool operator !=(IPv6 left, string right) => !left.Equals(right);

        public static implicit operator string(IPv6 ip) => ip._value ?? "";
        public static implicit operator IPv6(string value) => new(value);
        public static implicit operator IPv6(IPAddress address) => new(address.MapToIPv6().ToString());

        #endregion

        #region public methods

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

        private static bool IsHex(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
                if (c is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                    return false;

            return true;
        }

        private static Validation ValidateFormat(ref string value)
        {
            // ---------- general ----------
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;

            // ---------- length ----------
            if (value.Length < 2)
                return Validation.TooShort;

            if (value.Length > 39)
                return Validation.TooLong;

            var span = value.AsSpan();

            // ---------- segments ----------
            var result = CheckSegments(ref span);
            if (result != Validation.Ok)
                return result;

            return Validation.Ok;
        }

        private static Validation CheckSegments(ref ReadOnlySpan<char> span)
        {
            var lastPos = 0;
            byte colons = 0;
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i] != ':') continue;
                
                // count colons to avoid ":::" => faster than Contains(":::")
                if (colons == 0)
                {
                    colons++;
                }
                else if (lastPos == i)
                {
                    if (colons == 2)
                        return Validation.MultipleColons;

                    colons++;
                }
                else
                {
                    colons = 0;
                }

                // validate segment
                var segment = span[lastPos..i];
                var result = ValidateSegment(ref segment);
                if (result != Validation.Ok)
                    return result;

                lastPos = i + 1;
            }

            if (lastPos == span.Length)
                return Validation.EndsWithColon;

            if (lastPos < span.Length)
            {
                // validate segment
                var segment = span[lastPos..];
                return ValidateSegment(ref segment);
            }

            return Validation.Ok;
        }

        private static Validation ValidateSegment(ref ReadOnlySpan<char> segment)
        {
            if (segment.Length > 4)
                return Validation.SegmentTooLong;

            if (segment.Length > 0 && !IsHex(ref segment))
                return Validation.SegmentNotHex;

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
