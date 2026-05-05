using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.Core
{
    // ✅ TECHNIQUE: Runtime Configuration
    public static class UIScaler
    {
        private static float _scaleFactor = 1.0f;

        public static void LoadSettings()
        {
            string val = ConfigurationManager.AppSettings["UIFontScale"];
            float scale;
            // Defensive: TryParse + Clamp range 0.5x - 2.0x
            if (float.TryParse(val, out scale))
            {
                _scaleFactor = Math.Max(0.5f, Math.Min(scale, 2.0f));
            }
        }

        public static Font ScaleFont(Font baseFont)
        {
            if (baseFont == null) throw new ArgumentNullException(nameof(baseFont));
            return new Font(baseFont.FontFamily, baseFont.Size * _scaleFactor, baseFont.Style);
        }

        public static Padding ScalePadding(Padding basePadding)
        {
            int scaled = (int)(basePadding.All * _scaleFactor);
            return new Padding(scaled);
        }

        public static float CurrentScale => _scaleFactor;
    }
}