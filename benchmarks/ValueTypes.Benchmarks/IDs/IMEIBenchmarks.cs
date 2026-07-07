using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.ID;

namespace Smart.ValueTypes.Benchmarks.IDs
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    public class IMEIBenchmarks
    {
        [Benchmark]
        public void IMEI_Constructor_Benchmark()
        {
            _ = new IMEI("352773074248705");
        }

        [Benchmark]
        public void IMEI_From_Benchmark()
        {
            _ = IMEI.From("352773074248705");
        }
        
        [Benchmark]
        public void IMEI_TryFrom_Benchmark()
        {
            _ = IMEI.TryFrom("352773074248705", out _);
        }
    }
}
