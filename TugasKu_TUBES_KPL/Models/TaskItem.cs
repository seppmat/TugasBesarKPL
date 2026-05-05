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
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
    }
}