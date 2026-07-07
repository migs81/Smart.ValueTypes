using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Unfinished;

namespace Smart.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class TextBenchmarks
    {
        [Benchmark]
        public void Text_Constructor_Benchmark()
        {
            _ = new Text("text");
        }

        [Benchmark]
        public void Text_From_Benchmark()
        {
            _ = Text.From("text");
        }

        [Benchmark]
        public void Text_TryFrom_Benchmark()
        {
            _ = Text.TryFrom("text", out _);
        }
    }
}
