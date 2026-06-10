using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu.Tests
{
    // ============================================================
    // UNIT TEST : API/Interface & Observer Pattern (ROZI)
    // Memverifikasi TaskService via ITaskService:
    // operasi dasar, guard clause null, dan event Observer.
    // ============================================================

    [TestClass]
    public class RoziTests
    {
        private static TaskItem CreateValidTask(string name = "Laporan")
            => new TaskItem
            {
                Name     = name,
                Course   = "KPL",
                Deadline = DateTime.Today.AddDays(7)
            };

        // ---- AddTask: sukses ----

        [TestMethod]
        [Description("AddTask dengan data valid harus menambahkan satu item ke service.")]
        public void TaskService_AddValidTask_CountIsOne()
        {
            ITaskService service = new TaskService();
            service.AddTask(CreateValidTask());

            Assert.AreEqual(1, service.GetAllTasks().Count,
                "GetAllTasks seharusnya mengembalikan 1 item setelah AddTask.");
        }

        // ---- AddTask: null ----

        [TestMethod]
        [Description("AddTask dengan null harus melempar ArgumentNullException.")]
        public void TaskService_AddNull_ThrowsArgumentNull()
        {
            ITaskService service = new TaskService();

            Assert.ThrowsException<ArgumentNullException>(
                () => service.AddTask(null),
                "Seharusnya melempar ArgumentNullException jika task null.");
        }

        // ---- Observer: event TaskAdded terpicu ----

        [TestMethod]
        [Description("Event TaskAdded harus dipicu setelah AddTask berhasil.")]
        public void TaskService_AddTask_FiresTaskAddedEvent()
        {
            var service    = new TaskService();
            bool eventFired = false;
            service.TaskAdded += (s, t) => eventFired = true;

            service.AddTask(CreateValidTask());

            Assert.IsTrue(eventFired,
                "Event TaskAdded seharusnya dipicu setelah AddTask.");
        }

        // ---- Observer: event TaskDeleted terpicu ----

        [TestMethod]
        [Description("Event TaskDeleted harus dipicu setelah DeleteTask berhasil.")]
        public void TaskService_DeleteTask_FiresTaskDeletedEvent()
        {
            var service      = new TaskService();
            bool eventFired   = false;
            service.TaskDeleted += (s, i) => eventFired = true;

            service.AddTask(CreateValidTask());
            service.DeleteTask(0);

            Assert.IsTrue(eventFired,
                "Event TaskDeleted seharusnya dipicu setelah DeleteTask.");
        }

        // ---- Style Table: lookup berhasil ----

        [TestMethod]
        [Description("Lookup style table untuk prioritas High harus mengembalikan warna yang benar.")]
        public void StyleTable_HighPriority_ReturnsCorrectColors()
        {
            var styleTable = new Dictionary<TaskPriority, (Color Back, Color Fore)>
            {
                { TaskPriority.High, (Color.Red, Color.White) }
            };

            bool found = styleTable.TryGetValue(TaskPriority.High, out var style);

            Assert.IsTrue(found, "Key TaskPriority.High seharusnya ada di tabel.");
            Assert.AreEqual(Color.Red, style.Back,
                "Warna background High seharusnya merah.");
        }
    }
}
