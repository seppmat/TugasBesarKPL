using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
<<<<<<< HEAD
using TugasKu_TUBES_KPL.Core;

namespace TugasKu_TUBES_KPL.TugasKu.Tests
=======
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
{
    public static class BenchmarkRozi
    {
        public static void Run()
        {
<<<<<<< HEAD
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ITaskService service = new TaskService();
            var styleTable = new Dictionary<TaskPriority, Color>
            {
                { TaskPriority.High, Color.Red }
            };

            for (int i = 0; i < 50000; i++)
            {
                // Testing API Call (Interface Dispatch)
                service.AddTask(new TaskItem() { Name = "Task " + i });

                // Testing Table-Driven Style Lookup
                Color c;
                styleTable.TryGetValue(TaskPriority.High, out c);
            }

            sw.Stop();
            Console.WriteLine("=== PERFORMANCE TEST: ROZI (API & Table) ===");
            Console.WriteLine("Loop: 50.000 iterasi");
            Console.WriteLine("Waktu Eksekusi: " + sw.ElapsedMilliseconds + " ms");
=======
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
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
        }
    }
}