using System;

namespace TugasKu_TUBES_KPL
{
    // Teknik Automata: Validator transisi state
    public static class AppStateValidator
    {
        // Cek apakah perpindahan status diizinkan
        public static bool CanTransition(TaskStatus current, TaskStatus target)
        {
            if (current == target) return false;

            // Aturan transisi otomatis:
            // Belum -> Proses (OK)
            // Proses -> Selesai (OK)
            // Selesai -> Belum (OK, untuk reset)
            // Selain itu -> Ditolak
            switch (current)
            {
                case TaskStatus.NotStarted:
                    return target == TaskStatus.InProgress;
                case TaskStatus.InProgress:
                    return target == TaskStatus.Done;
                case TaskStatus.Done:
                    return target == TaskStatus.NotStarted;
                default:
                    return false;
            }
        }

        // Terapkan transisi dengan pertahanan error
        public static void ApplyTransition(TaskItem task, TaskStatus newStatus)
        {
            if (!CanTransition(task.Status, newStatus))
            {
                throw new InvalidOperationException
                    ("Perpindahan status tidak diizinkan. Urutan: Belum > Proses > Selesai.");
            }
            task.Status = newStatus;
        }
    }
}