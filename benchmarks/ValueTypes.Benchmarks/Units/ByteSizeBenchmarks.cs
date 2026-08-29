using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Units;

namespace Smart.ValueTypes.Benchmarks.Units
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class ByteSizeBenchmarks
    {
        [Benchmark]
        public void ByteSize_Constructor_Benchmark()
        {
            _ = new ByteSize(1_234_567_890);
        }
        
        [Benchmark]
        public void ByteSize_FromTebibytes_Benchmark()
        {
            _ = ByteSize.FromTebibytes(10);
        }
    }
}
