<<<<<<< HEAD
// using System.Diagnostics;
//using System.Drawing;
//using TugasKu_TUBES_KPL;
//using TugasKu_TUBES_KPL.Core;
// ...
//var sw = Stopwatch.StartNew();
//var task = new TaskItem { Name = "Valid", Deadline = DateTime.Today.AddDays(1) };
//for (int i = 0; i < 50000; i++)
//{
//GenericValidator<TaskItem>.IsValid(task, t => !string.IsNullOrEmpty(t.Name));
//var scaled = UIScaler.ScaleFont(new Font("Segoe UI", 10));
//}
//sw.Stop();
//Console.WriteLine($"Yosep (Generics Validator + UI Scaling): {sw.ElapsedMilliseconds} ms");
=======
using System;
using System.Diagnostics;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu.Test
{
    public static class BenchmarkYosep
    {
        public static void Run()
        {
            Console.WriteLine("\n=== YOSEP: Generic Validator (Delegate) + UI Scaling ===");
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();

            // Load config sekali di awal
            try { UIScaler.LoadSettings(); } catch { }
            var sampleTask = new TaskItem { Name = "Test", Deadline = DateTime.Today.AddDays(1) };
            var baseFont = new System.Drawing.Font("Segoe UI", 10);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                // Generic Validator with Delegate invocation
                bool isValid = GenericValidator<TaskItem>.IsValid(sampleTask, t => !string.IsNullOrEmpty(t.Name));
                // Runtime UI Scaling calculation
                var scaledFont = UIScaler.ScaleFont(baseFont);
            }
            sw.Stop();
            Console.WriteLine($"Loop: 50.000 | Waktu: {sw.ElapsedMilliseconds} ms | Rata-rata: {(sw.ElapsedTicks / 50000.0):F2} ticks/iterasi");
        }
    }
}
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
