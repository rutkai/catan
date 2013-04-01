using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Catan.Common;

namespace Catan.ViewModel
{
	/// <summary>
	/// GameCell nézetmodellje
	/// </summary>
	public class GameCellContext : ViewModelBase
	{
		private ActionCommand _SelectCommand;
		private Image _BackgroundImage;
		private DelegateCommand<int> _BuildRoadCommand;
		private DelegateCommand<int> _BuildTownCommand;
		private int _Value;

		public GameCellContext(GameTableContext gameTable)
		{
			GameTable = gameTable;
		}

		/// <summary>
		/// Játékcella háttérképe
		/// </summary>
		public Image BackgroundImage
		{
			get { return _BackgroundImage; }
			set
			{
				_BackgroundImage = value;
				OnPropertyChanged("BackgroundImage");
			}
		}

		/// <summary>
		/// Jétéktáblára mutató referencia
		/// </summary>
		public GameTableContext GameTable { get; protected set; }

		/// <summary>
		/// Cella értéke
		/// </summary>
		public int Value
		{
			get { return _Value; }
			set
			{
				_Value = value;
				OnPropertyChanged("Value");
			}
		}

		/// <summary>
		/// Aktuális cella kijelölése
		/// </summary>
		public ActionCommand SelectCommand
		{
			get
			{
				return Lazy.Init(ref _SelectCommand, () => new ActionCommand(_ => GameTable != null,
																			 () => GameTable.SelectGameCellCommand.Execute(this)));
			}
		}

		/// <summary>
		/// Út építésének parancsa
		/// </summary>
		public DelegateCommand<int> BuildRoadCommand
		{
			get { return Lazy.Init(ref _BuildRoadCommand, () => new DelegateCommand<int>(null, null)); }
		}

		/// <summary>
		/// Város építésének parancsa
		/// </summary>
		public DelegateCommand<int> BuildTownCommand
		{
			get { return Lazy.Init(ref _BuildTownCommand, () => new DelegateCommand<int>(null, null)); }
		}
	}
}
