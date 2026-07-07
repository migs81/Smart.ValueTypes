using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.NationalInsuranceNumbers;

namespace Smart.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class SVNRBenchmarks
    {
        [Benchmark]
        public void SVNR_Constructor_Benchmark()
        {
            _ = new SVNR("1945130781");
        }
    }
}
