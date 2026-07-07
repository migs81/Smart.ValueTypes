using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types;
using Smart.ValueTypes.Unfinished;

namespace Smart.ValueTypes.Benchmarks
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
