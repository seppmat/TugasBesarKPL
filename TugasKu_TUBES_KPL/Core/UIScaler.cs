using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Singleton Pattern (static state terpusat)
    // TEKNIK KPL     : Runtime Configuration (YOSEP)
    // SECURE CODING  : (YOSEP)
    //   [SC-14] Clamping ketat pada ParseAndClamp() mencegah
    //           nilai ekstrem merusak layout (DoS via config)
    //   [SC-15] Font size result dibatasi agar tidak menghasilkan
    //           nilai negatif atau nol yang crash GDI+
    // ============================================================

    public static class UIScaler
    {
        private const float DefaultScale  = 1.0f;
        private const float MinScale      = 0.5f;
        private const float MaxScale      = 2.0f;

        // [SC-15] Batas minimum ukuran font yang aman untuk GDI+
        private const float MinFontSize   = 1.0f;

        private static float _scaleFactor = DefaultScale;

        public static float CurrentScale => _scaleFactor;

        public static void LoadSettings()
        {
            string raw   = System.Configuration.ConfigurationManager.AppSettings["UIFontScale"];
            _scaleFactor = ParseAndClamp(raw);
        }

        public static float ApplyClamping(string inputValue)
            => ParseAndClamp(inputValue);

        /// <summary>
        /// Membuat Font baru dengan ukuran yang diskalakan.
        /// [SC-15] Ukuran hasil scaling dibatasi minimal MinFontSize
        /// agar tidak menghasilkan font berukuran 0 atau negatif.
        /// </summary>
        /// <exception cref="ArgumentNullException">Jika baseFont null.</exception>
        public static Font ScaleFont(Font baseFont)
        {
            if (baseFont == null) throw new ArgumentNullException(nameof(baseFont));

            // [SC-15] Pastikan ukuran font tidak pernah di bawah batas aman
            float scaledSize = Math.Max(MinFontSize, baseFont.Size * _scaleFactor);

            return new Font(baseFont.FontFamily, scaledSize, baseFont.Style);
        }

        public static Padding ScalePadding(Padding basePadding)
        {
            int scaled = (int)(basePadding.All * _scaleFactor);
            return new Padding(Math.Max(0, scaled));
        }

        // ---- Private Helper ----

        /// <summary>
        /// [SC-14] Parse dan clamp ke [MinScale, MaxScale].
        /// Nilai di luar rentang atau bukan angka dikembalikan sebagai DefaultScale.
        /// </summary>
        private static float ParseAndClamp(string raw)
        {
            if (float.TryParse(
                    raw,
                    NumberStyles.Float | NumberStyles.AllowThousands,
                    CultureInfo.InvariantCulture,
                    out float parsed))
            {
                // [SC-14] Clamp ketat — tolak nilai ekstrem
                return Math.Max(MinScale, Math.Min(parsed, MaxScale));
            }

            return DefaultScale;
        }
    }
}
