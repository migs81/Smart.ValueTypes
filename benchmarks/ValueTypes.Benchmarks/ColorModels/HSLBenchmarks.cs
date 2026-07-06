using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types.ColorModels;

namespace Migs.ValueTypes.Benchmarks.ColorModels
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class HSLBenchmarks
    {
        [Benchmark]
        public void HSL_Constructor_Benchmark()
        {
            _ = new HSL(1, 1, 1);
        }
        
        [Benchmark]
        public void HSL_From_Benchmark()
        {
            _ = HSL.From(1, 1, 1);
        }

        [Benchmark]
        public void HSL_TryFrom_Benchmark()
        {
            _ = HSL.TryFrom(1, 1, 1, out _);
        }
    }
}
