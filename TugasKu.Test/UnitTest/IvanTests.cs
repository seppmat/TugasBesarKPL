using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
<<<<<<< HEAD
using System.Configuration;
using TugasKu_TUBES_KPL;
=======
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;    
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e

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
<<<<<<< HEAD
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestValidator_ThrowsOnInvalidTransition()
        {
            var task = new TaskItem { Status = TaskStatus.NotStarted };
            AppStateValidator.ApplyTransition(task, TaskStatus.Done);
=======
        public void TestValidator_ThrowsOnInvalidTransition()
        {
            var task = new TaskItem { Status = TaskStatus.NotStarted };
            Assert.Throws<InvalidOperationException>(() =>
                AppStateValidator.ApplyTransition(task, TaskStatus.Done));
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
        }

        [TestMethod]
        public void TestConfig_FallbackToDefault()
        {
            var color = ConfigManager.LoadColor("KeyTidakAda", System.Drawing.Color.Blue);
            Assert.AreEqual(System.Drawing.Color.Blue, color);
        }
    }
}