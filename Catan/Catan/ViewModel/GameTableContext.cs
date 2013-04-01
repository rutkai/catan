using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Catan.ViewModel
{
	/// <summary>
	/// GameTable nézetmodellje
	/// </summary>
	public class GameTableContext : ViewModelBase
	{
		private IEnumerable<GameCellContext> _GameCells;
		private GameCellContext _SelectedGameCell;
		private Size _TableSize;
		private DelegateCommand<GameCellContext> _SelectGameCellCommand;

		public GameTableContext()
			: this(5, 5)
		{

		}

		public GameTableContext(int width, int height)
		{
			_TableSize = new Size(width, height);
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
				OnPropertyChanged("SelectedGameCell");
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
				OnPropertyChanged("GameCells");
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
				OnPropertyChanged("TableSize");
			}
		}

		/// <summary>
		/// Aktuális játékcella kijelölése
		/// </summary>
		public DelegateCommand<GameCellContext> SelectGameCellCommand
		{
			get
			{
				if (_SelectGameCellCommand == null)
					_SelectGameCellCommand = new DelegateCommand<GameCellContext>(value => SelectedGameCell = value);
				return _SelectGameCellCommand;
			}
		}

	}
}
