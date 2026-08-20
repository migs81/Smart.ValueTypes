using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Address.Austria;

namespace Smart.ValueTypes.Benchmarks.Address.Austria
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class AustrianPostalCodeBenchmarks
    {
        [Benchmark]
        public void AustrianPostalCode_Constructor_Benchmark()
        {
            _ = new AustrianPostalCode("1120");
        }

        [Benchmark]
        public void AustrianPostalCode_From_Benchmark()
        {
            _ = AustrianPostalCode.From("1120");
        }

        [Benchmark]
        public void AustrianPostalCode_TryFrom_Benchmark()
        {
            _ = AustrianPostalCode.TryFrom("1120", out _);
        }
    }
}
