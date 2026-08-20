using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Security.Hashes;

namespace Smart.ValueTypes.Benchmarks.Security.Hashes
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class SHA1Benchmarks
    {
        [Benchmark]
        public void SHA1_Constructor_Benchmark()
        {
            _ = new SHA1("123456");
        }

        [Benchmark]
        public void SHA1_Create_Benchmark()
        {
            _ = SHA1.Create("123456");
        }
    }
}
