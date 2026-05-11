using System;
using System.Collections.Generic;

namespace TugasKu_TUBES_KPL
{
	// Implementasi Service
	public class TaskService : ITaskService
	{
		private List<TaskItem> _tasks = new List<TaskItem>();

		public List<TaskItem> GetAllTasks()
		{
			// Return copy untuk keamanan data (Defensive Programming)
			return new List<TaskItem>(_tasks);
		}

		public void AddTask(TaskItem task)
		{
			if (task == null) throw new ArgumentNullException(nameof(task), "Task tidak boleh null");
			if (string.IsNullOrWhiteSpace(task.Name)) throw new ArgumentException("Nama tugas wajib diisi");
			_tasks.Add(task);
		}

		public void UpdateTask(int index, TaskItem task)
		{
			if (index < 0 || index >= _tasks.Count)
				throw new ArgumentOutOfRangeException(nameof(index), "Index di luar jangkauan");
			if (task == null) throw new ArgumentNullException(nameof(task));
			_tasks[index] = task;
		}

		public void DeleteTask(int index)
		{
			if (index < 0 || index >= _tasks.Count)
				throw new ArgumentOutOfRangeException(nameof(index));
			_tasks.RemoveAt(index);
		}
	}
}