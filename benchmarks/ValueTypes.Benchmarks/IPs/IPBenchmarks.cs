using BenchmarkDotNet.Attributes;
using Migs.ValueTypes.Types.IPs;

namespace Migs.ValueTypes.Benchmarks.IPs
{
    internal class IPBenchmarks
    {
        [Benchmark]
        public void IP_v4_Constructor_Benchmark()
        {
            _ = new IP("127.0.0.1", IP.IPType.IPv4);
        }

        [Benchmark]
        public void IP_v6_Constructor_Benchmark()
        {
            _ = new IP("1050:0000:0000:0000:0005:0600:300c:326b", IP.IPType.IPv6);
        }
    }
}
