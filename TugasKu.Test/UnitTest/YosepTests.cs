using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu.Tests
{
    [TestClass]
    public class YosepTests
    {
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGenericValidator_NullItem_Throws()
        {
            GenericValidator<TaskItem>.IsValid(null, t => true);
        }

        [TestMethod]
        public void TestUIScaler_ClampsMaxValue()
        {
            ConfigurationManager.AppSettings["UIFontScale"] = "5.0";
            UIScaler.LoadSettings();
            Assert.AreEqual(2.0f, UIScaler.CurrentScale);
        }

        [TestMethod]
        public void TestUIScaler_ClampsMinValue()
        {
            ConfigurationManager.AppSettings["UIFontScale"] = "0.1";
            UIScaler.LoadSettings();
            Assert.AreEqual(0.5f, UIScaler.CurrentScale);
        }
    }
}