using System;
using System.Collections.Generic;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Observer Pattern (event-based)
    // TEKNIK KPL     : API / Interface (ROZI)
    // SECURE CODING  : (ROZI)
    //   [SC-11] Batas maksimum jumlah tugas mencegah memory exhaustion
    //   [SC-12] GetAllTasks() mengembalikan salinan untuk mencegah
    //           caller memodifikasi state internal secara langsung
    // ============================================================

    public class TaskService : ITaskService
    {
        // [SC-11] Batas maksimum tugas yang boleh disimpan
        private const int MaxTaskCount = 1000;

        private readonly List<TaskItem> _tasks = new List<TaskItem>();

        // ---- Observer Events ----

        public event EventHandler<TaskItem> TaskAdded;
        public event EventHandler<TaskItem> TaskUpdated;
        public event EventHandler<int>      TaskDeleted;

        // ---- ITaskService Implementation ----

        /// <inheritdoc/>
        /// [SC-12] Mengembalikan salinan defensif — caller tidak bisa mutasi _tasks.
        public List<TaskItem> GetAllTasks() => new List<TaskItem>(_tasks);

        /// <inheritdoc/>
        /// [SC-11] Menolak penambahan jika sudah mencapai batas maksimum.
        public void AddTask(TaskItem task)
        {
            ValidateTask(task);

            // [SC-11] Cegah penumpukan data tak terbatas
            if (_tasks.Count >= MaxTaskCount)
                throw new InvalidOperationException(
                    $"Batas maksimum tugas ({MaxTaskCount}) telah tercapai.");

            _tasks.Add(task);
            TaskAdded?.Invoke(this, task);
        }

        /// <inheritdoc/>
        public void UpdateTask(int index, TaskItem task)
        {
            ValidateIndex(index);
            ValidateTask(task);
            _tasks[index] = task;
            TaskUpdated?.Invoke(this, task);
        }

        /// <inheritdoc/>
        public void DeleteTask(int index)
        {
            ValidateIndex(index);
            _tasks.RemoveAt(index);
            TaskDeleted?.Invoke(this, index);
        }

        // ---- Private Guard Clauses ----

        private void ValidateTask(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Tugas tidak boleh null.");

            if (!task.ValidateAll())
                throw new ArgumentException(
                    "Data tugas tidak valid: nama/matkul kosong atau deadline tidak valid.",
                    nameof(task));
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= _tasks.Count)
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    $"Indeks {index} di luar batas. Jumlah tugas: {_tasks.Count}.");
        }
    }
}
