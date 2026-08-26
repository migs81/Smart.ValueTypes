using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Web;

namespace Smart.ValueTypes.Benchmarks.Web
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    public class SlugBenchmarks
    {
        [Benchmark]
        public void Slug_Constructor_Benchmark()
        {
            _ = new Slug("test-slug");
        }
        
        [Benchmark]
        public void Slug_Parse_Benchmark()
        {
            _ = Slug.Parse("Crème brûlée aus Österreich");
        }
    }
}
