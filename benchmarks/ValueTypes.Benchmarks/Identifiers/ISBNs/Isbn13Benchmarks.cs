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
    public class Isbn13Benchmarks
    {
        [Benchmark]
        public void ISBN13_Constructor_Benchmark()
        {
            _ = new ISBN13("978-0-201-61622-4");
        }

        [Benchmark]
        public void ISBN13_From_Benchmark()
        {
            _ = ISBN13.From("978-0-201-61622-4");
        }
        
        [Benchmark]
        public void ISBN13_TryFrom_Benchmark()
        {
            _ = ISBN13.TryFrom("978-0-201-61622-4", out _);
        }
    }
}
