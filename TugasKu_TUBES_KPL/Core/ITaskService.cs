using System.Collections.Generic;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // TEKNIK KPL : API / Interface (ROZI)
    // ITaskService mendefinisikan kontrak layanan tugas.
    // Memisahkan antarmuka dari implementasi sehingga mudah
    // diganti (misal: untuk testing dengan mock).
    // ============================================================

    /// <summary>
    /// Kontrak layanan untuk operasi CRUD tugas kuliah.
    /// Implementasi bisa diganti tanpa mengubah kode pemanggil.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>Mengembalikan salinan seluruh daftar tugas.</summary>
        List<TaskItem> GetAllTasks();

        /// <summary>Menambahkan tugas baru setelah melewati validasi.</summary>
        void AddTask(TaskItem task);

        /// <summary>Memperbarui tugas pada indeks tertentu.</summary>
        void UpdateTask(int index, TaskItem task);

        /// <summary>Menghapus tugas pada indeks tertentu.</summary>
        void DeleteTask(int index);
    }
}
