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
    public class NanoIdBenchmarks
    {
        [Benchmark]
        public void NanoId_Constructor_Benchmark()
        {
            _ = new NanoId("V1StGXR8_Z5jdHi6B-myT");
        }

        [Benchmark]
        public void NanoId_From_Benchmark()
        {
            _ = NanoId.From("V1StGXR8_Z5jdHi6B-myT");
        }
        
        [Benchmark]
        public void NanoId_TryFrom_Benchmark()
        {
            _ = NanoId.TryFrom("V1StGXR8_Z5jdHi6B-myT", out _);
        }
    }
}
