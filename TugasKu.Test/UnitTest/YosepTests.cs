using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu.Tests
{
    // ============================================================
    // UNIT TEST : Strategy Pattern (via Delegate) & UIScaler (YOSEP)
    // Memverifikasi GenericValidator<T> dengan berbagai strategi
    // validasi, serta clamping logic UIScaler.
    // ============================================================

    [TestClass]
    public class YosepTests
    {
        // ============================================================
        // GENERIC VALIDATOR (Strategy Pattern via Delegate)
        // ============================================================

        [TestMethod]
        [Description("IsValid dengan kondisi terpenuhi harus mengembalikan true.")]
        public void GenericValidator_ValidCondition_ReturnsTrue()
        {
            var task   = new TaskItem { Name = "Tugas Valid" };
            bool result = GenericValidator<TaskItem>.IsValid(
                task, t => !string.IsNullOrEmpty(t.Name));

            Assert.IsTrue(result,
                "Validator seharusnya return true untuk kondisi yang terpenuhi.");
        }

        [TestMethod]
        [Description("IsValid dengan kondisi tidak terpenuhi harus mengembalikan false.")]
        public void GenericValidator_FalseCondition_ReturnsFalse()
        {
            var task   = new TaskItem { Name = "" };
            bool result = GenericValidator<TaskItem>.IsValid(
                task, t => !string.IsNullOrEmpty(t.Name));

            Assert.IsFalse(result,
                "Validator seharusnya return false untuk kondisi yang tidak terpenuhi.");
        }

        [TestMethod]
        [Description("IsValid dengan item null harus melempar ArgumentNullException.")]
        public void GenericValidator_NullItem_ThrowsArgumentNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GenericValidator<TaskItem>.IsValid(null, t => true),
                "Seharusnya melempar ArgumentNullException jika item null.");
        }

        [TestMethod]
        [Description("IsValid dengan condition null harus melempar ArgumentNullException.")]
        public void GenericValidator_NullCondition_ThrowsArgumentNull()
        {
            var task = new TaskItem { Name = "Test" };

            Assert.ThrowsException<ArgumentNullException>(
                () => GenericValidator<TaskItem>.IsValid(task, null),
                "Seharusnya melempar ArgumentNullException jika condition null.");
        }

        [TestMethod]
        [Description("Strategi validasi yang berbeda bisa dipakai untuk objek yang sama (Strategy Pattern).")]
        public void GenericValidator_DifferentStrategies_SameObject_DifferentResults()
        {
            var task = new TaskItem
            {
                Name     = "Tugas",
                Course   = "",
                Deadline = DateTime.Today.AddDays(-1)
            };

            bool nameValid    = GenericValidator<TaskItem>.IsValid(task, t => !string.IsNullOrEmpty(t.Name));
            bool courseValid  = GenericValidator<TaskItem>.IsValid(task, t => !string.IsNullOrEmpty(t.Course));

            Assert.IsTrue(nameValid,   "Nama tidak kosong, seharusnya valid.");
            Assert.IsFalse(courseValid, "Course kosong, seharusnya tidak valid.");
        }

        // ============================================================
        // UI SCALER (Clamping Logic)
        // ============================================================

        [TestMethod]
        [Description("Nilai di atas MaxScale (2.0) harus diclamping ke 2.0.")]
        public void UIScaler_ValueAboveMax_ClampsToMax()
        {
            float result = UIScaler.ApplyClamping("5.0");
            Assert.AreEqual(2.0f, result, "Nilai 5.0 seharusnya diclamping ke 2.0.");
        }

        [TestMethod]
        [Description("Nilai di bawah MinScale (0.5) harus diclamping ke 0.5.")]
        public void UIScaler_ValueBelowMin_ClampsToMin()
        {
            float result = UIScaler.ApplyClamping("0.1");
            Assert.AreEqual(0.5f, result, "Nilai 0.1 seharusnya diclamping ke 0.5.");
        }

        [TestMethod]
        [Description("Nilai dalam rentang valid harus diteruskan tanpa perubahan.")]
        public void UIScaler_ValidValue_PassesThroughUnchanged()
        {
            float result = UIScaler.ApplyClamping("1.2");
            Assert.AreEqual(1.2f, result, 0.0001f,
                "Nilai 1.2 dalam rentang valid seharusnya tidak berubah.");
        }

        [TestMethod]
        [Description("Input bukan angka harus menggunakan fallback 1.0.")]
        public void UIScaler_InvalidInput_FallsBackToDefault()
        {
            float result = UIScaler.ApplyClamping("bukan_angka");
            Assert.AreEqual(1.0f, result, "Input tidak valid seharusnya menghasilkan fallback 1.0.");
        }

        [TestMethod]
        [Description("Input kosong harus menggunakan fallback 1.0.")]
        public void UIScaler_EmptyInput_FallsBackToDefault()
        {
            float result = UIScaler.ApplyClamping("");
            Assert.AreEqual(1.0f, result, "Input kosong seharusnya menghasilkan fallback 1.0.");
        }
    }
}
