using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Catan.Common;
using Catan.Model;
using Catan.ViewModel.Commons;
using Microsoft.Win32;

namespace Catan.ViewModel
{
    public class NewGameContext : ViewModelBase
    {
        public GameTableContext GameTableContext { get; set; }
        private int _TableSize;
        private int _WinnerScore;
        private ActionCommand _LoadGameCommand;
        private ActionCommand _ExitGameCommand;
        private ActionCommand _NewGameCommand;
        private bool _IsOpened;
        public List<ChoosablePlayer> Players { get; protected set; }

        public event EventHandler Closed;

        public bool IsOpened
        {
            get { return _IsOpened; }
            set
            {
                _IsOpened = value;
                OnPropertyChanged(() => IsOpened);
            }
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
            get { return _WinnerScore; }
            set
            {
                _WinnerScore = value;
                OnPropertyChanged(() => WinnerScore);
            }
        }

        public NewGameContext(GameTableContext gameTableContext, IEnumerable<Player> players)
        {
            if (players == null)
                throw new ArgumentNullException("players");
            if (gameTableContext == null)
                throw new ArgumentNullException("gameTableContext");

            GameTableContext = gameTableContext;
            WinnerScore = 9;
            TableSize = 7;
            Open();
            Players = new List<ChoosablePlayer>(players.Select(player => new ChoosablePlayer(player)));
        }

        public void Open()
        {
            IsOpened = true;
        }

        public void Close()
        {
            IsOpened = false;
            if (Closed != null)
                Closed(this, EventArgs.Empty);
        }

        public IEnumerable<Player> GetPlayers()
        {
            return Players.Where(player => player.IsChoosen)
                          .Select(player => player.Player);
        }

        public ActionCommand NewGameCommand
        {
            get
            {
                return Lazy.Init(ref _NewGameCommand, () => new ActionCommand(
                    () => {
                        var players = GetPlayers();
                        if (!players.Any()) {
                            GameTableContext.ShowMessage("Legalább egy játékost ki kell választani!", "Figyelmeztetés", MessageType.Warning);
                            return;
                        }
                        Close();
                    }));
            }
        }

        public ActionCommand ExitGameCommand
        {
            get
            {
                return Lazy.Init(ref _ExitGameCommand, () => new ActionCommand(() => {
                    App.Current.Shutdown();
                }));
            }
        }

        public ActionCommand LoadGameCommand
        {
            get
            {
                return Lazy.Init(ref _LoadGameCommand, () => new ActionCommand(() => {
                    var dialog = new OpenFileDialog() {
                        CheckFileExists = true,
                        CheckPathExists = true,
                        Filter = "Játék fájlok (*.xml)|*.xml"
                    };
                    if (dialog.ShowDialog() == true) {
                        GameController.Instance.Load();
                        GameTableContext.ShowMessage("Játék sikeresen betöltődött ...", "Betöltés sikeres");
                        Close();
                    }
                }));
            }
        }
    }
}
