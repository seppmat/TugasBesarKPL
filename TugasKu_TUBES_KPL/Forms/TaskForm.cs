using System;
using System.Drawing;
using System.Windows.Forms;
using TugasKu_TUBES_KPL.Controls;
namespace TugasKu_TUBES_KPL
{
    public class TaskForm : Form
    {
        public TaskItem Task { get; private set; } = new TaskItem();
        private PlaceholderTextBox txtName;
        private PlaceholderTextBox txtCourse;
        private DateTimePicker date;
        private ComboBox cbPriority;
        private ComboBox cbStatus;
        private RoundedButton btnSave;
        private Button btnCancel;

        public TaskForm(TaskItem existing = null)
        {
            Text = existing == null ? "Tambah Tugas" : "Edit Tugas";
            Size = new Size(500, 580);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ColorTranslator.FromHtml("#f7f9fc");
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            InitUI();
            if (existing != null) PopulateForm(existing);
        }

        private void PopulateForm(TaskItem item)
        {
            txtName.Text = item.Name;
            txtCourse.Text = item.Course;
            date.Value = item.Deadline;
            cbPriority.SelectedIndex = (int)item.Priority;
            cbStatus.SelectedIndex = (int)item.Status;
        }

        private void InitUI()
        {
            int marginX = 40, width = 400, y = 40, gap = 30;
            void AddField(string label, Control ctrl, ref int yPos)
            {
                Controls.Add(new Label { Text = label, Location = new Point(marginX, yPos), Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true });
                ctrl.Location = new Point(marginX, yPos + 25);
                ctrl.Width = width; ctrl.Height = 40; ctrl.Font = new Font("Segoe UI", 10);
                Controls.Add(ctrl);
                yPos += 25 + ctrl.Height + gap;
            }

            txtName = new PlaceholderTextBox { Placeholder = "Nama tugas...", BorderStyle = BorderStyle.FixedSingle };
            AddField("Nama Tugas *", txtName, ref y);

            txtCourse = new PlaceholderTextBox { Placeholder = "Mata kuliah...", BorderStyle = BorderStyle.FixedSingle };
            AddField("Mata Kuliah *", txtCourse, ref y);

            date = new DateTimePicker { Format = DateTimePickerFormat.Short };
            AddField("Deadline *", date, ref y);

            cbPriority = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cbPriority.Items.AddRange(new string[] { "🔴 High", "🟡 Medium", "🟢 Low" });
            cbPriority.SelectedIndex = 1;
            AddField("Prioritas", cbPriority, ref y);

            cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cbStatus.Items.AddRange(new string[] { " Belum", "🔄 Proses", "✅ Selesai" });
            cbStatus.SelectedIndex = 0;
            AddField("Status", cbStatus, ref y);

            btnSave = new RoundedButton { Text = "Simpan", BackColor = ColorTranslator.FromHtml("#667eea"), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Radius = 10, Location = new Point(marginX, y + 20), Width = width, Height = 45, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += SaveTask;
            Controls.Add(btnSave);

            btnCancel = new Button { Text = "Batal", BackColor = Color.Transparent, ForeColor = Color.Gray, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Location = new Point(marginX, y + 75), Width = width, Height = 40, Font = new Font("Segoe UI", 10) };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => Close();
            Controls.Add(btnCancel);
        }

        private void SaveTask(object sender, EventArgs e)
        {
            string name = txtName.Text == txtName.Placeholder ? "" : txtName.Text.Trim();
            string course = txtCourse.Text == txtCourse.Placeholder ? "" : txtCourse.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(course))
            {
                MessageBox.Show("Nama tugas dan mata kuliah wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Task = new TaskItem { Name = name, Course = course, Deadline = date.Value.Date, Priority = (TaskPriority)cbPriority.SelectedIndex, Status = (TaskStatus)cbStatus.SelectedIndex };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}