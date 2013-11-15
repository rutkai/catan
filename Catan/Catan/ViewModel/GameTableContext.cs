using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Catan.Common;
using Catan.IFace;
using Catan.Model;
using Catan.View;

namespace Catan.ViewModel
{
    /// <summary>
    /// GameTable nézetmodellje
    /// </summary>
    public class GameTableContext : ViewModelBase
    {
        private ActionCommand _ShowTradeWindowCommand;
        private ActionCommand _ShowNewGameWindowCommand;
        private IEnumerable<GameCellContext> _GameCells;
        private GameCellContext _SelectedGameCell;
        private Size _TableSize;
        private DelegateCommand<GameCellContext> _SelectGameCellCommand;
        private ActionCommand _StepCommand;
        private ImageSource _BackgroundImage;

        public IFace.IWindowService WindowService { get; set; }

        public GameTableContext(IFace.IWindowService service)
            : this(5, 5, service)
        {
        }

        public GameTableContext(uint size, IWindowService service)
            : this(size, size, service)
        {

        }

        /// <summary>
        /// Konstruktor, ami megkapja paraméteréül a tábla méretét
        /// </summary>
        private GameTableContext(uint width, uint height, IFace.IWindowService service)
        {
            //_BackgroundImage = new BitmapImage(new Uri("Images//sea_field.png"));
            _TableSize = new Size(width, height);
            GameController.Instance.Init(width, new[] { new Player("Gipsz Jakab", PlayerColor.Blue),
														new Player("Játékos 2", PlayerColor.Red) });

            if (service == null)
                throw new ArgumentNullException("service");
            WindowService = service;
        }

        /// <summary>
        /// Aktuális játékos
        /// </summary>
        public Player CurrentPlayer
        {
            get { return GameController.Instance.CurrentPlayer; }
        }

        /// <summary>
        /// Játékosokat reprezentáló tömb
        /// </summary>
        public IEnumerable<Player> Players
        {
            get { return GameController.Instance.Players; }
        }

        /// <summary>
        /// Aktuálisan kijelölt játékcella
        /// </summary>
        public GameCellContext SelectedGameCell
        {
            get { return _SelectedGameCell; }
            set
            {
                _SelectedGameCell = value;
                OnPropertyChanged(() => SelectedGameCell);
            }
        }

        /// <summary>
        /// Játéktáblán lévő cellák
        /// </summary>
        public IEnumerable<GameCellContext> GameCells
        {
            get { return _GameCells; }
            set
            {
                _GameCells = value;
                OnPropertyChanged(() => GameCells);
            }
        }

        /// <summary>
        /// Tábla mérete
        /// </summary>
        public Size TableSize
        {
            get { return _TableSize; }
            set
            {
                _TableSize = value;
                OnPropertyChanged(() => TableSize);
            }
        }

        /// <summary>
        /// Aktuális játékcella kijelölése
        /// </summary>
        public DelegateCommand<GameCellContext> SelectGameCellCommand
        {
            get
            {
                return Lazy.Init(ref _SelectGameCellCommand,
                                 () => new DelegateCommand<GameCellContext>(
                                     value => {
                                         if (SelectedGameCell != null) {
                                             SelectedGameCell.BuildRoadMode = false;
                                             SelectedGameCell.BuildTownMode = false;
                                         }
                                         SelectedGameCell = value;
                                     }));
            }
        }

        /// <summary>
        /// Játékban való lépés
        /// </summary>
        public ActionCommand StepCommand
        {
            get
            {
                return Lazy.Init(ref _StepCommand, () => new ActionCommand(
                    () => {
                        try {
                            GameController.Instance.Step();
                        }
                        catch (Exception e) {
                            MessageBox.Show(e.Message);
                            WindowService.Close();
                        }
                        OnPropertyChanged(() => CurrentPlayer,
                                          () => TradeContext,
                                          () => DiceResult,
                                          () => Materials);
                    }));
            }
        }

        public TradeContext TradeContext
        {
            get
            {
                return new TradeContext(this, CurrentPlayer);
            }
        }

        public ImageSource BackgroundImage
        {
            get { return _BackgroundImage; }
            set
            {
                _BackgroundImage = value;
                OnPropertyChanged(() => BackgroundImage);
            }
        }

        public ActionCommand ShowTradeWindowCommand
        {
            get
            {
                return Lazy.Init(ref _ShowTradeWindowCommand,
                    () => new ActionCommand(() => {
                        var tradeControl = new TradeControl();
                        tradeControl.SetBinding(FrameworkElement.DataContextProperty,
                            new Binding("TradeContext") {
                                Source = this
                            });
                        tradeControl.ShowDialog();
                    }));
            }
        }

        public ActionCommand ShowNewGameWindowCommand
        {
            get
            {
                return Lazy.Init(ref _ShowNewGameWindowCommand,
                    () => new ActionCommand(() => {
                        var newGameControl = new NewGameWindow();
                        newGameControl.DataContext = new NewGameContext(this, _TableSize, new WPFWindowService(newGameControl));
                        newGameControl.ShowDialog();
                    }));
            }
        }

        public override void Refresh()
        {
            foreach (var cell in GameCells)
                if (cell != null)
                    cell.Refresh();

            OnPropertyChanged(() => Materials);
        }

        public int[] DiceResult
        {
            get
            {
                return new[] { GameController.Instance.Dobas1, GameController.Instance.Dobas2 };
            }
        }

        /// <summary>
        /// Visszatér az aktuális játékos nyersanyagaival
        /// </summary>
        /*public List<Tuple<Material, int>> Materials
        {
            get
            {
                return CurrentPlayer.Materials
                                    .Select(item => Tuple.Create(item.Key, item.Value))
                                    .ToList();
            }
        }*/

        public int MaximumMaterialQuantity
        {
            get { return 3; }
        }

        public List<object> Materials
        {   
            get
            {
                return CurrentPlayer.Materials
                                    .Select(item => new {
                                        Material = item.Key,
                                        Quantity = item.Value,
                                        IsFull = item.Value > MaximumMaterialQuantity
                                    })
                                    .ToList<object>();
            }
        }
    }
}
