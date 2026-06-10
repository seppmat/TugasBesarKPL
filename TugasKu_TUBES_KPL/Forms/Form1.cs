using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TugasKu_TUBES_KPL.Controls;
using TugasKu_TUBES_KPL.Core;

namespace TugasKu_TUBES_KPL.Forms
{
    public partial class Form1 : Form
    {
        private string dataFile = "tasks.json";

        private DataGridView grid;
        private PlaceholderTextBox searchBox;
        private ComboBox filterStatus, filterPriority;
        private Label emptyState;

        private readonly Color primary = ColorTranslator.FromHtml("#667eea");
        private readonly Color background = ColorTranslator.FromHtml("#f7f9fc");
        private readonly Color surface = Color.White;
        private readonly Color textPrimary = Color.FromArgb(30, 30, 30);

        // Repository
        private readonly GenericRepository<TaskItem> repository =
            new GenericRepository<TaskItem>();

        // TABLE DRIVEN
        private readonly Dictionary<int, TaskStatus> statusTable =
            new Dictionary<int, TaskStatus>
        {
            { 1, TaskStatus.NotStarted },
            { 2, TaskStatus.InProgress },
            { 3, TaskStatus.Done }
        };

        private readonly Dictionary<int, TaskPriority> priorityTable =
            new Dictionary<int, TaskPriority>
        {
            { 1, TaskPriority.High },
            { 2, TaskPriority.Medium },
            { 3, TaskPriority.Low }
        };

        // PRIORITY STYLE
        private readonly Dictionary<TaskPriority, (Color Back, Color Fore)> priorityStyleTable =
            new Dictionary<TaskPriority, (Color Back, Color Fore)>
        {
            { TaskPriority.High, (ColorTranslator.FromHtml("#fef2f2"), Color.Red) },
            { TaskPriority.Medium, (ColorTranslator.FromHtml("#fffbeb"), Color.Orange) },
            { TaskPriority.Low, (ColorTranslator.FromHtml("#f0fdf4"), Color.Green) }
        };  

        public Form1()
        {
            UIScaler.LoadSettings();

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

            // SIDEBAR
            var sidebar = new Panel
            {
                Width = 240,
                Height = Height,
                Location = new Point(0, 0),
                BackColor = primary
            };

            Controls.Add(sidebar);

            var logo = new Label
            {
                Text = "📚 TugasKu",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 30),
                AutoSize = true
            };

            sidebar.Controls.Add(logo);

