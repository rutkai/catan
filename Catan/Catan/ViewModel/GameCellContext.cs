using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
		private DelegateCommand<string> _BuildRoadCommand;
		private DelegateCommand<string> _BuildTownCommand;
		private Player _Player;

		private DelegateCommand<string> _SetTownCommmand;
		private int? _TownIndex;
		private bool _BuildTownMode;
		private bool _BuildRoadMode;

		private static readonly Dictionary<Material, ImageSource> _BackgroundImages;

		/// <summary>
		/// Képek inicializálása
		/// </summary>
		private static Dictionary<Material, ImageSource> InitImages()
		{
			var dictionary = new Dictionary<Material, ImageSource>();

			foreach (var key in new[] { Material.Clay, 
										Material.Iron, 
										Material.Wheat,
										Material.Wood, 
										Material.Wool })
			{
				var filename = "Images//" + key.ToString().ToLower() + "_field.png";
				if (File.Exists(filename))
				{
					var bmp = new BitmapImage(new Uri(filename,
															  UriKind.Relative));
					dictionary.Add(key, bmp);
				}
			}
			return dictionary;
		}

		/// <summary>
		/// Statikus konstruktor
		/// </summary>
		static GameCellContext()
		{
			_BackgroundImages = InitImages();
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		public GameCellContext(GameTableContext gameTable, Hexagon hexagon)
		{
			if (hexagon == null)
				throw new ArgumentNullException("hexagon");
			GameTable = gameTable;
			_Hexagon = hexagon;
			Player = new Player();
		}

		/// <summary>
		/// Bekapcsolja a városépítő módot
		/// </summary>
		public bool BuildTownMode
		{
			get { return _BuildTownMode; }
			set
			{
				_BuildTownMode = value;
				_BuildRoadMode = false;
				OnPropertyChanged(() => BuildTownMode);
				OnPropertyChanged(() => BuildRoadMode);
			}
		}

		/// <summary>
		/// Bekapcsolja az útépítő módot
		/// </summary>
		public bool BuildRoadMode
		{
			get { return _BuildRoadMode; }
			set
			{
				_BuildRoadMode = value;
				_BuildTownMode = false;
				OnPropertyChanged(() => BuildRoadMode);
				OnPropertyChanged(() => BuildTownMode);
			}
		}

		/// <summary>
		/// Hexagont reprezentáló tulajdonság
		/// </summary>
		protected Hexagon _Hexagon { get; set; }

		/// <summary>
		/// Játékcella háttérképe
		/// </summary>
		public ImageSource BackgroundImage
		{
			get
			{
				if (_Hexagon == null)
					throw new Exception("Nem lehet null a Hexagon!");

				return _BackgroundImages[Material];
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
			get { return _Hexagon.ProduceNumber; }
			set
			{
				_Hexagon.ProduceNumber = value;
				OnPropertyChanged(() => Value);
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
		/// Építendő város indexét tárolja
		/// </summary>
		public int? TownIndex
		{
			get { return _TownIndex; }
			set
			{
				_TownIndex = value;
				OnPropertyChanged(() => TownIndex);
			}
		}

		/// <summary>
		/// Beállítja az építendő várost
		/// </summary>
		public DelegateCommand<string> SetTownCommand
		{
			get
			{
				return Lazy.Init(ref _SetTownCommmand,
					() => new DelegateCommand<string>(
						index =>
						{
							int result;
							int.TryParse(index, out result);
							TownIndex = result;
						},
						index => !string.IsNullOrWhiteSpace(index)
				));
			}
		}

		/// <summary>
		/// Út építésének parancsa
		/// </summary>
		public DelegateCommand<string> BuildRoadCommand
		{
			get
			{
				return Lazy.Init(ref _BuildRoadCommand,
					() => new DelegateCommand<string>(
						index =>
						{
							try
							{
								int result;
								if (int.TryParse(index, out result))
								{
									GameController.Instance.BuildRoad(result, _Hexagon);
									GameTable.Refresh();
								}
							}
							catch (Exception ex)
							{
								GameTable.WindowService.ShowMessageBox(ex.Message, "Catan");
							}
						},
						index => !string.IsNullOrWhiteSpace(index)));
			}
		}

		/// <summary>
		/// Város építésének parancsa
		/// </summary>
		public DelegateCommand<string> BuildTownCommand
		{
			get
			{
				return Lazy.Init(ref _BuildTownCommand,
					() => new DelegateCommand<string>(
						index =>
						{
							try
							{
								int result;
								if (int.TryParse(index, out result))
								{
									GameController.Instance.BuildSettlement(result, _Hexagon);
									GameTable.Refresh();
								}
							}
							catch (Exception ex)
							{
								GameTable.WindowService.ShowMessageBox(ex.Message, "Catan");
							}
						},
						index => !string.IsNullOrWhiteSpace(index)));
			}
		}

		/// <summary>
		/// Cella tulajdonosa
		/// </summary>
		public Player Player
		{
			get { return _Player; }
			set
			{
				_Player = value;
				OnPropertyChanged(() => Player);
			}
		}

		/// <summary>
		/// Nyersanyag
		/// </summary>
		public Material Material
		{
			get { return _Hexagon.Material; }
			set
			{
				_Hexagon.Material = value;
				OnPropertyChanged(() => Material);
			}
		}

		/// <summary>
		/// Városokat adja vissza egy tömbben
		/// </summary>
		public Settlement[] Settlements
		{
			get { return _Hexagon.Settlements; }
			protected set
			{
				_Hexagon.Settlements = value;
				OnPropertyChanged(() => Settlements);
			}
		}

		/// <summary>
		/// Utakat adja vissza egy tömbben
		/// </summary>
		public Player[] Roads
		{
			get { return _Hexagon.Roads; }
			protected set
			{
				_Hexagon.Roads = value;
				OnPropertyChanged(() => Roads);
			}
		}

		public override void Refresh()
		{
			base.Refresh();
			OnPropertyChanged(() => Settlements);
			OnPropertyChanged(() => Roads);
		}
	}
}
