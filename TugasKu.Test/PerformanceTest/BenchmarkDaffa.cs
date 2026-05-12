using System.Diagnostics;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.TugasKu.Tests
{
    public static class BenchmarkDaffa
    {
        public static void Run()
        {
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
        }
    }
}