using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Catan.Model;
using Catan.ViewModel;

namespace Catan
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var context = new GameTableContext() { TableSize = new Size(2, 2) };

			context.GameCells = new List<GameCellContext>()
				                    {
										new GameCellContext(context, new Hexagon(10, Material.Iron)) { Value = 1},
										new GameCellContext(context, new Hexagon(10, Material.Wheat)) { Value = 2},
										new GameCellContext(context, new Hexagon(10, Material.Wood)) { Value = 3},
										new GameCellContext(context, new Hexagon(10, Material.Wool)) { Value = 4},
				                    };

			DataContext = context;
		}
	}
}
