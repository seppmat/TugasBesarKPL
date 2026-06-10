using System;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Facade Pattern
    // TEKNIK KPL     : Automata Validator (IVAN)
    // SECURE CODING  : (IVAN)
    //   [SC-8]  Enum range validation — menolak nilai enum di luar
    //           rentang yang terdefinisi (cast integer sembarang)
    // ============================================================

    public static class AppStateValidator
    {
        /// <summary>
        /// [SC-8] Memeriksa apakah nilai TaskStatus adalah anggota enum yang valid
        /// sebelum meneruskan ke tabel transisi.
        /// </summary>
        public static bool CanTransition(TaskStatus current, TaskStatus target)
        {
            if (!IsDefinedStatus(current) || !IsDefinedStatus(target))
                return false;

            return TaskItem.TransitionTable.TryGetValue(current, out TaskStatus[] allowed)
                   && Array.IndexOf(allowed, target) >= 0;
        }

        /// <summary>
        /// Menerapkan transisi ke status baru pada tugas yang diberikan.
        /// [SC-8] Validasi enum dilakukan sebelum transisi.
        /// </summary>
        /// <exception cref="ArgumentNullException">Jika task null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Jika newStatus bukan nilai enum valid.</exception>
        /// <exception cref="InvalidOperationException">Jika transisi tidak diizinkan.</exception>
        public static void ApplyTransition(TaskItem task, TaskStatus newStatus)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            // [SC-8] Tolak nilai enum yang di-cast dari integer sembarang
            if (!IsDefinedStatus(newStatus))
                throw new ArgumentOutOfRangeException(
                    nameof(newStatus),
                    $"Nilai '{(int)newStatus}' bukan TaskStatus yang valid.");

            if (!task.TryTransitionTo(newStatus))
                throw new InvalidOperationException(
                    $"Transisi dari '{task.Status}' ke '{newStatus}' tidak diizinkan.");
        }

        // ---- Private Helper ----

        /// <summary>
        /// [SC-8] Memverifikasi bahwa nilai enum terdefinisi di deklarasi enum-nya.
        /// Mencegah integer cast seperti (TaskStatus)999 lolos tanpa terdeteksi.
        /// </summary>
        private static bool IsDefinedStatus(TaskStatus status)
            => Enum.IsDefined(typeof(TaskStatus), status);
    }
}
