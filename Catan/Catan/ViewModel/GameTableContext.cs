using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Catan.View.Services;
using Catan.ViewModel.Commons;

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
        private int _TableSize;
        private DelegateCommand<GameCellContext> _SelectGameCellCommand;
        private ActionCommand _StepCommand;
        private ActionCommand _SaveCommand;
        private MessageContext _RuntimeMessage;
        private ImageSource _BackgroundImage;
        private ActionCommand _ClearMessageCommand;
        private GamePhase _GamePhase;
        private NewGameContext _NewGameContext;

        public IFace.IWindowService WindowService { get; set; }

        public NewGameContext NewGameContext
        {
            get { return _NewGameContext; }
            protected set
            {
                _NewGameContext = value;
                OnPropertyChanged(() => NewGameContext);
            }
        }

        public GamePhase GamePhase
        {
            get { return _GamePhase; }
            set
            {
                _GamePhase = value;
                switch (value) {
                    case GamePhase.Initialization:
                        ShowMessage("Játékosok sorrendjének megállapítása ...", "Játék kezdete");
                        break;
                    case GamePhase.FirstPhase:
                        ShowMessage("Első falu és a hozzátartozó út megépítése ...", "Első fázis");
                        break;
                    case GamePhase.SecondPhase:
                        ShowMessage("Első város és a hozzátartozó út megépítése ...", "Második fázis");
                        break;
                    case GamePhase.Game:
                        ShowMessage("Kezdődjön a játék!");
                        break;
                    case GamePhase.GameOver:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        public GameTableContext(IFace.IWindowService service)
            : this(5, service)
        {
        }

        public GameTableContext(uint size, IWindowService service)
        {
            //_BackgroundImage = new BitmapImage(new Uri("Images//sea_field.png"));
            _TableSize = (int)size;
            GameController.Instance.Init(size, 9, new[] { new Player("Gipsz Jakab", PlayerColor.Blue),
														new Player("Játékos 2", PlayerColor.Red) });

            if (service == null)
                throw new ArgumentNullException("service");
            WindowService = service;
            GamePhase = GamePhase.Initialization;
            NewGameContext = new NewGameContext(new[] { new Player("1. játékos", PlayerColor.Blue),
														new Player("2. játékos", PlayerColor.Red),
                                                        new Player("3. játékos", PlayerColor.Orange),
                                                        new Player("4. játékos", PlayerColor.Red), });
            InitializeGame();
        }

        private GameTableContext InitializeGame()
        {
            var players = Players.ToList();
            List<Player> newPlayers = new List<Player>();
            var random = new Random();

            while (players.Any()) {
                var index = random.Next(0, players.Count);
                newPlayers.Add(players[index]);
                players.RemoveAt(index);
            }

            GameController.Instance.Init((uint)TableSize, 10, newPlayers);

            return this;
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
        public int TableSize
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
                            ShowMessage(string.Format("Következő játékos: {0}", CurrentPlayer.Name), "Játék állása", MessageType.Error);
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

        public ActionCommand SaveCommand
        {
            get
            {
                return Lazy.Init(ref _SaveCommand, () => new ActionCommand(
                    () => {
                        GameController.Instance.Save();
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
                        /*var newGameControl = new NewGameWindow();
                        newGameControl.DataContext = new NewGameContext(this, _TableSize, new WPFWindowService(newGameControl));
                        newGameControl.ShowDialog();*/
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

        public MessageContext RuntimeMessage
        {
            get { return _RuntimeMessage; }
            protected set
            {
                _RuntimeMessage = value;
                OnPropertyChanged(() => RuntimeMessage);
            }
        }

        protected GameTableContext ShowMessage(string message, string title = "", MessageType messageType = MessageType.Information)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException("message");
            RuntimeMessage = new MessageContext(this, message, title, messageType);
            return this;
        }

        public ActionCommand ClearMessageCommand
        {
            get
            {
                if (_ClearMessageCommand == null)
                    _ClearMessageCommand = new ActionCommand(() => {
                        RuntimeMessage = null;
                    });

                return _ClearMessageCommand;
            }
        }
    }

    public enum GamePhase
    {
        Initialization, FirstPhase, SecondPhase, Game, GameOver
    }
}
