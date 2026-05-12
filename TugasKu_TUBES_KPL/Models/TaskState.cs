using System;

namespace TugasKu_TUBES_KPL
{
	public abstract class TaskState
	{
		public abstract string Label { get; }
		public abstract bool CanEdit { get; }
		public abstract bool CanDelete { get; }
		public abstract void Transition(TaskItem task, TaskStatus next);
	}

	public class NotStartedState : TaskState
	{
		public override string Label => "? Belum";
		public override bool CanEdit => true;
		public override bool CanDelete => true;
		public override void Transition(TaskItem task, TaskStatus next)
		{
			if (next != TaskStatus.InProgress)
				throw new InvalidOperationException("Transisi hanya ke InProgress");
			task.SetStatus(next);
		}
	}

	public class InProgressState : TaskState
	{
		public override string Label => "?? Proses";
		public override bool CanEdit => true;
		public override bool CanDelete => true;
		public override void Transition(TaskItem task, TaskStatus next)
		{
			if (next != TaskStatus.Done)
				throw new InvalidOperationException("Transisi hanya ke Done");
			task.SetStatus(next);
		}
	}

	public class DoneState : TaskState
	{
		public override string Label => "? Selesai";
		public override bool CanEdit => false;
		public override bool CanDelete => true;
		public override void Transition(TaskItem task, TaskStatus next)
		{
			if (next != TaskStatus.NotStarted)
				throw new InvalidOperationException("Transisi hanya ke NotStarted");
			task.SetStatus(next);
		}
	}
}