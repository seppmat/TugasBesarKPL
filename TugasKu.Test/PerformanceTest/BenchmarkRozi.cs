using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu_TUBES_KPL.TugasKu.Tests
{
    public static class BenchmarkRozi
    {
        public static void Run()
        {
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
        }
    }
}