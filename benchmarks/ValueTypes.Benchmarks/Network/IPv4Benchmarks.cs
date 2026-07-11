using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Network;

namespace Smart.ValueTypes.Benchmarks.Network
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class IPv4Benchmarks
    {
        [Benchmark]
        public void IPv4_Constructor_Benchmark()
        {
            _ = new IPv4("127.0.0.1");
        }
    }
}
