using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Smart.ValueTypes.Types;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Smart.ValueTypes.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    public class EmailAddressBenchmarks
    {
        [Benchmark]
        public void EmailAddress_Constructor_Benchmark()
        {
            _ = new EmailAddress("user@domain.com");
        }

        [Benchmark]
        public void Email_From_Benchmark()
        {
            _ = EmailAddress.From("user@domain.com");
        }

        [Benchmark]
        public void Email_TryFrom_Benchmark()
        {
            _ = EmailAddress.TryFrom("user@domain.com", out _);
        }
    }
}
