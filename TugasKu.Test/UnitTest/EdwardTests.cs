using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
    // ============================================================
    // UNIT TEST : Repository Pattern & Table-Driven (EDWARD)
    // Memverifikasi operasi CRUD GenericRepository<T> dan
    // lookup tabel berbasis Dictionary.
    // ============================================================

    [TestClass]
    public class EdwardTests
    {
        // ---- Add & Count ----

        [TestMethod]
        [Description("Setelah Add satu item, GetAll harus mengembalikan tepat 1 item.")]
        public void Repository_Add_SingleItem_CountIsOne()
        {
            var repo = new GenericRepository<TaskItem>();
            repo.Add(new TaskItem { Name = "Test Task" });

            Assert.AreEqual(1, repo.GetAll().Count,
                "Repository seharusnya berisi 1 item setelah Add.");
        }

        // ---- Update dengan indeks tidak valid ----

        [TestMethod]
        [Description("Update dengan indeks di luar batas harus melempar ArgumentOutOfRangeException.")]
        public void Repository_Update_InvalidIndex_ThrowsArgumentOutOfRange()
        {
            var repo = new GenericRepository<TaskItem>();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => repo.Update(99, new TaskItem()),
                "Seharusnya melempar exception untuk indeks yang tidak valid.");
        }

        // ---- Delete & Count berkurang ----

        [TestMethod]
        [Description("Setelah Delete, jumlah item harus berkurang satu.")]
        public void Repository_Delete_ReducesCountByOne()
        {
            var repo = new GenericRepository<TaskItem>();
            repo.Add(new TaskItem { Name = "Task A" });
            repo.Add(new TaskItem { Name = "Task B" });

            repo.Delete(0);

            Assert.AreEqual(1, repo.GetAll().Count,
                "Count seharusnya berkurang setelah Delete.");
        }

        // ---- GetAll mengembalikan salinan defensif ----

        [TestMethod]
        [Description("GetAll harus mengembalikan salinan baru, bukan referensi internal.")]
        public void Repository_GetAll_ReturnsCopy_NotReference()
        {
            var repo = new GenericRepository<TaskItem>();
            repo.Add(new TaskItem { Name = "Task X" });

            var list1 = repo.GetAll();
            list1.Clear();
            var list2 = repo.GetAll();

            Assert.AreEqual(1, list2.Count,
                "Modifikasi list hasil GetAll tidak boleh mempengaruhi data internal.");
        }

        // ---- Table-Driven: lookup berhasil ----

        [TestMethod]
        [Description("TryGetValue pada tabel yang valid harus berhasil dan mengembalikan nilai yang benar.")]
        public void TableDriven_TryGetValue_ExistingKey_ReturnsCorrectValue()
        {
            var table = new Dictionary<int, TaskStatus> { { 1, TaskStatus.InProgress } };

            bool found = table.TryGetValue(1, out TaskStatus result);

            Assert.IsTrue(found, "Key 1 seharusnya ditemukan di tabel.");
            Assert.AreEqual(TaskStatus.InProgress, result,
                "Nilai yang dikembalikan harus InProgress.");
        }

        // ---- Table-Driven: lookup gagal ----

        [TestMethod]
        [Description("TryGetValue untuk key yang tidak ada harus mengembalikan false.")]
        public void TableDriven_TryGetValue_MissingKey_ReturnsFalse()
        {
            var table = new Dictionary<int, TaskStatus> { { 1, TaskStatus.InProgress } };

            bool found = table.TryGetValue(99, out _);

            Assert.IsFalse(found, "Key 99 tidak ada, seharusnya TryGetValue return false.");
        }
    }
}
