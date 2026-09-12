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
    public class IsinBenchmarks
    {
        [Benchmark]
        public void ISIN_Constructor_Benchmark()
        {
            _ = new ISIN("AT0000APOST4");
        }

        [Benchmark]
        public void ISIN_From_Benchmark()
        {
            _ = ISIN.From("AT0000APOST4");
        }
        
        [Benchmark]
        public void ISIN_TryFrom_Benchmark()
        {
            _ = ISIN.TryFrom("AT0000APOST4", out _);
        }
    }
}
