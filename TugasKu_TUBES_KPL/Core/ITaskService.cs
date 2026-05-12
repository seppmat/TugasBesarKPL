using System.Collections.Generic;

namespace TugasKu_TUBES_KPL.Core
{
    // ✅ TECHNIQUE: API (Interface)
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        void AddTask(TaskItem task);
        void UpdateTask(int index, TaskItem task);
        void DeleteTask(int index);
    }
}