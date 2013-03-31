using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catan.ViewModel
{
	public class GameTableContext : ViewModelBase
	{
		private GameCellContext _SelectedGameCell;

		public GameCellContext SelectedGameCell
		{
			get { return _SelectedGameCell; }
			set
			{
				_SelectedGameCell = value;
				OnPropertyChanged("SelectedGameCell");
			}
		}

		private IEnumerable<GameCellContext> _GameCells;

		public IEnumerable<GameCellContext> GameCells
		{
			get { return _GameCells; }
			set
			{
				_GameCells = value;
				OnPropertyChanged("GameCells");
			}
		}
	}
}
