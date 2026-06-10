using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
    public static class BenchmarkEdward
    {
        public static void Run()
        {
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
        }
    }
}