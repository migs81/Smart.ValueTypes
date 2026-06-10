using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types.IPs;

namespace Migs.ValueTypes.Benchmarks.IPs
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class IPv6Benchmarks
    {
        [Benchmark]
        public void IPv6_Constructor_Benchmark()
        {
            _ = new IPv6("1050:0000:0000:0000:0005:0600:300c:326b");
        }
    }
}
