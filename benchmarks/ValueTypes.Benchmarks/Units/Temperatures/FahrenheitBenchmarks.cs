using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Units.Temperatures;

namespace Smart.ValueTypes.Benchmarks.Units.Temperatures
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
