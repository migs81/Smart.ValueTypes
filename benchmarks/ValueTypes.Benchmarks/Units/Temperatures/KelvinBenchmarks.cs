using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Units.Temperatures;

namespace Smart.ValueTypes.Benchmarks.Units.Temperatures
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class KelvinBenchmarks
    {
        [Benchmark]
        public void Kelvin_Constructor_Benchmark()
        {
            _ = new Kelvin(-5D);
        }
    }
}
