using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Unfinished.Location;

namespace Smart.ValueTypes.Benchmarks.Address.Austria
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class AustrianLocalityCodeBenchmarks
    {
        [Benchmark]
        public void AustrianLocalityCode_Constructor_Benchmark()
        {
            _ = new AustrianLocalityCode("12345");
        }

        [Benchmark]
        public void AustrianLocalityCode_From_Benchmark()
        {
            _ = AustrianLocalityCode.From("12345");
        }

        [Benchmark]
        public void AustrianLocalityCode_TryFrom_Benchmark()
        {
            _ = AustrianLocalityCode.TryFrom("12345", out _);
        }
    }
}
