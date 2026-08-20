using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Address.Austria;

namespace Smart.ValueTypes.Benchmarks.Address.Austria
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class AustrianAddressCodeBenchmarks
    {
        [Benchmark]
        public void AustrianAddressCode_Constructor_Benchmark()
        {
            _ = new AustrianAddressCode("12345");
        }

        [Benchmark]
        public void AustrianAddressCode_From_Benchmark()
        {
            _ = AustrianAddressCode.From("12345");
        }

        [Benchmark]
        public void AustrianAddressCode_TryFrom_Benchmark()
        {
            _ = AustrianAddressCode.TryFrom("12345", out _);
        }
    }
}
