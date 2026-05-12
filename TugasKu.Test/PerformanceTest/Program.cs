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
        }
    }
}