using System;
using System.Drawing;
using System.Windows.Forms;
using TugasKu_TUBES_KPL.Controls;

namespace TugasKu_TUBES_KPL.Core
{
    // ============================================================
    // DESIGN PATTERN : Facade Pattern
    // TEKNIK KPL     : Code Reuse Helper (DAFFA)
    // SECURE CODING  : (DAFFA)
    //   [SC-16] Panjang pesan toast dibatasi agar tidak meluap
    //           dari area notifikasi (UI overflow prevention)
    //   [SC-17] itemName pada ConfirmDelete di-escape agar karakter
    //           khusus tidak merusak tampilan dialog
    // ============================================================

    public static class UIHelper
    {
        // [SC-16] Batas panjang pesan yang ditampilkan di toast
        private const int MaxToastMessageLength = 200;

        // ---- Toast Notifications ----

        public static void ShowSuccess(Form owner, string message)
            => ShowToast(owner, message, ColorTranslator.FromHtml("#4ade80"));

        public static void ShowInfo(Form owner, string message)
            => ShowToast(owner, message, ColorTranslator.FromHtml("#60a5fa"));

        public static void ShowDanger(Form owner, string message)
            => ShowToast(owner, message, ColorTranslator.FromHtml("#f87171"));

        // ---- Dialogs ----

        /// <summary>
        /// Menampilkan dialog konfirmasi penghapusan.
        /// [SC-17] itemName dipotong dan karakter newline dihapus
        /// agar tidak bisa memanipulasi tampilan dialog box.
        /// </summary>
        /// <exception cref="ArgumentNullException">Jika owner null.</exception>
        public static bool ConfirmDelete(Form owner, string itemName)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            // [SC-17] Sanitasi nama item sebelum ditampilkan di dialog
            string safeItemName = SanitizeDisplayText(itemName ?? "item ini", 100);

            DialogResult result = MessageBox.Show(
                $"Yakin ingin menghapus \"{safeItemName}\"?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }

        // ---- Private ----

        private static void ShowToast(Form owner, string message, Color accentColor)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (string.IsNullOrWhiteSpace(message)) return;

            // [SC-16] Potong pesan yang terlalu panjang sebelum ditampilkan
            string safeMessage = SanitizeDisplayText(message, MaxToastMessageLength);

            ToastNotification.Show(safeMessage, accentColor, owner);
        }

        /// <summary>
        /// [SC-16][SC-17] Membersihkan teks untuk ditampilkan di UI:
        /// hapus newline/tab tersembunyi dan batasi panjang.
        /// </summary>
        private static string SanitizeDisplayText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text ?? string.Empty;

            // Ganti newline/tab dengan spasi agar tidak memanipulasi layout dialog
            string cleaned = text
                .Replace("\r\n", " ")
                .Replace("\n",   " ")
                .Replace("\r",   " ")
                .Replace("\t",   " ");

            return cleaned.Length > maxLength
                ? cleaned.Substring(0, maxLength) + "..."
                : cleaned;
        }
    }
}
