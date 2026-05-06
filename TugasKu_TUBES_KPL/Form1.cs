private void AddTask()
{
    using (var form = new TaskForm())
    {
        if (form.ShowDialog() == DialogResult.OK)
        {
            tasks.Add(form.Task);
            RefreshGrid();
            SaveData();
            UIHelper.ShowSuccess(this, "Tugas berhasil ditambahkan!");
        }
    }
}

private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
{
    if (e.RowIndex < 0) return;
    var filtered = GetFilteredTasks();
    var task = filtered[e.RowIndex];
    var originalIndex = tasks.IndexOf(task);

    if (e.ColumnIndex == 5)
    {
        if (!task.CurrentState.CanEdit)
        {
            MessageBox.Show($"Status \"{task.CurrentState.Label}\" tidak bisa diedit.", "Aksi Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using (var form = new TaskForm(task))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                tasks[originalIndex] = form.Task;
                RefreshGrid();
                SaveData();
                UIHelper.ShowInfo(this, "Tugas diperbarui!");
            }
        }
    }
    else if (e.ColumnIndex == 6)
    {
        if (!task.CurrentState.CanDelete)
        {
            MessageBox.Show("Tugas ini tidak bisa dihapus.", "Aksi Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (UIHelper.ConfirmDelete(this, task.Name))
        {
            tasks.RemoveAt(originalIndex);
            RefreshGrid();
            SaveData();
            UIHelper.ShowDanger(this, "Tugas dihapus");
        }
    }
}