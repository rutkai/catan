using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Catan
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		[STAThread]
		public static void Main()
		{
			App app = new App();
			MainWindow mainWindow = new MainWindow();
			app.Run(mainWindow);
		}
	}
}
