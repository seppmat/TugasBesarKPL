using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
    public static class BenchmarkRozi
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ROZI: API Service Call + Table-Driven Styling ===");
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();

            // Simulasi Interface Dispatch & Table Lookup
            var styleTable = new Dictionary<TaskPriority, Color> { { TaskPriority.High, Color.Red }, { TaskPriority.Low, Color.Green } };

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                // Table-Driven Styling Lookup
                styleTable.TryGetValue(TaskPriority.High, out Color c);
                // Simulasi API/Interface call overhead
                var task = new TaskItem { Name = "API_Test_" + i };
            }
            sw.Stop();
            Console.WriteLine($"Loop: 50.000 | Waktu: {sw.ElapsedMilliseconds} ms | Rata-rata: {(sw.ElapsedTicks / 50000.0):F2} ticks/iterasi");
        }
    }
}