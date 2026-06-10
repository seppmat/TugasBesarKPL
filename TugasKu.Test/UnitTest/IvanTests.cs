using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
    // ============================================================
    // UNIT TEST : Automata Validator & Runtime Config (IVAN)
    // Memverifikasi AppStateValidator (izin/tolak transisi)
    // dan ConfigManager (fallback nilai default).
    // ============================================================

    [TestClass]
    public class IvanTests
    {
        // ---- CanTransition: valid ----

        [TestMethod]
        [Description("Transisi NotStarted -> InProgress harus diizinkan.")]
        public void CanTransition_NotStartedToInProgress_ReturnsTrue()
        {
            bool result = AppStateValidator.CanTransition(
                TaskStatus.NotStarted, TaskStatus.InProgress);

            Assert.IsTrue(result,
                "Transisi NotStarted ke InProgress seharusnya valid.");
        }

        // ---- CanTransition: tidak valid ----

        [TestMethod]
        [Description("Transisi langsung NotStarted -> Done tidak diizinkan.")]
        public void CanTransition_NotStartedToDone_ReturnsFalse()
        {
            bool result = AppStateValidator.CanTransition(
                TaskStatus.NotStarted, TaskStatus.Done);

            Assert.IsFalse(result,
                "Transisi NotStarted langsung ke Done seharusnya ditolak.");
        }

        // ---- ApplyTransition: melempar exception ----

        [TestMethod]
        [Description("ApplyTransition ke status yang tidak valid harus melempar InvalidOperationException.")]
        public void ApplyTransition_InvalidTransition_ThrowsInvalidOperation()
        {
            var task = new TaskItem { };
            // task.Status = NotStarted, tidak bisa langsung ke Done

            Assert.ThrowsException<InvalidOperationException>(
                () => AppStateValidator.ApplyTransition(task, TaskStatus.Done),
                "Seharusnya melempar InvalidOperationException untuk transisi yang tidak valid.");
        }

        // ---- ApplyTransition: null task ----

        [TestMethod]
        [Description("ApplyTransition dengan task null harus melempar ArgumentNullException.")]
        public void ApplyTransition_NullTask_ThrowsArgumentNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => AppStateValidator.ApplyTransition(null, TaskStatus.InProgress),
                "Seharusnya melempar ArgumentNullException jika task null.");
        }

        // ---- ConfigManager: fallback warna ----

        [TestMethod]
        [Description("LoadColor dengan key yang tidak ada harus mengembalikan warna default.")]
        public void ConfigManager_LoadColor_MissingKey_ReturnsDefault()
        {
            Color result = ConfigManager.LoadColor("KeyTidakAda", Color.Blue);

            Assert.AreEqual(Color.Blue, result,
                "Warna default harus dikembalikan jika key tidak ditemukan.");
        }

        // ---- ConfigManager: fallback enum ----

        [TestMethod]
        [Description("LoadEnum dengan key yang tidak ada harus mengembalikan nilai enum default.")]
        public void ConfigManager_LoadEnum_MissingKey_ReturnsDefault()
        {
            TaskStatus result = ConfigManager.LoadEnum("KeyTidakAda", TaskStatus.NotStarted);

            Assert.AreEqual(TaskStatus.NotStarted, result,
                "Nilai enum default harus dikembalikan jika key tidak ditemukan.");
        }
    }
}
