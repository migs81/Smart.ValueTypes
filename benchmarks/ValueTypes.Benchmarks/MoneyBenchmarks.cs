using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;
using System.Globalization;

namespace Migs.ValueTypes.Benchmarks
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
