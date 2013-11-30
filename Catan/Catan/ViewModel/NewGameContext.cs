using System;
using System.Collections.Generic;
using System.Linq;
using Catan.Common;
using Catan.Model;
using Catan.ViewModel.Commons;
using Microsoft.Win32;

namespace Catan.ViewModel
{
    public class NewGameContext : ViewModelBase
    {
        private int _TableSize;
        private int _WinnerScore;
        private ActionCommand _LoadGameCommand;
        private ActionCommand _ExitGameCommand;
        public List<ChoosablePlayer> Players { get; protected set; }

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

        public NewGameContext(IEnumerable<Player> players)
        {
            if (players == null) throw new ArgumentNullException("players");
            Players = new List<ChoosablePlayer>(players.Select(player => new ChoosablePlayer(player)));
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
                    var dialog = new OpenFileDialog();
                    if (dialog.ShowDialog() == true) {
                        GameController.Instance.Load();
                    }
                }));
            }
        }
    }
}
