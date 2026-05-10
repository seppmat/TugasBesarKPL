using System;
using System.Drawing;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL
{
	public static class UIHelper
	{
		public static void ShowSuccess(Form owner, string message)
			=> ShowToast(owner, message, ColorTranslator.FromHtml("#4ade80"));

		public static void ShowInfo(Form owner, string message)
			=> ShowToast(owner, message, ColorTranslator.FromHtml("#60a5fa"));

		public static void ShowDanger(Form owner, string message)
			=> ShowToast(owner, message, ColorTranslator.FromHtml("#f87171"));

		private static void ShowToast(Form owner, string message, Color color)
		{
			if (owner == null) throw new ArgumentNullException(nameof(owner));
			if (string.IsNullOrWhiteSpace(message)) return;
			ToastNotification.Show(message, color, owner);
		}

		public static bool ConfirmDelete(Form owner, string itemName)
		{
			if (owner == null) throw new ArgumentNullException(nameof(owner));
			return MessageBox.Show($"Yakin hapus \"{itemName}\"?", "Konfirmasi",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}
	}
}