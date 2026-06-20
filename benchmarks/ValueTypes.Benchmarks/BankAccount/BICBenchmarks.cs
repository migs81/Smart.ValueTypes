using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;
using Migs.ValueTypes.Types.BankAccount;

namespace Migs.ValueTypes.Benchmarks.BankAccount
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class BICBenchmarks
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
