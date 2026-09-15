using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Identifiers;

namespace Smart.ValueTypes.Benchmarks.Identifiers
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    public class WknBenchmarks
    {
        [Benchmark]
        public void WKN_Constructor_Benchmark()
        {
            _ = new WKN("581005");
        }

        [Benchmark]
        public void WKN_From_Benchmark()
        {
            _ = WKN.From("581005");
        }
        
        [Benchmark]
        public void WKN_TryFrom_Benchmark()
        {
            _ = WKN.TryFrom("581005", out _);
        }
    }
}
