using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;
using Migs.ValueTypes.Unfinished;

namespace Migs.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class UrlBenchmarks
    {
        [Benchmark]
        public void Url_Constructor_Benchmark()
        {
            _ = new Url("amazon.com");
        }
    }
}
