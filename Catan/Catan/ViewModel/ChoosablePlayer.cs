using System;
using Catan.Model;
using Catan.ViewModel.Commons;

namespace Catan.ViewModel
{
    public class ChoosablePlayer : ViewModelBase
    {
        protected bool _IsChoosen;
        public Player Player { get; protected set; }

        public bool IsChoosen
        {
            get { return _IsChoosen; }
            set
            {
                _IsChoosen = value;
                OnPropertyChanged(() => IsChoosen);
            }
        }

        public string Name
        {
            get { return Player.Name; }
            set
            {
                Player.Name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public ChoosablePlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException("player");
            Player = player;
            IsChoosen = false;
        }
    }
}