﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Catan.Common;
using Catan.Model;

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
		private ActionCommand _StepCommand;

		public GameTableContext()
			: this(5, 5)
		{

		}

		public GameTableContext(uint size)
			: this(size, size)
		{

		}

		public GameTableContext(uint width, uint height)
		{
			_TableSize = new Size(width, height);
			GameController.Instance.Init(width, new[] { new Player("Gipsz Jakab", PlayerColor.Blue),
														new Player("Játékos 2", PlayerColor.Red) });
		}

		public Player CurrentPlayer
		{
			get { return GameController.Instance.CurrentPlayer; }
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
				return Lazy.Init(ref _SelectGameCellCommand,
								 () => new DelegateCommand<GameCellContext>(value => SelectedGameCell = value));
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
					() =>
					{
						GameController.Instance.Step();
						OnPropertyChanged("CurrentPlayer");
					}));
			}
		}
	}
}
