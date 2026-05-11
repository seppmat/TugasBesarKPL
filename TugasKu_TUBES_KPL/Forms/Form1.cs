using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using TugasKu_TUBES_KPL.Controls;

<<<<<<< HEAD

=======
>>>>>>> origin/Edward
namespace TugasKu_TUBES_KPL
{
    public partial class Form1 : Form
    {
        // ✅ GANTI List<TaskItem> dengan GenericRepository
        private GenericRepository<TaskItem> repository = new GenericRepository<TaskItem>();

        // ✅ TABLE-DRIVEN: mapping untuk filter (hindari if/switch)
        private readonly Dictionary<int, TaskStatus> statusTable = new Dictionary<int, TaskStatus>
        {
            { 1, TaskStatus.NotStarted },   // "⏳ Belum"
            { 2, TaskStatus.InProgress },   // "🔄 Proses"
            { 3, TaskStatus.Done }          // "✅ Selesai"
        };

        private readonly Dictionary<int, TaskPriority> priorityTable = new Dictionary<int, TaskPriority>
        {
            { 1, TaskPriority.High },       // " High"
            { 2, TaskPriority.Medium },     // "🟡 Medium"
            { 3, TaskPriority.Low }         // " Low"
        };

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

            // ✅ RUNTIME CONFIG: Load skala UI saat startup
            UIScaler.LoadSettings();

            // Terapkan ke kontrol utama
            var header = this.Controls.Find("headerLabel", true)[0] as Label; // Sesuaikan nama kontrol
            if (header != null) header.Font = UIScaler.ScaleFont(new Font("Segoe UI", 18, FontStyle.Bold));

            LoadData();
            RefreshGrid();
        }
        private void InitializeComponent()
        {
            // ✅ RUNTIME CONFIG: Baca tema dari App.config
            Color primary = ConfigManager.LoadColor("PrimaryColor", ColorTranslator.FromHtml("#667eea"));
            Color bg = ConfigManager.LoadColor("BackgroundColor", ColorTranslator.FromHtml("#f7f9fc"));

            Text = "Task Manager - TugasKu";
            Size = new Size(1100, 650);
            BackColor = bg;
            StartPosition = FormStartPosition.CenterScreen;

            var sidebar = new Panel { Width = 240, Height = Height, Location = new Point(0, 0), BackColor = primary };
            Controls.Add(sidebar);

            var logo = new Label { Text = "📚 TugasKu", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 30), AutoSize = true };
            sidebar.Controls.Add(logo);

            var btnAdd = new RoundedButton { Text = "+ Tambah Tugas", Size = new Size(190, 45), Location = new Point(25, 90), BackColor = Color.White, ForeColor = primary, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Radius = 10, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddTask();
            sidebar.Controls.Add(btnAdd);

            // ... (sisanya tetap sama, gunakan variabel 'primary' dan 'bg' yang sudah dimuat)
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
<<<<<<< HEAD
                    // ✅ GENERICS: Validasi dinamis sebelum simpan
                    bool isValid = GenericValidator<TaskItem>.IsValid(form.Task, t =>
                        !string.IsNullOrWhiteSpace(t.Name) &&
                        t.Deadline >= DateTime.Today);

                    if (!isValid)
                    {
                        MessageBox.Show("Data tidak valid: Nama kosong atau deadline di masa lalu.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    repository.Add(form.Task);
                    RefreshGrid();
                    SaveData();
                    UIHelper.ShowSuccess(this, "Tugas berhasil ditambahkan!");
                }
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var filtered = GetFilteredTasks();            // daftar terfilter
            var task = filtered[e.RowIndex];              // objek yang dipilih

            // Cari indeks asli di dalam list penuh (repository)
            var allTasks = repository.GetAll();
            var originalIndex = allTasks.IndexOf(task);

            if (originalIndex == -1) return; // data sudah tidak cocok

            // Column 5 = Edit
            if (e.ColumnIndex == 5)
            {
                using (var form = new TaskForm(task))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        repository.Update(originalIndex, form.Task); // ✅ update via repository
                        RefreshGrid();
                        SaveData();
                        ToastNotification.Show("✅ Tugas diperbarui!", ColorTranslator.FromHtml("#60a5fa"), this);
                    }
                }
            }
            // Column 6 = Delete
            else if (e.ColumnIndex == 6)
            {
                if (MessageBox.Show("Yakin ingin menghapus tugas ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    repository.Delete(originalIndex); // ✅ hapus via repository
                    RefreshGrid();
                    SaveData();
                    ToastNotification.Show("🗑️ Tugas dihapus", ColorTranslator.FromHtml("#f87171"), this);
                }
            }
        }

<<<<<<< HEAD
        // ✅ TECHNIQUE: Table-driven Construction (Mapping Styling)
        // Map Priority ke Warna (Background, Foreground)
        private readonly Dictionary<TaskPriority, (Color Back, Color Fore)> priorityStyleTable =
            new Dictionary<TaskPriority, (Color Back, Color Fore)>
        {
            { TaskPriority.High, (ColorTranslator.FromHtml("#fef2f2"), Color.Red) },
            { TaskPriority.Medium, (ColorTranslator.FromHtml("#fffbeb"), Color.Orange) },
            { TaskPriority.Low, (ColorTranslator.FromHtml("#f0fdf4"), Color.Green) }
        };

