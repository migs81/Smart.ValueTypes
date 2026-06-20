using BenchmarkDotNet.Running;
using Migs.ValueTypes.Benchmarks.Temperatures;
using Migs.ValueTypes.Types.Temperatures;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Migs.ValueTypes.Types;

namespace Migs.ValueTypes.Benchmarks
{
    /*
     * Input (Allowed chars only)
     * PIN?
     * 
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            //var celsius = new Celsius();

            // temperatures
            _ = BenchmarkRunner.Run<CelsiusBenchmarks>();
            //_ = BenchmarkRunner.Run<KelvinBenchmarks>();
            //_ = BenchmarkRunner.Run<FahrenheitBenchmarks>();

            //_ = BenchmarkRunner.Run<TextBenchmarks>();
            //_ = BenchmarkRunner.Run<IBANBenchmarks>();
            //_ = BenchmarkRunner.Run<BICBenchmarks>();
            //_ = BenchmarkRunner.Run<CurrencyBenchmarks>();
            //_ = BenchmarkRunner.Run<EmailAddressBenchmarks>();
            //_ = BenchmarkRunner.Run<IMEIBenchmarks>();
            //_ = BenchmarkRunner.Run<FilePathBenchmarks>();
            //_ = BenchmarkRunner.Run<IPBenchmarks>();
            //_ = BenchmarkRunner.Run<IPv4Benchmarks>();
            //_ = BenchmarkRunner.Run<IPv6Benchmarks>();
            //_ = BenchmarkRunner.Run<MD5Benchmarks>();
            //_ = BenchmarkRunner.Run<MoneyBenchmarks>();
            //_ = BenchmarkRunner.Run<PasswordBenchmarks>();
            //_ = BenchmarkRunner.Run<SHA1Benchmarks>();
            //_ = BenchmarkRunner.Run<SVNRBenchmarks>();
            //_ = BenchmarkRunner.Run<UrlBenchmarks>();

            //TryCreateMailAddressFrom("name@domain.com");
            //TryCreateMailAddressFrom("name@domain");
            //TryCreateMailAddressFrom("[DisplayName] name@domain");

            //var x = new MailAddress("[DisplayName] name@domain");
            //var z = new MailAddress(" DisplayName name@domain ");

            //MeasureExecutionTimeOf(() => new EmailAddress("name@domain.com"), 100_000_000);

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        #region tests

        private static void TryCreateMailAddressFrom(string emailAddress)
        {
            Console.Write($"Creating '{emailAddress}'...");
            if (TryExecute(() => _ = new MailAddress(emailAddress)))
                Console.WriteLine("OK");
            else
                Console.WriteLine("ERROR");
        }

        private static bool TryExecute(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void MeasureExecutionTimeOf<T>(Func<T> func, int times)
        {
            Console.Write($"Testing '{typeof(T).Name}' {times:N0} times...");
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < times; i++)
                func.Invoke();

            stopwatch.Stop();
            Console.WriteLine($"needed {stopwatch.Elapsed.TotalSeconds} seconds");
        }

        #endregion
    }
}
