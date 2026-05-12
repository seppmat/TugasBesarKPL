<<<<<<< HEAD
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

namespace TugasKu_TUBES_KPL.TugasKu.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkDaffa.Run();
            BenchmarkEdward.Run();
            BenchmarkIvan.Run();
            BenchmarkRozi.Run();

            Console.WriteLine("\nSemua benchmark selesai.");
            Console.ReadKey();
=======
﻿using System;
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
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
        }
    }
}