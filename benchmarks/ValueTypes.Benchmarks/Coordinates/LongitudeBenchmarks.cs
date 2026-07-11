using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Coordinates;

namespace Smart.ValueTypes.Benchmarks.Coordinates
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class LongitudeBenchmarks
    {
        [Benchmark]
        public void Longitude_Constructor_Benchmark()
        {
            _ = new Longitude(0);
        }

        [Benchmark]
        public void Longitude_From_Benchmark()
        {
            _ = Longitude.From(0);
        }
    }
}
