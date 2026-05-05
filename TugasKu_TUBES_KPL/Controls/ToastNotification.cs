using System;
using System.Drawing;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.Controls
{
    public class ToastNotification : Form
    {
        public ToastNotification(string message, Color accentColor)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.White;
            this.Size = new Size(280, 70);
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Opacity = 0.95;

            var panel = new Panel { Dock = DockStyle.Fill, BackColor = accentColor };

            var lbl = new Label
            {
                Text = message,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(15, 10),
                AutoSize = true,
                MaximumSize = new Size(240, 0)
            };

            var closeBtn = new Label
            {
                Text = "×",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(250, 5),
                Cursor = Cursors.Hand,
                AutoSize = true
            };
            closeBtn.Click += (s, e) => this.Close();
            closeBtn.MouseEnter += (s, e) => closeBtn.ForeColor = Color.LightGray;
            closeBtn.MouseLeave += (s, e) => closeBtn.ForeColor = Color.White;

            panel.Controls.Add(lbl);
            panel.Controls.Add(closeBtn);
            this.Controls.Add(panel);

            var timer = new Timer { Interval = 3000 };
            timer.Tick += (s, e) => { this.Close(); timer.Stop(); };
            timer.Start();
        }

        public static void Show(string message, Color accentColor, Form owner)
        {
            var toast = new ToastNotification(message, accentColor);
            toast.Location = new Point(
                owner.Location.X + owner.Width - toast.Width - 20,
                owner.Location.Y + owner.Height - toast.Height - 20
            );
            toast.Show(owner);
        }
    }
}