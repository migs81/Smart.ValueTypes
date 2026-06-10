using System;
using System.Net;

namespace Migs.ValueTypes.UnitTests.TestData
{
    internal class IPv6Generator
    {
        public static string[] CreateAddresses(uint amount)
        {
            var values = new string[amount];
            for (int i = 0; i < values.Length; i++)
                values[i] = CreateAddress();

            return values;
        }

        public static string CreateAddress()
        {
            byte[] bytes = new byte[16];
            new Random().NextBytes(bytes);
            IPAddress ipv6Address = new(bytes);
            return ipv6Address.ToString();
        }
    }
}
