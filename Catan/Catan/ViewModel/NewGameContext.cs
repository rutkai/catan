using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using Catan.Common;
using Catan.Model;
using Catan.View;
using Catan.IFace;
using Catan.ViewModel.Converters;

namespace Catan.ViewModel
{
    public class NewGameContext : ViewModelBase
    {
        /// <summary>
        /// Új gombra tett akció
        /// </summary>
        private ActionCommand _NewGame;

        /// <summary>
        /// A kiválasztott játékosok listája
        /// </summary>
        private List<string> _SelectedPlayers;

        /// <summary>
        /// A játéktábla nézetmodellje
        /// </summary>
        public GameTableContext GameTableContext;

        /// <summary>
        /// A nézethez hozzáférést nyújtú interfész
        /// </summary>
        protected IWindowService WindowService;

        /// <summary>
        /// Tábla méretei
        /// </summary>
        protected int _TableSize;

        /// <summary>
        /// Konstruktor
        /// Inicializálja az objektumot
        /// </summary>
        /// <param name="context">Játéktábla nézetmodellje</param>
        /// <param name="size">Játéktábla mérete</param>
        /// <param name="service">Nézethez hozzáférést nyújtó interfész</param>
        public NewGameContext(GameTableContext context, int size, IWindowService service)
        {
            GameTableContext = context;
            TableSize = size;
            WindowService = service;
            _SelectedPlayers = new List<string>();

            Players = new Dictionary<PlayerColor, Player>();
            EnabledPlayers = new Dictionary<PlayerColor, bool>();
            foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor))) {
                Players.Add(color, new Player(color.ToString(), color));
                EnabledPlayers.Add(color, false);
            }
        }

        /// <summary>
        /// Játékosok tömbje
        /// </summary>
        public Dictionary<PlayerColor, Player> Players
        {
            get;
            private set;
        }

        /// <summary>
        /// Színek aktív státuszát tartalmazó tömb
        /// </summary>
        public Dictionary<PlayerColor, bool> EnabledPlayers
        {
            get;
            private set;
        }

        /// <summary>
        /// A játékosok színeit visszaadó getter
        /// </summary>
        public IEnumerable<PlayerColor> PlayerColors
        {
            get
            {
                return new[] {
                    PlayerColor.Blue, PlayerColor.Red, PlayerColor.Green, PlayerColor.Orange
				};
            }
        }

        /// <summary>
        /// Aktív színek állítását figyelő metódus
        /// </summary>
        public string SelectedPlayerValues
        {
            get
            {
                return String.Join(",", _SelectedPlayers.ToArray());
            }
            set
            {
                _SelectedPlayers = new List<string>(value.Split(','));
                foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor))) {
                    EnabledPlayers[color] = _SelectedPlayers.Contains(color.ToString());
                }
                OnPropertyChanged(() => EnabledPlayers);
            }
        }

        /// <summary>
        /// Új játék gombra kattintáskor lefutó akció
        /// </summary>
        public ActionCommand NewGameCommand
        {
            get
            {
                return Lazy.Init(ref _NewGame,
                    () => new ActionCommand(() => {
                        List<Player> players = new List<Player>();
                        foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor))) {
                            if (EnabledPlayers[color]) {
                                players.Add(Players[color]);
                            }
                        }

                        if (players.Count <= 1) {
                            MessageBox.Show("Legalább két játékos kell!");
                        }
                        else if (players.Any(x => x.Name.Length < 2)) {
                            MessageBox.Show("A játékosok nevének legalább kettő karakter hosszúnak kell lenni!");
                        }
                        else {
                            GameController.Instance.Init((uint)TableSize, players);
                            CloseCommand();
                        }
                    }));
            }
        }

        /// <summary>
        /// Az ablak bezárását végző metódus
        /// </summary>
        protected void CloseCommand()
        {
            WindowService.Close();
        }

        public int TableSize
        {
            get { return _TableSize; }
            set
            {
                _TableSize = value;
                OnPropertyChanged(() => TableSize);
            }
        }

        public int WinnerScore
        {
            get
            {
                return GameController.Instance.WinnerScore;
            }
            set
            {
                GameController.Instance.WinnerScore = value;
                OnPropertyChanged(() => WinnerScore);
            }
        }
    }
}
