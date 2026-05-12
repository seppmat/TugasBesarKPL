using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu.Tests
{
    [TestClass]
    public class YosepTests
    {
        // ============================================
        // GENERIC VALIDATOR TESTS
        // ============================================

        [TestMethod]
        public void TestGenericValidator_ValidCondition_ReturnsTrue()
        {
            var task = new TaskItem { Name = "Valid Task" };
            bool result = GenericValidator<TaskItem>.IsValid(task, t => !string.IsNullOrEmpty(t.Name));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGenericValidator_FalseCondition_ReturnsFalse()
        {
            var task = new TaskItem { Name = "" };
            bool result = GenericValidator<TaskItem>.IsValid(task, t => !string.IsNullOrEmpty(t.Name));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGenericValidator_NullItem_Throws()
        {
            try
            {
                GenericValidator<TaskItem>.IsValid(null, t => true);
                Assert.Fail("Seharusnya melempar ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // Lolos: exception yang diharapkan muncul
            }
        }

        // ============================================
        // UI SCALER TESTS - Pure Logic via ApplyClamping
        // ============================================

        [TestMethod]
        public void TestUIScaler_ClampsMaxValue()
        {
            float result = UIScaler.ApplyClamping("5.0");
            Assert.AreEqual(2.0f, result);
        }

        [TestMethod]
        public void TestUIScaler_ClampsMinValue()
        {
            float result = UIScaler.ApplyClamping("0.1");
            Assert.AreEqual(0.5f, result);
        }

        [TestMethod]
        public void TestUIScaler_ValidValue_PassesThrough()
        {
            float result = UIScaler.ApplyClamping("1.2");
            Assert.AreEqual(1.2f, result);
        }

        [TestMethod]
        public void TestUIScaler_InvalidInput_UsesFallback()
        {
            float result = UIScaler.ApplyClamping("abc");
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void TestUIScaler_EmptyInput_UsesFallback()
        {
            float result = UIScaler.ApplyClamping("");
            Assert.AreEqual(1.0f, result);
        }
    }
}