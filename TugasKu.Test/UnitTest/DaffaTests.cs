using Microsoft.VisualStudio.TestTools.UnitTesting;
using TugasKu_TUBES_KPL;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;

namespace TugasKu.Tests
{
    // ============================================================
    // UNIT TEST : State Pattern & Automata (DAFFA)
    // Memverifikasi perilaku state machine TaskItem:
    // default state, properti CanEdit/CanDelete, dan perubahan
    // instance state setelah transisi.
    // ============================================================

    [TestClass]
    public class DaffaTests
    {
        // ---- State Default ----

        [TestMethod]
        [Description("TaskItem baru harus memiliki status NotStarted dengan CanEdit dan CanDelete true.")]
        public void NewTask_DefaultState_IsNotStartedWithEditAndDeleteAllowed()
        {
            var task = new TaskItem();

            Assert.AreEqual(TaskStatus.NotStarted, task.Status,
                "Status default seharusnya NotStarted.");
            Assert.IsTrue(task.CurrentState.CanEdit,
                "NotStartedState seharusnya mengizinkan edit.");
            Assert.IsTrue(task.CurrentState.CanDelete,
                "NotStartedState seharusnya mengizinkan hapus.");
        }

        // ---- State Done ----

        [TestMethod]
        [Description("Tugas dengan status Done tidak boleh diedit, tetapi masih bisa dihapus.")]
        public void TaskSetToDone_CanEditFalse_CanDeleteTrue()
        {
            var task = new TaskItem();
            task.SetStatus(TaskStatus.Done);

            Assert.IsFalse(task.CurrentState.CanEdit,
                "DoneState seharusnya melarang edit.");
            Assert.IsTrue(task.CurrentState.CanDelete,
                "DoneState seharusnya masih mengizinkan hapus.");
        }

        // ---- Instance State setelah Transisi ----

        [TestMethod]
        [Description("Setelah SetStatus ke InProgress, CurrentState harus berupa instance InProgressState.")]
        public void SetStatus_ToInProgress_CurrentStateIsInProgressState()
        {
            var task = new TaskItem();
            task.SetStatus(TaskStatus.InProgress);

            Assert.IsInstanceOfType(task.CurrentState, typeof(InProgressState),
                "CurrentState seharusnya InProgressState setelah transisi ke InProgress.");
        }

        // ---- Label State ----

        [TestMethod]
        [Description("Label setiap state harus sesuai dengan representasi yang diharapkan.")]
        public void StateLabels_AllStates_ReturnCorrectLabels()
        {
            Assert.AreEqual("⏳ Belum",   new NotStartedState().Label);
            Assert.AreEqual("🔄 Proses",  new InProgressState().Label);
            Assert.AreEqual("✅ Selesai", new DoneState().Label);
        }
    }
}
