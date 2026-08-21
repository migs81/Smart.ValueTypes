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
    public class ImsiBenchmarks
    {
        [Benchmark]
        public void IMSI_Constructor_Benchmark()
        {
            _ = new IMSI("123456789012345");
        }

        [Benchmark]
        public void IMSI_From_Benchmark()
        {
            _ = IMSI.From("123456789012345");
        }
        
        [Benchmark]
        public void IMSI_TryFrom_Benchmark()
        {
            _ = IMSI.TryFrom("123456789012345", out _);
        }
    }
}
