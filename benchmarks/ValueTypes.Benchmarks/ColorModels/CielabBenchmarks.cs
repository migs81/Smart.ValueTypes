using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.ColorModels;

namespace Smart.ValueTypes.Benchmarks.ColorModels
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class CielabBenchmarks
    {
        [Benchmark]
        public void CIELAB_Constructor_Benchmark()
        {
            _ = new CIELAB(1, 1, 1);
        }
        
        [Benchmark]
        public void CIELAB_From_Benchmark()
        {
            _ = CIELAB.From(1, 1, 1);
        }

        [Benchmark]
        public void CIELAB_TryFrom_Benchmark()
        {
            _ = CIELAB.TryFrom(1, 1, 1, out _);
        }
    }
}
