using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Catan.Common;
using Catan.Model;

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

		private static Dictionary<Material, Image> _BackgroundImages =
			new Dictionary<Material, Image>
			        {
				        { Material.Clay, null },
				        { Material.Iron, null },
				        { Material.Wheat, null },
						{ Material.Wood, null },
						{ Material.Wool, null },
			        };

		public GameCellContext(GameTableContext gameTable, Hexagon hexagon = null)
		{
			GameTable = gameTable;
			_Hexagon = hexagon;
		}

		/// <summary>
		/// Hexagont reprezentáló tulajdonság
		/// </summary>
		protected Hexagon _Hexagon { get; set; }

		/// <summary>
		/// Játékcella háttérképe
		/// </summary>
		public Image BackgroundImage
		{
			get
			{
				if (_Hexagon == null)
					throw new Exception("Nem lehet null a Hexagon!");

				return _BackgroundImages[_Hexagon.Material];
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
				return Lazy.Init(ref _SelectCommand,
					() => new ActionCommand(
						_ => GameTable != null,
						() => GameTable.SelectGameCellCommand.Execute(this)));
			}
		}

		/// <summary>
		/// Út építésének parancsa
		/// </summary>
		public DelegateCommand<int> BuildRoadCommand
		{
			get
			{
				return Lazy.Init(ref _BuildRoadCommand,
					() => new DelegateCommand<int>(
						roadId =>
						{

						},
						roadId => roadId >= 0 && roadId < 6));
			}
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
