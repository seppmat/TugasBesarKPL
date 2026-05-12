using System;
using System.Configuration;
using System.Drawing;
<<<<<<< HEAD
=======
using System.Globalization;
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.Core
{
<<<<<<< HEAD
    // ✅ TECHNIQUE: Runtime Configuration
=======
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
    public static class UIScaler
    {
        private static float _scaleFactor = 1.0f;

        public static void LoadSettings()
        {
            string val = ConfigurationManager.AppSettings["UIFontScale"];
<<<<<<< HEAD
            float scale;
            // Defensive: TryParse + Clamp range 0.5x - 2.0x
            if (float.TryParse(val, out scale))
            {
                _scaleFactor = Math.Max(0.5f, Math.Min(scale, 2.0f));
            }
=======
            _scaleFactor = ApplyClamping(val);
        }

        public static float ApplyClamping(string inputValue)
        {
            float scale;
            if (float.TryParse(inputValue, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out scale))
            {
                return Math.Max(0.5f, Math.Min(scale, 2.0f));
            }
            return 1.0f;
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
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