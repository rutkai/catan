using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Catan.Common;
using Catan.Model;

namespace Catan.ViewModel
{
	/// <summary>
	/// Kereskedésért felelős nézetmodell
	/// </summary>
	public class TradeContext : ViewModelBase
	{
		private DelegateCommand<object> _AddCommand;
		private DelegateCommand<Material> _DeleteCommand;
		public GameTableContext GameTableContext;
		protected Player Player { get; set; }

		public TradeContext(GameTableContext context, Player player)
		{
			GameTableContext = context;
			Player = player;
		}

		/// <summary>
		/// Elérhető termékek, amik még nincsenek meghirdetve
		/// </summary>
		public IEnumerable<Material> AvailableTradeItems
		{
			get
			{
				var array = new[]
					       {
						       Material.Wool, Material.Iron, Material.Clay, Material.Wood, Material.Wheat
					       }.Except(Player.TradeItems.Keys.ToArray()).ToArray();
				return array;
			}
		}

		/// <summary>
		/// Kereskedelmi termékek
		/// </summary>
		public IEnumerable<TradeItemContext> MyTradeItems
		{
			get { return Player.TradeItems.Values.Select(item => new TradeItemContext(this, item)).ToArray(); }
		}

		public IEnumerable<TradeItemContext> TradeItems
		{
			get
			{
				var items = new List<TradeItem>();
				foreach (var player in GameTableContext.Players)
				{
					if (player != null && player != GameTableContext.CurrentPlayer)
						items.AddRange(player.TradeItems.Values);
				}
				return items.Where(item => item.Quantity > 0)
							.Select(item => new TradeItemContext(this, item))
							.ToArray();
			}
		}

		/// <summary>
		/// Kereskedelmi termék törlése
		/// </summary>
		public DelegateCommand<Material> DeleteCommand
		{
			get
			{
				return Lazy.Init(ref _DeleteCommand,
					() => new DelegateCommand<Material>(
						material =>
						{
							if (Player.TradeItems.ContainsKey(material))
							{
								Player.TradeItems.Remove(material);
								OnPropertyChanged(() => MyTradeItems, () => AvailableTradeItems);
							}
						},
						_ => Player != null));
			}
		}

		/// <summary>
		/// Új kereskedelmi termék hozzáadása
		/// </summary>
		public DelegateCommand<object> AddCommand
		{
			get
			{
				return Lazy.Init(ref _AddCommand,
					() => new DelegateCommand<object>(
								material =>
								{
									if (Player != null)
									{
										if (!Player.AddTradeItem(1, 1, (Material)material))
											GameTableContext.WindowService.ShowMessageBox("Nincs ilyen nyersanyagod raktáron!", "Kevés a nyersanyag");
										
										Player.Materials[(Material)material]--;
									}
									OnPropertyChanged(() => AvailableTradeItems, () => MyTradeItems);
								},
								material => material is Material && AvailableTradeItems.Count() != 0
							));
			}
		}

		public override void Refresh()
		{
			base.Refresh();
			if (GameTableContext != null)
				GameTableContext.Refresh();
		}
	}
}
