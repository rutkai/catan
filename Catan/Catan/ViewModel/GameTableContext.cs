using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Catan.Common;
using Catan.Model;
using Catan.View;

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
		private Size _TableSize;
		private DelegateCommand<GameCellContext> _SelectGameCellCommand;
		private ActionCommand _StepCommand;
		private ImageSource _BackgroundImage;

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
			//_BackgroundImage = new BitmapImage(new Uri("Images//sea_field.png"));
			_TableSize = new Size(width, height);
			GameController.Instance.Init(width, new[] { new Player("Gipsz Jakab", PlayerColor.Blue),
														new Player("Játékos 2", PlayerColor.Red) });
		}

		public Player CurrentPlayer
		{
			get { return GameController.Instance.CurrentPlayer; }
		}

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
		public Size TableSize
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
						OnPropertyChanged(() => CurrentPlayer, () => TradeContext);
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

		public NewGameContext NewGameContext {
			get {
				return new NewGameContext(this, _TableSize);
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
					() => new ActionCommand(() =>
						{
							var tradeControl = new TradeControl();
							tradeControl.SetBinding(FrameworkElement.DataContextProperty,
								new Binding("TradeContext")
									{
										Source = this
									});
							tradeControl.ShowDialog();
						}));
			}
		}

		public ActionCommand ShowNewGameWindowCommand {
			get {
				return Lazy.Init(ref _ShowNewGameWindowCommand,
					() => new ActionCommand(() => {
							var newGameControl = new NewGameWindow();
							newGameControl.SetBinding(FrameworkElement.DataContextProperty,
								new Binding("NewGameContext") {
									Source = this
								});
							newGameControl.ShowDialog();
						}));
			}
		}
	}
}
