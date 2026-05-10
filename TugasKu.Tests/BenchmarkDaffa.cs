using System;
using System.Diagnostics;
using TugasKu_TUBES_KPL;

namespace TugasKu_TUBES_KPL.PerformanceTests
{
    public static class BenchmarkDaffa
    {
        public static void Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Simulasi loop 50.000 iterasi
            for (int i = 0; i < 50000; i++)
            {
                TaskItem task = new TaskItem();
                task.SetStatus(TaskStatus.InProgress);

                // Mengakses behavior dari State Object
                bool isEditable = task.CurrentState.CanEdit;
            }

            sw.Stop();
            Console.WriteLine("=== PERFORMANCE TEST: DAFFA (Automata) ===");
            Console.WriteLine("Loop: 50.000 iterasi");
            Console.WriteLine("Waktu Eksekusi: " + sw.ElapsedMilliseconds + " ms");
        }
    }
}