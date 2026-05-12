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