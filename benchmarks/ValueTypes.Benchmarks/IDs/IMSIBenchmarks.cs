using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types.ID;

namespace Migs.ValueTypes.Benchmarks.IDs
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    public class IMSIBenchmarks
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
