using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.ColorModels;

namespace Smart.ValueTypes.Benchmarks.ColorModels
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class YCbCrBenchmarks
    {
        [Benchmark]
        public void YCbCr_Constructor_Benchmark()
        {
            _ = new YCbCr(1, 0.5f, 0.5f);
        }
        
        [Benchmark]
        public void YCbCr_From_Benchmark()
        {
            _ = YCbCr.From(1, 0.5f, 0.5f);
        }

        [Benchmark]
        public void YCbCr_TryFrom_Benchmark()
        {
            _ = YCbCr.TryFrom(1, 0.5f, 0.5f, out _);
        }
    }
}
