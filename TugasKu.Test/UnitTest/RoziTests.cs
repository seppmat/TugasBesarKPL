using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu.Tests
{
    [TestClass]
    public class RoziTests
    {
        [TestMethod]
        public void TestService_AddTask_Success()
        {
            ITaskService service = new TaskService();
            service.AddTask(new TaskItem { Name = "Laporan" });
            Assert.AreEqual(1, service.GetAllTasks().Count);
        }

        [TestMethod]
        public void TestService_AddNull_Throws()
        {
            ITaskService service = new TaskService();
            Assert.Throws<ArgumentNullException>(() =>
                service.AddTask(null));
        }

        [TestMethod]
        public void TestStyleTable_LookupCorrect()
        {
            var styleTable = new Dictionary<TaskPriority, (Color Back, Color Fore)>
            {
                { TaskPriority.High, (Color.Red, Color.White) }
            };
            bool found = styleTable.TryGetValue(TaskPriority.High, out var style);
            Assert.IsTrue(found);
            Assert.AreEqual(Color.Red, style.Back);
        }
    }
}