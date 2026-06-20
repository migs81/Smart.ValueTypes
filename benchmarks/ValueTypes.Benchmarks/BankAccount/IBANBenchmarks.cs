using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;
using Migs.ValueTypes.Types.BankAccount;

namespace Migs.ValueTypes.Benchmarks.BankAccount
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class IBANBenchmarks
    {
        [Benchmark]
        public void IBAN_Constructor_Benchmark()
        {
            _ = new IBAN("AT483200000012345864");
        }

        //[Benchmark]
        //public void IBAN_From_Benchmark()
        //{
        //    _ = IBAN.From("AT483200000012345864");
        //}

        //[Benchmark]
        //public void IBAN_TryFrom_Benchmark()
        //{
        //    _ = IBAN.TryFrom("AT483200000012345864", out _);
        //}
    }
}
