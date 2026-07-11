using System;
using System.Net;

namespace Smart.ValueTypes.UnitTests.TestData
{
    internal class IPv6Generator
    {
        public static string[] CreateAddresses(uint amount)
        {
            var values = new string[amount];
            for (var i = 0; i < values.Length; i++)
                values[i] = CreateAddress();

            return values;
        }

        public static string CreateAddress()
        {
            var bytes = new byte[16];
            new Random().NextBytes(bytes);
            IPAddress ipv6Address = new(bytes);
            return ipv6Address.ToString();
        }
    }
}
