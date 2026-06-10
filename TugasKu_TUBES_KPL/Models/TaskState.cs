using System;

namespace TugasKu_TUBES_KPL
{
    // ============================================================
    // DESIGN PATTERN : State Pattern
    // TEKNIK KPL     : Automata / State Machine (DAFFA)
    // Setiap state merepresentasikan perilaku berbeda dari TaskItem
    // berdasarkan status saat ini (NotStarted, InProgress, Done).
    // ============================================================

    /// <summary>
    /// Kontrak abstrak untuk semua state tugas.
    /// Mendefinisikan perilaku yang bergantung pada status aktif.
    /// </summary>
    public abstract class TaskState
    {
        /// <summary>Label tampilan untuk status ini.</summary>
        public abstract string Label { get; }

        /// <summary>Apakah tugas boleh diedit dalam state ini.</summary>
        public abstract bool CanEdit { get; }

        /// <summary>Apakah tugas boleh dihapus dalam state ini.</summary>
        public abstract bool CanDelete { get; }

        /// <summary>
        /// Melakukan transisi ke status berikutnya.
        /// Melempar <see cref="InvalidOperationException"/> jika transisi tidak valid.
        /// </summary>
        public abstract void Transition(TaskItem task, TaskStatus next);

        /// <summary>
        /// Guard clause terpusat: validasi target sebelum transisi.
        /// Mengurangi duplikasi logika di setiap subclass.
        /// </summary>
        protected void EnforceTransition(TaskItem task, TaskStatus next, TaskStatus allowed)
        {
            if (next != allowed)
                throw new InvalidOperationException(
                    $"Dari state '{Label}', transisi hanya ke '{allowed}' yang diizinkan.");

            task.SetStatus(next);
        }
    }

    /// <summary>
    /// State: Tugas belum dimulai.
    /// Hanya bisa transisi ke InProgress.
    /// </summary>
    public class NotStartedState : TaskState
    {
        public override string Label    => "⏳ Belum";
        public override bool   CanEdit   => true;
        public override bool   CanDelete => true;

        public override void Transition(TaskItem task, TaskStatus next)
            => EnforceTransition(task, next, TaskStatus.InProgress);
    }

    /// <summary>
    /// State: Tugas sedang dikerjakan.
    /// Hanya bisa transisi ke Done.
    /// </summary>
    public class InProgressState : TaskState
    {
        public override string Label    => "🔄 Proses";
        public override bool   CanEdit   => true;
        public override bool   CanDelete => true;

        public override void Transition(TaskItem task, TaskStatus next)
            => EnforceTransition(task, next, TaskStatus.Done);
    }

    /// <summary>
    /// State: Tugas selesai.
    /// Tidak bisa diedit, hanya bisa direset ke NotStarted atau dihapus.
    /// </summary>
    public class DoneState : TaskState
    {
        public override string Label    => "✅ Selesai";
        public override bool   CanEdit   => false;
        public override bool   CanDelete => true;

        public override void Transition(TaskItem task, TaskStatus next)
            => EnforceTransition(task, next, TaskStatus.NotStarted);
    }
}
