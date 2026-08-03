using System;
using System.Net;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Network
{
    /// <summary>
    /// Value type for IP addresses.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidIPException"></exception>
    public readonly record struct IP : IValueType<string, IP>
    {
        #region fields

        private readonly string? _value;
        
        public enum IPType
        {
            IPv4 = 0,
            IPv6
        }

        public readonly IPType Type;

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidIPException"></exception>
        public IP(string value)
        {
            if (IPv4.ValidateFormat(value) == IPv4.Validation.Ok)
            {
                _value = value;
                Type = IPType.IPv4;
            }
            else if (IPv6.ValidateFormat(value) == IPv6.Validation.Ok)
            {
                _value = value;
                Type = IPType.IPv6;
            }
            else
            {
                throw new InvalidIPException($"The value '{value}' is not a valid IP address!");
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <exception cref="InvalidIPException"></exception>
        public IP(string value, IPType type)
        {
            if (type == IPType.IPv4 && IPv4.ValidateFormat(value) != IPv4.Validation.Ok
                || type == IPType.IPv6 && IPv6.ValidateFormat(value) == IPv6.Validation.Ok)
            {
                throw new InvalidIPException($"The value '{value}' is not a valid IP address!");
            }

            _value = value;
            Type = type;
        }
        
        // required for internal initialization
        private IP(ref string value, IPType type)
        {
            _value = value;
            Type = type;
        }

        #endregion

        #region operator

        public static bool operator ==(IP left, string right) => left.Equals(right);
        public static bool operator !=(IP left, string right) => !left.Equals(right);

        public static implicit operator string(IP ip) => ip._value ?? "";
        public static implicit operator IP(string value) => new(value);
        public static implicit operator IP(IPv4 ip) => new(ip, IPType.IPv4);
        public static implicit operator IP(IPv6 ip) => new(ip, IPType.IPv4);

        public static explicit operator IPv4(IP ip) => new(ip);
        public static explicit operator IPv6(IP ip) => new(ip);
        public static explicit operator IP(IPAddress ipAddress) => new(ipAddress.ToString());

        #endregion

        #region public methods

        public static IP From(string value) => new(value);
        
        public static bool TryFrom(string value, out IP output)
        {
            try
            {
                if (IPv4.ValidateFormat(value) == IPv4.Validation.Ok)
                {
                    output = new IP(ref value, IPType.IPv4);
                    return true;
                }

                if (IPv6.ValidateFormat(value) == IPv6.Validation.Ok)
                {
                    output = new IP(ref value, IPType.IPv6);
                    return true;
                }

                output = default;
                return false;
            }
            catch (Exception)
            {
                output = default;
                return false;
            }
        }
        
        public static bool TryFrom(string value, IPType type, out IP output)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (type == IPType.IPv4 && IPv4.ValidateFormat(value) == IPv4.Validation.Ok
                        || type == IPType.IPv6 && IPv6.ValidateFormat(value) == IPv6.Validation.Ok)
                    {
                        output = new IP(ref value, type);
                        return true;
                    }
                }

                output = default;
                return false;
            }
            catch (Exception)
            {
                output = default;
                return false;
            }
        }

        #endregion
    }
    
    public class InvalidIPException : Exception
    {
        public InvalidIPException()
        {
        }

        public InvalidIPException(string message) : base(message)
        {
        }
    }
}
