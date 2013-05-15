using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catan.IFace {
	/// <summary>
	/// Ablek bezáráshoz használt interfész
	/// </summary>
	public interface IWindowService {
		/// <summary>
		/// Bezárás
		/// </summary>
		void Close();
		/// <summary>
		/// MessageBox feldobása
		/// </summary>
		/// <param name="message">Üzenet</param>
		/// <param name="title">Cím</param>
		void ShowMessageBox(string message, string title);
	}
}
