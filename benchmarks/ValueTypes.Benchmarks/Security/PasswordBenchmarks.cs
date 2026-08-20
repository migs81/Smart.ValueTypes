using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types.Security;

namespace Smart.ValueTypes.Benchmarks.Security
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class PasswordBenchmarks
    {
        [Benchmark]
        public void Password_Constructor_Benchmark()
        {
            _ = new Password("abc123456", 8, 15,
                Password.Requirement.Numbers | Password.Requirement.LowercaseLetters);
        }

        [Benchmark]
        public void Password_TryFrom_Benchmark()
        {
            _ = Password.TryFrom("abc123456", 8, 15,
                Password.Requirement.Numbers | Password.Requirement.LowercaseLetters, out _);
        }

        [Benchmark]
        public void Password_From_Benchmark()
        {
            _ = Password.From("abc123456", 8, 15,
                Password.Requirement.Numbers | Password.Requirement.LowercaseLetters);
        }
    }
}
