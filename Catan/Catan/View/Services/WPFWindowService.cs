using System.Windows;
using Catan.IFace;

namespace Catan.View.Services {
	/// <summary>
	/// Ablak bezáró service
	/// </summary>
	public class WPFWindowService : IWindowService {
		/// <summary>
		/// Ablak
		/// </summary>
		protected Window Window;

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="window">Ablak</param>
		public WPFWindowService(Window window) {
			Window = window;
		}

		/// <summary>
		/// Ablak bezárása
		/// </summary>
		public void Close() {
			Window.Close();
		}

		public void ShowMessageBox(string message, string title)
		{
			MessageBox.Show(message, title);
		}
	}
}
