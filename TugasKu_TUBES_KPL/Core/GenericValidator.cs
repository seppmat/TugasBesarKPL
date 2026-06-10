using System;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Strategy Pattern (via Delegate/Predicate)
    // TEKNIK KPL     : Parameterization / Generics (YOSEP)
    // SECURE CODING  : (YOSEP)
    //   [SC-13] Exception dari fungsi condition ditangkap agar
    //           validator tidak bocorkan detail internal ke caller
    // ============================================================

    public static class GenericValidator<T> where T : class
    {
        /// <summary>
        /// Mengevaluasi apakah item memenuhi kondisi validasi yang diberikan.
        /// [SC-13] Exception dari dalam condition ditangkap dan diperlakukan
        /// sebagai hasil validasi false, mencegah kebocoran stack trace.
        /// </summary>
        /// <exception cref="ArgumentNullException">Jika item atau condition null.</exception>
        public static bool IsValid(T item, Func<T, bool> condition)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item yang divalidasi tidak boleh null.");
            if (condition == null)
                throw new ArgumentNullException(nameof(condition), "Fungsi validasi tidak boleh null.");

            try
            {
                return condition(item);
            }
            catch (Exception)
            {
                // [SC-13] Anggap validasi gagal jika kondisi melempar exception tak terduga.
                // Tidak meneruskan exception ke atas agar detail internal tidak bocor.
                return false;
            }
        }

        /// <summary>
        /// Guard clause generik: memastikan item tidak null sebelum diproses.
        /// </summary>
        /// <exception cref="ArgumentNullException">Jika item null.</exception>
        public static T EnsureNotNull(T item, string paramName)
        {
            if (item == null)
                throw new ArgumentNullException(
                    paramName,
                    $"Parameter '{paramName}' tidak boleh bernilai null.");

            return item;
        }
    }
}
