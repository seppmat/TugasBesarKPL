using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.Controls
{
    public class RoundedButton : Button
    {
        public int Radius { get; set; } = 12;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = new GraphicsPath())
            {
                int diameter = Radius * 2;
                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(Width - diameter, 0, diameter, diameter, 270, 90);
                path.AddArc(Width - diameter, Height - diameter, diameter, diameter, 0, 90);
                path.AddArc(0, Height - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                this.Region = new Region(path);
            }
        }
    }
}