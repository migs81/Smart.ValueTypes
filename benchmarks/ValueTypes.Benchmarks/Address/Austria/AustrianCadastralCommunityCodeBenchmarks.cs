using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Address.Austria;

namespace Smart.ValueTypes.Benchmarks.Address.Austria
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class AustrianCadastralCommunityCodeBenchmarks
    {
        [Benchmark]
        public void AustrianCadastralCommunityCode_Constructor_Benchmark()
        {
            _ = new AustrianCadastralCommunityCode("12345");
        }

        [Benchmark]
        public void AustrianCadastralCommunityCode_From_Benchmark()
        {
            _ = AustrianCadastralCommunityCode.From("12345");
        }

        [Benchmark]
        public void AustrianCadastralCommunityCode_TryFrom_Benchmark()
        {
            _ = AustrianCadastralCommunityCode.TryFrom("12345", out _);
        }
    }
}
