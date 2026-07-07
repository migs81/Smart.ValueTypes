using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types;
using System.Globalization;
using Smart.ValueTypes.Unfinished.Currency;

namespace Smart.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class MoneyBenchmarks
    {
        private readonly RegionInfo region = new("de-DE");

        [Benchmark]
        public void Money_Constructor_Benchmark()
        {
            _ = new Money(102.12M, new Currency(region));
        }
    }
}
