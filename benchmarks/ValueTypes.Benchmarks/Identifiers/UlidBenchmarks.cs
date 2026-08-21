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
    public class UlidBenchmarks
    {
        [Benchmark]
        public void ULID_Constructor_Benchmark()
        {
            _ = new ULID("01J8R5T4W8M3F2X9QK7HCVN6PA");
        }

        [Benchmark]
        public void ULID_From_Benchmark()
        {
            _ = ULID.From("01J8R5T4W8M3F2X9QK7HCVN6PA");
        }
        
        [Benchmark]
        public void ULID_TryFrom_Benchmark()
        {
            _ = ULID.TryFrom("01J8R5T4W8M3F2X9QK7HCVN6PA", out _);
        }
    }
}
