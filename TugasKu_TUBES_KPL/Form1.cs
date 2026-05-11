using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TugasKu_TUBES_KPL.Controls;

namespace TugasKu_TUBES_KPL
{
    public partial class Form1 : Form
    {
        private List<TaskItem> tasks = new List<TaskItem>();
        private string dataFile = "tasks.json";
        private DataGridView grid;
        private PlaceholderTextBox searchBox;
        private ComboBox filterStatus, filterPriority;
        private Label emptyState;
        private Color primary = ColorTranslator.FromHtml("#667eea");
        private Color background = ColorTranslator.FromHtml("#f7f9fc");
        private Color surface = Color.White;
        private Color textPrimary = Color.FromArgb(30, 30, 30);

        public Form1()
        {
            InitializeComponent();
            LoadData();
            RefreshGrid();
        }

        private void InitializeComponent()
        {
            Text = "Task Manager - TugasKu";
            Size = new Size(1100, 650);
            BackColor = background;
            StartPosition = FormStartPosition.CenterScreen;

            var sidebar = new Panel { Width = 240, Height = Height, Location = new Point(0, 0), BackColor = primary };
            Controls.Add(sidebar);

            var logo = new Label { Text = "📚 TugasKu", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 30), AutoSize = true };
            sidebar.Controls.Add(logo);

            var btnAdd = new RoundedButton { Text = "+ Tambah Tugas", Size = new Size(190, 45), Location = new Point(25, 90), BackColor = Color.White, ForeColor = primary, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Radius = 10, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddTask();
            sidebar.Controls.Add(btnAdd);

            var mainPanel = new Panel { Location = new Point(240, 0), Size = new Size(Width - 240, Height), BackColor = background };
            Controls.Add(mainPanel);

            var header = new Label { Text = "Dashboard", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = textPrimary, Location = new Point(30, 25), AutoSize = true };
            mainPanel.Controls.Add(header);

            searchBox = new PlaceholderTextBox { Placeholder = " Cari tugas...", Location = new Point(30, 70), Width = 280, Height = 35, Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle };
            searchBox.TextChanged += (s, e) => RefreshGrid();
            mainPanel.Controls.Add(searchBox);

            filterPriority = new ComboBox { Location = new Point(330, 70), Width = 140, Height = 35, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            filterPriority.Items.AddRange(new string[] { "Semua Prioritas", " High", "🟡 Medium", " Low" });
            filterPriority.SelectedIndex = 0;
            filterPriority.SelectedIndexChanged += (s, e) => RefreshGrid();
            mainPanel.Controls.Add(filterPriority);

            filterStatus = new ComboBox { Location = new Point(490, 70), Width = 140, Height = 35, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            filterStatus.Items.AddRange(new string[] { "Semua Status", "⏳ Belum", "🔄 Proses", "✅ Selesai" });
            filterStatus.SelectedIndex = 0;
            filterStatus.SelectedIndexChanged += (s, e) => RefreshGrid();
            mainPanel.Controls.Add(filterStatus);

            emptyState = new Label { Text = "🎉 Belum ada tugas!\nKlik tombol di kiri untuk menambah.", Font = new Font("Segoe UI", 12), ForeColor = Color.Gray, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(150, 200), Size = new Size(600, 100), Visible = false };
            mainPanel.Controls.Add(emptyState);

            grid = new DataGridView { Location = new Point(30, 120), Size = new Size(mainPanel.Width - 60, mainPanel.Height - 150), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, BorderStyle = BorderStyle.None, BackgroundColor = surface, RowTemplate = { Height = 45 }, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            grid.Columns.Add("Name", "Tugas");
            grid.Columns.Add("Course", "Mata Kuliah");
            grid.Columns.Add("Deadline", "Deadline");
            grid.Columns.Add("Priority", "Prioritas");
            grid.Columns.Add("Status", "Status");
            grid.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Aksi", Text = "✏️", UseColumnTextForButtonValue = true, Width = 60 });
            grid.Columns.Add(new DataGridViewButtonColumn { HeaderText = "", Text = "🗑️", UseColumnTextForButtonValue = true, Width = 60 });
            grid.CellClick += Grid_CellClick;
            grid.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#f9fafb");
            grid.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#eef2ff");
            grid.DefaultCellStyle.SelectionForeColor = primary;
            mainPanel.Controls.Add(grid);

            FormClosing += (s, e) => SaveData();
            Resize += (s, e) => { sidebar.Height = Height; mainPanel.Size = new Size(Width - 240, Height); grid.Size = new Size(mainPanel.Width - 60, mainPanel.Height - 150); };
        }

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
                // Cek apakah status saat ini memperbolehkan edit
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
                // Cek apakah status saat ini memperbolehkan hapus
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

        private List<TaskItem> GetFilteredTasks()
        {
            string keyword = searchBox.Text == searchBox.Placeholder ? "" : searchBox.Text.ToLower();
            var statusFilter = filterStatus.SelectedIndex == 0 ? -1 : filterStatus.SelectedIndex - 1;
            var priorityFilter = filterPriority.SelectedIndex == 0 ? -1 : filterPriority.SelectedIndex - 1;

            return tasks.Where(t =>
                (string.IsNullOrWhiteSpace(keyword) || t.Name.ToLower().Contains(keyword) || t.Course.ToLower().Contains(keyword)) &&
                (statusFilter == -1 || (int)t.Status == statusFilter) &&
                (priorityFilter == -1 || (int)t.Priority == priorityFilter)
            ).OrderBy(t => t.Deadline).ToList();
        }

        private void RefreshGrid()
        {
            grid.Rows.Clear();
            var filtered = GetFilteredTasks();
            emptyState.Visible = filtered.Count == 0;
            grid.Visible = filtered.Count > 0;
            foreach (var t in filtered)
                grid.Rows.Add(t.Name, t.Course, t.Deadline.ToString("dd MMM yyyy"), t.Priority.ToString(), t.Status.ToString());
        }

        private void SaveData() => File.WriteAllText(dataFile, JsonConvert.SerializeObject(tasks, Formatting.Indented));

        private void LoadData()
        {
            if (File.Exists(dataFile))
                tasks = JsonConvert.DeserializeObject<List<TaskItem>>(File.ReadAllText(dataFile)) ?? new List<TaskItem>();
        }
    }
}