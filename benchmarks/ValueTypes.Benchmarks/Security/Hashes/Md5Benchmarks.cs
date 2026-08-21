using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Security.Hashes;

namespace Smart.ValueTypes.Benchmarks.Security.Hashes
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class Md5Benchmarks
    {
        [Benchmark]
        public void MD5_Constructor_Benchmark()
        {
            _ = new MD5("d41d8cd98f00b204e9800998ecf8427e");
        }

        [Benchmark]
        public void MD5_Create_Benchmark()
        {
            _ = MD5.Create("123456");
        }
    }
}
