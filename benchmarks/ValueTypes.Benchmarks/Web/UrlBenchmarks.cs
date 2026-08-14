using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Web;

namespace Smart.ValueTypes.Benchmarks.Web
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class UrlBenchmarks
    {
        [Benchmark]
        public void Url_Constructor_Benchmark()
        {
            _ = new Url("https://example.com:80/path?query=1#fragment");
        }
    }
}
