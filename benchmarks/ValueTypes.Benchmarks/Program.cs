using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using Smart.ValueTypes.Benchmarks.Address.Austria;
using Smart.ValueTypes.Types.Address.Austria;

namespace Smart.ValueTypes.Benchmarks
{
    internal abstract class Program
    {
        private static void Main(string[] args)
        {
            // _ = BenchmarkRunner.Run<AustrianAddressCodeBenchmarks>();
            MeasureExecutionTimeOf(() => new AustrianAddressCode("12345"), 100_000_000);
            
            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        #region tests

        private static void MeasureExecutionTimeOf<T>(Func<T> func, int times)
        {
            Console.WriteLine();
            Console.WriteLine("---------- Measure execution time of ----------");
            Console.Write($"Creating '{typeof(T).Name}' {times:N0} times...");
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < times; i++)
                func.Invoke();

            stopwatch.Stop();
            Console.WriteLine($"needed {stopwatch.Elapsed.TotalSeconds} seconds");
            Console.WriteLine("-------------------- End ----------------------");
        }

        #endregion
    }
}
