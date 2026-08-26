using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.IO;

namespace Smart.ValueTypes.Benchmarks.IO
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class WindowsFilePathBenchmarks
    {
        [Benchmark]
        public void WindowsFilePath_Constructor_Benchmark()
        {
            _ = new WindowsFilePath(@"C:\test\temp.log");
        }
        
        [Benchmark]
        public void WindowsFilePath_Parse_Benchmark()
        {
            _ = WindowsFilePath.Parse(@"C:\\test/temp.log");
        }
    }
}
