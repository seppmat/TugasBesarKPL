using System.Diagnostics;
using System.Windows.Forms;
using TugasKu_TUBES_KPL;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
    public static class BenchmarkDaffa
    {
        public static void Run()
        {
            Console.WriteLine("\n=== DAFFA: Automata (State Pattern) ===");
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                var task = new TaskItem();
                task.SetStatus(TaskStatus.InProgress);
                bool canEdit = task.CurrentState.CanEdit;
                bool canDelete = task.CurrentState.CanDelete;
            }
            sw.Stop();
            Console.WriteLine($"Loop: 50.000 | Waktu: {sw.ElapsedMilliseconds} ms | Rata-rata: {(sw.ElapsedTicks / 50000.0):F2} ticks/iterasi");
        }
    }
}