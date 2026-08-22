using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Bank;

namespace Smart.ValueTypes.Benchmarks.Bank
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class BicBenchmarks
    {
        [Benchmark]
        public void BIC_Constructor_Benchmark()
        {
            _ = new BIC("HBUKGB4B");
        }

        [Benchmark]
        public void BIC_From_Benchmark()
        {
            _ = BIC.From("HBUKGB4B");
        }

        [Benchmark]
        public void BIC_TryFrom_Benchmark()
        {
            _ = BIC.TryFrom("HBUKGB4B", out _);
        }
    }
}
