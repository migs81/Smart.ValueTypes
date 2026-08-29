using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Units.Temperatures;

namespace Smart.ValueTypes.Benchmarks.Units.Temperatures
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class CelsiusBenchmarks
    {
        private readonly Kelvin _kelvin = new(10);
        private readonly Fahrenheit _fahrenheit = new(10);
        
        [Benchmark]
        public void CelsiusConstructorIntegerBenchmark()
        {
            _ = new Celsius(-50);
        }

        [Benchmark]
        public void Celsius_Constructor_Double_Benchmark()
        {
            _ = new Celsius(-50);
        }

        [Benchmark]
        public void Celsius_Constructor_Kelvin_Benchmark()
        {
            _ = new Celsius(_kelvin);
        }

        [Benchmark]
        public void Celsius_Constructor_Fahrenheit_Benchmark()
        {
            _ = new Celsius(_fahrenheit);
        }
    }
}
