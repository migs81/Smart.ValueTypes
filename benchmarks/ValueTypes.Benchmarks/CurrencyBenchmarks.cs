using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;
using System.Globalization;

namespace Migs.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class CurrencyBenchmarks
    {
        private readonly RegionInfo region = new("de-DE");

        [Benchmark]
        public void Currency_Constructor_Benchmark()
        {
            _ = new Currency(region);
        }
    }
}
