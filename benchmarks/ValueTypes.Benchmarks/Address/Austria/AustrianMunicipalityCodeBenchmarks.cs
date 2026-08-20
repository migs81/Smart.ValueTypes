using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Address.Austria;

namespace Smart.ValueTypes.Benchmarks.Address.Austria
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class AustrianMunicipalityCodeBenchmarks
    {
        [Benchmark]
        public void AustrianMunicipalityCode_Constructor_Benchmark()
        {
            _ = new AustrianMunicipalityCode("12345");
        }

        [Benchmark]
        public void AustrianMunicipalityCode_From_Benchmark()
        {
            _ = AustrianMunicipalityCode.From("12345");
        }

        [Benchmark]
        public void AustrianMunicipalityCode_TryFrom_Benchmark()
        {
            _ = AustrianMunicipalityCode.TryFrom("12345", out _);
        }
    }
}
