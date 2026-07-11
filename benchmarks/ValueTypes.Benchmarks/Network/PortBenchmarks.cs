using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Network;

namespace Smart.ValueTypes.Benchmarks.Network
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class PortBenchmarks
    {
        [Benchmark]
        public void Port_Constructor_Benchmark()
        {
            _ = new Port(0);
        }
        
        [Benchmark]
        public void Port_From_Benchmark()
        {
            _ = Port.From(0);
        }

        [Benchmark]
        public void Port_TryFrom_Benchmark()
        {
            _ = Port.TryFrom(0, out _);
        }
    }
}
