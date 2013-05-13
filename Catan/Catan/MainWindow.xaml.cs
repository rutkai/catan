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

			var context = new GameTableContext(7);

			/*context.GameCells = new List<GameCellContext>()
				                    {
										new GameCellContext(context, new Hexagon(10, Material.Iron)) { Value = 1},
										new GameCellContext(context, new Hexagon(10, Material.Wheat)) { Value = 2},
										new GameCellContext(context, new Hexagon(10, Material.Wood)) { Value = 3},
										new GameCellContext(context, new Hexagon(10, Material.Wool)) { Value = 4},
				                    };*/

			context.GameCells = new List<GameCellContext>();

			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					(context.GameCells as List<GameCellContext>)
						.Add(new GameCellContext(context, 
							 new Hexagon(10, Material.Iron)) { Value = i });
				}
			}

			DataContext = context;
		}
	}
}
