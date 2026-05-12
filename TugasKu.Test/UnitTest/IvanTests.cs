using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;    

namespace TugasKu.Tests
{
    [TestClass]
    public class IvanTests
    {
        [TestMethod]
        public void TestValidator_AllowNotStartedToInProgress()
        {
            bool result = AppStateValidator.CanTransition(TaskStatus.NotStarted, TaskStatus.InProgress);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestValidator_BlockNotStartedToDone()
        {
            bool result = AppStateValidator.CanTransition(TaskStatus.NotStarted, TaskStatus.Done);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestValidator_ThrowsOnInvalidTransition()
        {
            var task = new TaskItem { Status = TaskStatus.NotStarted };
            Assert.Throws<InvalidOperationException>(() =>
                AppStateValidator.ApplyTransition(task, TaskStatus.Done));
        }

        [TestMethod]
        public void TestConfig_FallbackToDefault()
        {
            var color = ConfigManager.LoadColor("KeyTidakAda", System.Drawing.Color.Blue);
            Assert.AreEqual(System.Drawing.Color.Blue, color);
        }
    }
}