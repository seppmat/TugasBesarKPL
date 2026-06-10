using System;
using TugasKu.Test;
using TugasKu.Tests;

namespace TugasKu.Test.PerformanceTests
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("🚀 MEMULAI PERFORMANCE TESTING...");
            Console.WriteLine("================================");

            BenchmarkDaffa.Run();
            BenchmarkIvan.Run();
            BenchmarkEdward.Run();
            BenchmarkRozi.Run();
            BenchmarkYosep.Run();

            Console.WriteLine("\n================================");
            Console.WriteLine("✅ SELESAI. Tekan Enter untuk keluar.");
            Console.ReadLine();
        }
    }
}