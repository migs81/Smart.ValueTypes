using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Text;

namespace Smart.ValueTypes.Benchmarks.Text
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class NonEmptyStringBenchmarks
    {
        [Benchmark]
        public void NonEmptyString_Constructor_Benchmark()
        {
            _ = new NonEmptyString("12345");
        }

        [Benchmark]
        public void NonEmptyString_From_Benchmark()
        {
            _ = NonEmptyString.From("12345");
        }

        [Benchmark]
        public void NonEmptyString_TryFrom_Benchmark()
        {
            _ = NonEmptyString.TryFrom("12345", out _);
        }
    }
}
