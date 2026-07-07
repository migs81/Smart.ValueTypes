using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.ColorModels;

namespace Smart.ValueTypes.Benchmarks.ColorModels
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class HSVBenchmarks
    {
        [Benchmark]
        public void HSV_Constructor_Benchmark()
        {
            _ = new HSV(1, 1, 1);
        }
        
        [Benchmark]
        public void HSV_From_Benchmark()
        {
            _ = HSV.From(1, 1, 1);
        }

        [Benchmark]
        public void HSV_TryFrom_Benchmark()
        {
            _ = HSV.TryFrom(1, 1, 1, out _);
        }
    }
}
