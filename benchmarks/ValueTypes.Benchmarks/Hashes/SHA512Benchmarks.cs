using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Hashes;

namespace Smart.ValueTypes.Benchmarks.Hashes
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class SHA512Benchmarks
    {
        [Benchmark]
        public void SHA512_Constructor_Benchmark()
        {
            _ = new SHA512("123456");
        }

        [Benchmark]
        public void SHA512_Create_Benchmark()
        {
            _ = SHA512.Create("123456");
        }
    }
}
