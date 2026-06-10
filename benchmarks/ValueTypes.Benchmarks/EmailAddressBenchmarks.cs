using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Migs.ValueTypes.Types;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Migs.ValueTypes.Benchmarks
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
        public void MailAddress_Constructor_Benchmark()
        {
            _ = new MailAddress("user@domain.com");
        }

        [Benchmark]
        public void Regex_Benchmark()
        {
            _ = Regex.Match("user@domain.com", @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])").Value;
        }

        //[Benchmark]
        //public void Email_From_Benchmark()
        //{
        //    _ = EmailAddress.From("a@a.com");
        //}

        //[Benchmark]
        //public void Email_TryFrom_Benchmark()
        //{
        //    _ = EmailAddress.TryFrom("a@a.com", out _);
        //}
    }
}
