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
using Catan.View;
using Catan.ViewModel;

namespace Catan
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            var context = new GameTableContext(7, new WPFWindowService(this));

            /*context.GameCells = new List<GameCellContext>()
                                    {
                                        new GameCellContext(context, new Hexagon(10, Material.Iron)) { Value = 1},
                                        new GameCellContext(context, new Hexagon(10, Material.Wheat)) { Value = 2},
                                        new GameCellContext(context, new Hexagon(10, Material.Wood)) { Value = 3},
                                        new GameCellContext(context, new Hexagon(10, Material.Wool)) { Value = 4},
                                    };*/

            context.GameCells = new List<GameCellContext>();

            var random = new Random();

            var materials = new[]
            {
                Material.Wood,
                Material.Wool,
                Material.Clay,
                Material.Wheat,
                Material.Iron
            };

            for (var j = 0; j < 7; ++j) {
                for (var i = 0; i < 7 - Math.Abs(3 - j); ++i) {
                    Hexagon h = new Hexagon(10, materials[random.Next(0, materials.Length)], new Hexid(j, i));
                    (context.GameCells as List<GameCellContext>)
                        .Add(new GameCellContext(context, h) { Value = random.Next(2, 13) });
                    GameController.Instance.Hexagons.Add(h);
                }
            }

            GameController.Instance.SetAllNeighbours();
            NewGameWindow newGameWindow = new NewGameWindow();
            var newGameContext = new NewGameContext(context, new Size(7, 7), new WPFWindowService(newGameWindow));
            newGameWindow.DataContext = newGameContext;
            if (newGameWindow.ShowDialog() == false)
                Close();

            DataContext = context;
        }
    }
}
