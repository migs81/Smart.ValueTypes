using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Numeric;

namespace Smart.ValueTypes.Benchmarks.Numeric
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class BoundedNumberBenchmarks
    {
        [Benchmark]
        public void BoundedNumber_Constructor_Benchmark()
        {
            _ = new BoundedNumber<int>(1, 1, 1);
        }
        
        [Benchmark]
        public void BoundedNumber_From_Benchmark()
        {
            _ = BoundedNumber<int>.From(1, 1, 1);
        }

        [Benchmark]
        public void BoundedNumber_TryFrom_Benchmark()
        {
            _ = BoundedNumber<int>.TryFrom(1, 1, 1, out _);
        }
    }
}
