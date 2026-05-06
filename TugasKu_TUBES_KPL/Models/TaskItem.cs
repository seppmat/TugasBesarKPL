using System;

namespace TugasKu_TUBES_KPL
{
    public enum TaskStatus { NotStarted, InProgress, Done }
    public enum TaskPriority { High, Medium, Low }

    public class TaskItem
    {
        public string Name { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public DateTime Deadline { get; set; } = DateTime.Today;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; private set; }
        public TaskState CurrentState { get; private set; }

        public TaskItem()
        {
            SetStatus(TaskStatus.NotStarted);
        }

        public void SetStatus(TaskStatus status)
        {
            if (!Enum.IsDefined(typeof(TaskStatus), status))
                throw new ArgumentException("Status tidak valid");

            Status = status;
            CurrentState = status switch
            {
                TaskStatus.NotStarted => new NotStartedState(),
                TaskStatus.InProgress => new InProgressState(),
                TaskStatus.Done => new DoneState(),
                _ => throw new InvalidOperationException("State mapping error")
            };

            if (CurrentState == null)
                throw new InvalidOperationException("CurrentState tidak boleh null");
        }
    }
}