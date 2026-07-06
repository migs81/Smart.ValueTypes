using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types.ColorModels;

namespace Migs.ValueTypes.Benchmarks.ColorModels
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class CMYKBenchmarks
    {
        [Benchmark]
        public void CMYK_Constructor_Benchmark()
        {
            _ = new CMYK(1, 1, 1, 1);
        }
        
        [Benchmark]
        public void CMYK_From_Benchmark()
        {
            _ = CMYK.From(1, 1, 1, 1);
        }

        [Benchmark]
        public void CMYK_TryFrom_Benchmark()
        {
            _ = CMYK.TryFrom(1, 1, 1, 1, out _);
        }
    }
}
