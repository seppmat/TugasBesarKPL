using System;
using System.Drawing;
using System.Windows.Forms;
using TugasKu_TUBES_KPL.Forms;

namespace TugasKu_TUBES_KPL
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}