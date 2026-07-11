using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Coordinates;

namespace Smart.ValueTypes.Benchmarks.Coordinates
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class LatitudeBenchmarks
    {
        [Benchmark]
        public void Latitude_Constructor_Benchmark()
        {
            _ = new Latitude(0);
        }

        [Benchmark]
        public void Latitude_From_Benchmark()
        {
            _ = Latitude.From(0);
        }
    }
}
