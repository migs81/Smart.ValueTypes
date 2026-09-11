using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Identifiers.ISBNs;

namespace Smart.ValueTypes.Benchmarks.Identifiers.ISBNs
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    public class Isbn10Benchmarks
    {
        [Benchmark]
        public void ISBN10_Constructor_Benchmark()
        {
            _ = new ISBN10("0-131-10362-8");
        }

        [Benchmark]
        public void ISBN10_From_Benchmark()
        {
            _ = ISBN10.From("0-131-10362-8");
        }
        
        [Benchmark]
        public void ISBN10_TryFrom_Benchmark()
        {
            _ = ISBN10.TryFrom("0-131-10362-8", out _);
        }
    }
}
