using System;
using System.Diagnostics;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
<<<<<<< HEAD

namespace TugasKu_TUBES_KPL.TugasKu.Tests
{
	public static class BenchmarkIvan
	{
		public static void Run()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			for (int i = 0; i < 50000; i++)
			{
				// Testing Runtime Configuration Load
				var color = ConfigManager.LoadColor("PrimaryColor", System.Drawing.Color.Red);

				// Testing Automata Validation
				bool isValid = AppStateValidator.CanTransition(TaskStatus.NotStarted, TaskStatus.Done);
			}

			sw.Stop();
			Console.WriteLine("=== PERFORMANCE TEST: IVAN (Runtime Config) ===");
			Console.WriteLine("Loop: 50.000 iterasi");
			Console.WriteLine("Waktu Eksekusi: " + sw.ElapsedMilliseconds + " ms");
		}
	}
=======
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
	public static class BenchmarkIvan
    {
        public static void Run()
        {
            Console.WriteLine("\n=== IVAN: Automata Validation + Runtime Config ===");
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                // Validasi transisi state
                bool valid = AppStateValidator.CanTransition(TaskStatus.NotStarted, TaskStatus.InProgress);
                // Simulasi baca config (dibungkus try-catch agar tidak crash jika config belum siap)
                try { var _ = System.Configuration.ConfigurationManager.AppSettings["UIFontScale"]; } catch { }
            }
            sw.Stop();
            Console.WriteLine($"Loop: 50.000 | Waktu: {sw.ElapsedMilliseconds} ms | Rata-rata: {(sw.ElapsedTicks / 50000.0):F2} ticks/iterasi");
        }
    }
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
}