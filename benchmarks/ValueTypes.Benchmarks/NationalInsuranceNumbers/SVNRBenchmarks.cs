using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.NationalInsuranceNumbers;

namespace Smart.ValueTypes.Benchmarks.NationalInsuranceNumbers
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class SVNRBenchmarks
    {
        [Benchmark]
        public void SVNR_Constructor_Benchmark()
        {
            _ = new SVNR("7123060697");
        }
        
        [Benchmark]
        public void SVNR_From_Benchmark()
        {
            _ = SVNR.From("7123060697");
        }

        [Benchmark]
        public void SVNR_TryFrom_Benchmark()
        {
            _ = SVNR.TryFrom("7123060697", out _);
        }
    }
}
