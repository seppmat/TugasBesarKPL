using System;
using System.Diagnostics;
using TugasKu_TUBES_KPL;

namespace TugasKu_TUBES_KPL.PerformanceTests
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
}