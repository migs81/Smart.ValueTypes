using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Security.Hashes;

namespace Smart.ValueTypes.Benchmarks.Security.Hashes
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class SHA384Benchmarks
    {
        [Benchmark]
        public void SHA384_Constructor_Benchmark()
        {
            _ = new SHA384("123456");
        }

        [Benchmark]
        public void SHA384_Create_Benchmark()
        {
            _ = SHA384.Create("123456");
        }
    }
}