            var btnAdd = new RoundedButton
            {
                Text = "+ Tambah Tugas",
                Size = new Size(190, 45),
                Location = new Point(25, 90),
                BackColor = Color.White,
                ForeColor = primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Radius = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddTask();

            sidebar.Controls.Add(btnAdd);

            // MAIN PANEL
            var mainPanel = new Panel
            {
                Location = new Point(240, 0),
                Size = new Size(Width - 240, Height),
                BackColor = background
            };

            Controls.Add(mainPanel);

            var header = new Label
            {
                Text = "Dashboard",
                Font = UIScaler.ScaleFont(new Font("Segoe UI", 18, FontStyle.Bold)),
                ForeColor = textPrimary,
                Location = new Point(30, 25),
                AutoSize = true
            };

            mainPanel.Controls.Add(header);

            // SEARCH
            searchBox = new PlaceholderTextBox
            {
                Placeholder = " Cari tugas...",
                Location = new Point(30, 70),
                Width = 280,
                Height = 35,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            searchBox.TextChanged += (s, e) => RefreshGrid();

            mainPanel.Controls.Add(searchBox);

            // FILTER PRIORITY
            filterPriority = new ComboBox
            {
                Location = new Point(330, 70),
                Width = 140,
                Height = 35,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            filterPriority.Items.AddRange(new string[]
            {
                "Semua Prioritas",
                "High",
                "Medium",
                "Low"
            });

            filterPriority.SelectedIndex = 0;
            filterPriority.SelectedIndexChanged += (s, e) => RefreshGrid();

            mainPanel.Controls.Add(filterPriority);

            // FILTER STATUS
            filterStatus = new ComboBox
            {
                Location = new Point(490, 70),
                Width = 140,
                Height = 35,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            filterStatus.Items.AddRange(new string[]
            {
                "Semua Status",
                "Belum",
                "Proses",
                "Selesai"
            });

            filterStatus.SelectedIndex = 0;
            filterStatus.SelectedIndexChanged += (s, e) => RefreshGrid();

            mainPanel.Controls.Add(filterStatus);

            // EMPTY STATE
            emptyState = new Label
            {
                Text = "Belum ada tugas!",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(150, 200),
                Size = new Size(600, 100),
                Visible = false
            };

            mainPanel.Controls.Add(emptyState);

            // GRID
            grid = new DataGridView
            {
                Location = new Point(30, 120),
                Size = new Size(mainPanel.Width - 60, mainPanel.Height - 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                BorderStyle = BorderStyle.None,
                BackgroundColor = surface,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            grid.RowTemplate.Height = 45;

            grid.Columns.Add("Name", "Tugas");
            grid.Columns.Add("Course", "Mata Kuliah");
            grid.Columns.Add("Deadline", "Deadline");
            grid.Columns.Add("Priority", "Prioritas");
            grid.Columns.Add("Status", "Status");

            grid.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Aksi",
                Text = "✏️",
                UseColumnTextForButtonValue = true,
                Width = 60
            });

            grid.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                Width = 60
            });

            grid.CellClick += Grid_CellClick;
            grid.CellFormatting += Grid_CellFormatting;

            mainPanel.Controls.Add(grid);

            FormClosing += (s, e) => SaveData();

            Resize += (s, e) =>
            {
                sidebar.Height = Height;

                mainPanel.Size = new Size(Width - 240, Height);

                grid.Size = new Size(
                    mainPanel.Width - 60,
                    mainPanel.Height - 150);
            };
        }

        private void AddTask()
        {
            using (var form = new TaskForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    bool isValid = GenericValidator<TaskItem>.IsValid(form.Task, t =>
                        !string.IsNullOrWhiteSpace(t.Name) &&
                        !string.IsNullOrWhiteSpace(t.Course) &&
                        t.Deadline >= DateTime.Today);

                    if (!isValid)
                    {
                        MessageBox.Show(
                            "Data tidak valid: Nama/Kosong atau deadline kelewat.",
                            "Validasi Gagal",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    repository.Add(form.Task);

                    RefreshGrid();
                    SaveData();

                    UIHelper.ShowSuccess(this,
                        "Tugas berhasil ditambahkan!");
                }
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var filtered = GetFilteredTasks();
            var task = filtered[e.RowIndex];

            var allTasks = repository.GetAll();

            var originalIndex = allTasks.IndexOf(task);

            if (originalIndex == -1)
                return;

            // EDIT
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
                        repository.Update(originalIndex, form.Task);
                        RefreshGrid();
                        SaveData();
                        UIHelper.ShowInfo(this, "Tugas diperbarui!");
                    }
                }
            }

            // DELETE
            else if (e.ColumnIndex == 6)
            {
                if (!task.CurrentState.CanDelete)
                {
                    MessageBox.Show(
                        "Tugas ini tidak bisa dihapus.",
                        "Aksi Ditolak",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                if (UIHelper.ConfirmDelete(this, task.Name))
                {
                    repository.Delete(originalIndex);

                    RefreshGrid();
                    SaveData();

                    UIHelper.ShowDanger(this,
                        "Tugas dihapus");
                }
            }
        }

        private List<TaskItem> GetFilteredTasks()
        {
            var allTasks = repository.GetAll();

            string keyword =
                searchBox.Text == searchBox.Placeholder
                ? ""
                : searchBox.Text.ToLower();

            TaskStatus? targetStatus = null;

            if (filterStatus.SelectedIndex != 0)
            {
                targetStatus =
                    statusTable.TryGetValue(
                        filterStatus.SelectedIndex,
                        out var s)
                    ? s
                    : (TaskStatus?)null;
            }

            TaskPriority? targetPriority = null;

            if (filterPriority.SelectedIndex != 0)
            {
                targetPriority =
                    priorityTable.TryGetValue(
                        filterPriority.SelectedIndex,
                        out var p)
                    ? p
                    : (TaskPriority?)null;
            }

            return allTasks
                .Where(t =>
                    (string.IsNullOrWhiteSpace(keyword)
                    || t.Name.ToLower().Contains(keyword)
                    || t.Course.ToLower().Contains(keyword))
                    &&
                    (!targetStatus.HasValue
                    || t.Status == targetStatus.Value)
                    &&
                    (!targetPriority.HasValue
                    || t.Priority == targetPriority.Value))
                .OrderBy(t => t.Deadline)
                .ToList();
        }

        private void RefreshGrid()
        {
            grid.Rows.Clear();

            var filtered = GetFilteredTasks();

            emptyState.Visible = filtered.Count == 0;
            grid.Visible = filtered.Count > 0;

            foreach (var t in filtered)
            {
                grid.Rows.Add(
                    t.Name,
                    t.Course,
                    t.Deadline.ToString("dd MMM yyyy"),
                    t.Priority.ToString(),
                    t.Status.ToString());
            }
        }

        private void Grid_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (grid.Columns[e.ColumnIndex].Name == "Priority")
            {
                var filtered = GetFilteredTasks();

                if (e.RowIndex >= filtered.Count)
                    return;

                var taskItem = filtered[e.RowIndex];

                if (priorityStyleTable.TryGetValue(
                    taskItem.Priority,
                    out var style))
                {
                    e.CellStyle.BackColor = style.Back;
                    e.CellStyle.ForeColor = style.Fore;
                }
            }
        }

        private void SaveData()
        {
            repository.Save(dataFile);
        }

        private void LoadData()
        {
            repository.Load(dataFile);
        }
    }
}