using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Graphics;

namespace Smart.ValueTypes.Benchmarks.Graphics
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class OpacityBenchmarks
    {
        [Benchmark]
        public void Opacity_Constructor_Benchmark()
        {
            _ = new Opacity(0.75);
        }

        [Benchmark]
        public void Opacity_From_Benchmark()
        {
            _ = Opacity.From(0.75);
        }
    }
}