        public Form1()
        {
            InitializeComponent();
            // Inisialisasi Service (Dependency Injection sederhana)
            _taskService = new TaskService();
            LoadData();
            RefreshGrid();
        }

        private List<TaskItem> GetFilteredTasks()
        {
            var allTasks = repository.GetAll();
            string keyword = searchBox.Text == searchBox.Placeholder ? "" : searchBox.Text.ToLower();

            TaskStatus? targetStatus = null;
            if (filterStatus.SelectedIndex != 0)
                targetStatus = statusTable.TryGetValue(filterStatus.SelectedIndex, out var s) ? s : (TaskStatus?)null;

            TaskPriority? targetPriority = null;
            if (filterPriority.SelectedIndex != 0)
                targetPriority = priorityTable.TryGetValue(filterPriority.SelectedIndex, out var p) ? p : (TaskPriority?)null;

            return allTasks.Where(t =>
                (string.IsNullOrWhiteSpace(keyword) || t.Name.ToLower().Contains(keyword) || t.Course.ToLower().Contains(keyword)) &&
                (!targetStatus.HasValue || t.Status == targetStatus.Value) &&
                (!targetPriority.HasValue || t.Priority == targetPriority.Value)
            ).OrderBy(t => t.Deadline).ToList();
        }

        // ✅ RefreshGrid (tidak ada perubahan indeks tombol, tetap 5 dan 6)
        private void RefreshGrid()
        {
            grid.Rows.Clear();
            var filtered = GetFilteredTasks();
            emptyState.Visible = filtered.Count == 0;
            grid.Visible = filtered.Count > 0;

            foreach (var t in filtered)
                grid.Rows.Add(t.Name, t.Course, t.Deadline.ToString("dd MMM yyyy"), t.Priority.ToString(), t.Status.ToString());
        }

<<<<<<< HEAD
        private void SaveData() => File.WriteAllText(dataFile, JsonConvert.SerializeObject(tasks, Formatting.Indented));

        private void LoadData()
        {
            if (File.Exists(dataFile))
                tasks = JsonConvert.DeserializeObject<List<TaskItem>>(File.ReadAllText(dataFile)) ?? new List<TaskItem>();
        }

        private void AddTask()
        {
            using (var form = new TaskForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _taskService.AddTask(form.Task);
                    RefreshGrid();
                    SaveData();
                    UIHelper.ShowSuccess(this, "Tugas berhasil ditambahkan!");
                }
            }
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Cek kolom "Prioritas" (Index 3)
            if (grid.Columns[e.ColumnIndex].Name == "Priority")
            {
                var row = grid.Rows[e.RowIndex];
                if (row.DataBoundItem is TaskItem taskItem)
                {
                    // ✅ TABLE-DRIVEN: Ambil warna dari Dictionary Lookup
                    if (priorityStyleTable.TryGetValue(taskItem.Priority, out var style))
                    {
                        e.CellStyle.BackColor = style.Back;
                        e.CellStyle.ForeColor = style.Fore;
                    }
                }
            }
        }

        // ✅ Gunakan repository untuk penyimpanan data
        private void SaveData() => repository.Save("tasks.json");
        private void LoadData() => repository.Load("tasks.json");
    }
}
