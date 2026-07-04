using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Migs.ValueTypes.Types.IPs
{
    /// <summary>
    /// Value type for IP addresses.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, IP&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="FormatException"></exception>
    public readonly record struct IP : IValueType<string, IP>
    {
        #region fields

        private readonly string _value;
        private const string Default = "0.0.0.0";
        
        public enum IPType
        {
            IPv4 = 0,
            IPv6
        }

        public readonly IPType Type;

        #endregion

        #region properties

        public static IP Empty => new();

        #endregion

        #region constructor

        public IP()
        {
            _value = Default;
            Type = IPType.IPv4;
        }
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
                throw new FormatException($"The value '{value}' is not a valid IP address!");
            }
        }
        public IP(string value, IPType type)
        {
            if (type == IPType.IPv4 && IPv4.ValidateFormat(value) != IPv4.Validation.Ok
                || type == IPType.IPv6 && IPv6.ValidateFormat(value) == IPv6.Validation.Ok)
            {
                throw new FormatException($"The value '{value}' is not a valid IP address!");
            }

            _value = value;
            Type = type;
        }
        private IP(ref string value, IPType type)
        {
            _value = value;
            Type = type;
        }

        #endregion

        #region operator

        public static bool operator ==(IP left, string right) => left.Equals(right);
        public static bool operator !=(IP left, string right) => !left.Equals(right);

        public static implicit operator string(IP ip) => ip._value;
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

                output = Empty;
                return false;
            }
            catch (Exception)
            {
                output = Empty;
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

                output = Empty;
                return false;
            }
            catch (Exception)
            {
                output = Empty;
                return false;
            }
        }

        public IPAddress GetIpAddress() => IPAddress.Parse(_value);

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(_value, value);

        #endregion
    }
}
