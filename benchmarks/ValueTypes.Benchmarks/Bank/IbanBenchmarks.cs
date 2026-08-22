using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Bank;

namespace Smart.ValueTypes.Benchmarks.Bank
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class IbanBenchmarks
    {
        [Benchmark]
        public void IBAN_Constructor_Benchmark()
        {
            _ = new IBAN("AT483200000012345864");
        }

        [Benchmark]
        public void IBAN_From_Benchmark()
        {
            _ = IBAN.From("AT483200000012345864");
        }

        [Benchmark]
        public void IBAN_TryFrom_Benchmark()
        {
            _ = IBAN.TryFrom("AT483200000012345864", out _);
        }
    }
}
