using Microsoft.VisualStudio.TestTools.UnitTesting;
using TugasKu_TUBES_KPL;

namespace TugasKu.Tests
{
	[TestClass]
	public class DaffaTests
	{
		[TestMethod]
		public void TestState_NotStarted_DefaultsCorrect()
		{
			var task = new TaskItem();
			Assert.AreEqual(TaskStatus.NotStarted, task.Status);
			Assert.IsTrue(task.CurrentState.CanEdit);
			Assert.IsTrue(task.CurrentState.CanDelete);
		}

		[TestMethod]
		public void TestState_Done_CanEditFalse()
		{
			var task = new TaskItem();
			task.SetStatus(TaskStatus.Done);
			Assert.IsFalse(task.CurrentState.CanEdit);
			Assert.IsTrue(task.CurrentState.CanDelete);
		}

		[TestMethod]
		public void TestState_Transition_UpdatesStateObject()
		{
			var task = new TaskItem();
			task.SetStatus(TaskStatus.InProgress);
			Assert.IsInstanceOfType(task.CurrentState, typeof(InProgressState));
		}
	}
}