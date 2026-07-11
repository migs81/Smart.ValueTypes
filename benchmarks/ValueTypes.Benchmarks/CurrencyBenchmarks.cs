using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Unfinished.Currency;

namespace Smart.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class CurrencyBenchmarks
    {
        private static readonly RegionInfo Region = new("de-DE");

        [Benchmark]
        public void Currency_Constructor_Benchmark()
        {
            _ = new Currency(Region);
        }
    }
}
