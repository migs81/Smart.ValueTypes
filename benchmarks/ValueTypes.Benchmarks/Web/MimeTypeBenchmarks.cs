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
    public class MimeTypeBenchmarks
    {
        [Benchmark]
        public void MimeType_Constructor_Benchmark()
        {
            _ = new MimeType("application/json");
        }
    }
}
