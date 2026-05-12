using System;
<<<<<<< HEAD
using System.Diagnostics;
using System.Collections.Generic;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu_TUBES_KPL.TugasKu.Tests
=======
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
{
    public static class BenchmarkEdward
    {
        public static void Run()
        {
<<<<<<< HEAD
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
=======
            Console.WriteLine("\n=== EDWARD: Table-Driven Lookup + Generics Add ===");
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();

            var table = new Dictionary<int, TaskStatus> { { 1, TaskStatus.NotStarted }, { 2, TaskStatus.InProgress }, { 3, TaskStatus.Done } };
            var list = new List<TaskItem>();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                // Table-Driven Lookup (O(1))
                table.TryGetValue(i % 3 + 1, out TaskStatus status);
                // Generics Add (Type-safe collection)
                list.Add(new TaskItem { Name = "Item " + i, Status = status });
            }
            sw.Stop();
            Console.WriteLine($"Loop: 50.000 | Waktu: {sw.ElapsedMilliseconds} ms | Rata-rata: {(sw.ElapsedTicks / 50000.0):F2} ticks/iterasi");
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
        }
    }
}