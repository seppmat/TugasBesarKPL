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
        public TaskStatus Status { get; set; }
        public TaskState CurrentState { get; private set; }

        public TaskItem()
        {
            SetStatus(TaskStatus.NotStarted);
        }

        public TaskItem(string name, string course, DateTime deadline, TaskPriority priority, TaskStatus status)
        {
            Name = name;
            Course = course;
            Deadline = deadline;
            Priority = priority;
            SetStatus(status);
        }

        public void SetStatus(TaskStatus status)
        {
            if (!Enum.IsDefined(typeof(TaskStatus), status))
                throw new ArgumentException("Status tidak valid");

            Status = status;

            switch (status)
            {
                case TaskStatus.NotStarted:
                    CurrentState = new NotStartedState();
                    break;

                case TaskStatus.InProgress:
                    CurrentState = new InProgressState();
                    break;

                case TaskStatus.Done:
                    CurrentState = new DoneState();
                    break;

                default:
                    throw new InvalidOperationException("State mapping error");
            }

            if (CurrentState == null)
                throw new InvalidOperationException("CurrentState tidak boleh null");
        }
    }
}