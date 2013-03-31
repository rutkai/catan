using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catan.ViewModel
{
	public class GameCellContext : ViewModelBase
	{
		public GameTableContext GameTable { get; protected set; }

		private int _Value;

		public int Value
		{
			get { return _Value; }
			set
			{
				_Value = value;
				OnPropertyChanged("Value");
			}
		}

		public GameCellContext(GameTableContext gameTable)
		{
			GameTable = gameTable;
		}

		private ActionCommand _SelectCommand;

		public ActionCommand SelectCommand
		{
			get
			{
				if (_SelectCommand == null)
				{
					_SelectCommand = new ActionCommand(() => GameTable.SelectedGameCell = this);
				}
				return _SelectCommand;
			}
		}
	}
}
