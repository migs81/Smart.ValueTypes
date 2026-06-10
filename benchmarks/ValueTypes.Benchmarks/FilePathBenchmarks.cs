using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;

namespace Migs.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class FilePathBenchmarks
    {
        [Benchmark]
        public void FilePath_Constructor_Benchmark()
        {
            _ = new FilePath("text");
        }
    }
}
