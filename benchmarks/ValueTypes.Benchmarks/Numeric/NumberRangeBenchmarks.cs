using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Numeric;

namespace Smart.ValueTypes.Benchmarks.Numeric
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class NumberRangeBenchmarks
    {
        [Benchmark]
        public void NumberRange_Constructor_Benchmark()
        {
            _ = new NumberRange<int>(0, 10);
        }
        
        [Benchmark]
        public void NumberRange_From_Benchmark()
        {
            _ = NumberRange<int>.From(0, 10);
        }

        [Benchmark]
        public void NumberRange_TryFrom_Benchmark()
        {
            _ = NumberRange<int>.TryFrom(0, 10, out _);
        }
    }
}
