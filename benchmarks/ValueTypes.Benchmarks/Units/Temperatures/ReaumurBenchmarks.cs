using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Units.Temperatures;

namespace Smart.ValueTypes.Benchmarks.Units.Temperatures
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class ReaumurBenchmarks
    {
        [Benchmark]
        public void Reaumur_Constructor_Integer_Benchmark()
        {
            _ = new Reaumur(10d);
        }
    }
}
