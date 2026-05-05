using System;
using System.Diagnostics;
using System.Collections.Generic;
using TugasKu_TUBES_KPL;

namespace TugasKu_TUBES_KPL.PerformanceTests
{
    public static class BenchmarkEdward
    {
        public static void Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Persiapan Data Table
            Dictionary<int, string> statusTable = new Dictionary<int, string>()
            {
                {1, "Pending"}, {2, "Approved"}, {3, "Rejected"}
            };

            GenericRepository<TaskItem> repo = new GenericRepository<TaskItem>();

            for (int i = 0; i < 50000; i++)
            {
                // Testing Table-Driven (Lookup)
                string result;
                statusTable.TryGetValue(i % 3 + 1, out result);

                // Testing Generics Repository Add
                repo.Add(new TaskItem() { Name = "Item " + i });
            }

            sw.Stop();
            Console.WriteLine("=== PERFORMANCE TEST: EDWARD (Generics & Table) ===");
            Console.WriteLine("Loop: 50.000 iterasi");
            Console.WriteLine("Waktu Eksekusi: " + sw.ElapsedMilliseconds + " ms");
        }
    }
}