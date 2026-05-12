using System.Diagnostics;
using System.Windows.Forms;
<<<<<<< HEAD

namespace TugasKu_TUBES_KPL.TugasKu.Tests
=======
using TugasKu_TUBES_KPL;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
{
    public static class BenchmarkDaffa
    {
        public static void Run()
        {
<<<<<<< HEAD
            Stopwatch sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < 50000; i++)
            {
                TaskItem task = new TaskItem();

                task.SetStatus(TaskStatus.InProgress);

                bool editable =
                    task.CurrentState.CanEdit;
            }

            sw.Stop();

            MessageBox.Show(
                "=== PERFORMANCE TEST: DAFFA ===\n\n" +
                "Loop: 50.000 iterasi\n" +
                "Waktu: " +
                sw.ElapsedMilliseconds +
                " ms");
=======
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
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
        }
    }
}