using System;
using System.Configuration;
using System.Drawing;

namespace TugasKu_TUBES_KPL
{
    // Teknik Runtime Configuration
    public static class ConfigManager
    {
        // Ambil warna dari config, fallback ke default jika kosong/error
        public static Color LoadColor(string key, Color defaultColor)
        {
            string val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val)) return defaultColor;

            try
            {
                return ColorTranslator.FromHtml(val);
            }
            catch
            {
                return defaultColor;
            }
        }

        // Ambil nilai Enum dari config
        public static T LoadEnum<T>(string key, T defaultValue) where T : struct, Enum
        {
            string val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val)) return defaultValue;

            T result;
            if (Enum.TryParse(val, true, out result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}