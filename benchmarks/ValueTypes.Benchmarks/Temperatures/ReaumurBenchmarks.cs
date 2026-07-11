using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Temperatures;

namespace Smart.ValueTypes.Benchmarks.Temperatures
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
