using System;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Null Object Pattern (via fallback default)
    // TEKNIK KPL     : Runtime Configuration (IVAN)
    // SECURE CODING  : (IVAN)
    //   [SC-9]  Whitelist validasi format warna hex agar hanya
    //           nilai #RRGGBB / #RGB yang diterima
    //   [SC-10] Panjang nilai config dibatasi untuk mencegah
    //           memory exhaustion dari konfigurasi yang dimanipulasi
    // ============================================================

    public static class ConfigManager
    {
        // [SC-10] Batas panjang nilai yang wajar dari App.config
        private const int MaxConfigValueLength = 256;

        // [SC-9] Regex whitelist untuk warna hex: #RGB atau #RRGGBB saja
        private static readonly Regex HexColorPattern =
            new Regex(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$", RegexOptions.Compiled);

        /// <summary>
        /// Membaca warna dari App.config.
        /// [SC-9] Hanya format hex (#RGB / #RRGGBB) yang diterima.
        /// [SC-10] Nilai di atas batas panjang langsung dikembalikan default.
        /// </summary>
        public static Color LoadColor(string key, Color defaultColor)
        {
            string value = ReadSafeValue(key);
            if (value == null) return defaultColor;

            // [SC-9] Whitelist: hanya izinkan format hex yang valid
            if (!HexColorPattern.IsMatch(value)) return defaultColor;

            try
            {
                return ColorTranslator.FromHtml(value);
            }
            catch (Exception)
            {
                return defaultColor;
            }
        }

        /// <summary>
        /// Membaca nilai enum dari App.config.
        /// Parsing bersifat case-insensitive.
        /// </summary>
        public static T LoadEnum<T>(string key, T defaultValue) where T : struct, Enum
        {
            string value = ReadSafeValue(key);
            if (value == null) return defaultValue;

            return Enum.TryParse(value, ignoreCase: true, out T result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Membaca nilai string dari App.config.
        /// </summary>
        public static string LoadString(string key, string defaultValue = "")
        {
            string value = ReadSafeValue(key);
            return value ?? defaultValue;
        }

        // ---- Private Helper ----

        /// <summary>
        /// [SC-10] Membaca nilai konfigurasi dan menolak nilai yang terlalu panjang.
        /// Mengembalikan null jika key tidak ada atau nilai terlalu panjang.
        /// </summary>
        private static string ReadSafeValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;

            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value)) return null;

            // [SC-10] Tolak nilai yang melampaui batas panjang
            if (value.Length > MaxConfigValueLength) return null;

            return value;
        }
    }
}
