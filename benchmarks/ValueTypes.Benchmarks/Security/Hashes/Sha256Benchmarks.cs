using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Security.Hashes;

namespace Smart.ValueTypes.Benchmarks.Security.Hashes
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class Sha256Benchmarks
    {
        [Benchmark]
        public void SHA256_Constructor_Benchmark()
        {
            _ = new SHA256("123456");
        }

        [Benchmark]
        public void SHA256_Create_Benchmark()
        {
            _ = SHA256.Create("123456");
        }
    }
}
