using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types.Temperatures;

namespace Migs.ValueTypes.Benchmarks.Temperatures
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class FahrenheitBenchmarks
    {
        [Benchmark]
        public void Fahrenheit_Constructor_Benchmark()
        {
            _ = new Fahrenheit(-5D);
        }
    }
}
