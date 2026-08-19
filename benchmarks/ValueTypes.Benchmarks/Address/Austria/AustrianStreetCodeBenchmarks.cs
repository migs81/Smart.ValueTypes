using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Unfinished.Location;

namespace Smart.ValueTypes.Benchmarks.Address.Austria
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class AustrianStreetCodeBenchmarks
    {
        [Benchmark]
        public void AustrianStreetCode_Constructor_Benchmark()
        {
            _ = new AustrianStreetCode("123456");
        }

        [Benchmark]
        public void AustrianStreetCode_From_Benchmark()
        {
            _ = AustrianStreetCode.From("123456");
        }

        [Benchmark]
        public void AustrianStreetCode_TryFrom_Benchmark()
        {
            _ = AustrianStreetCode.TryFrom("123456", out _);
        }
    }
}
