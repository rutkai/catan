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
		protected GameTableContext _GameTableContext;
		protected Player Player { get; set; }

		public TradeContext(GameTableContext context, Player player)
		{
			_GameTableContext = context;
			Player = player;

			if (!Player.TradeItems.ContainsKey(Material.Wool))
				Player.TradeItems.Add(Material.Wool,
									  new TradeItem
										  {
											  Material = Material.Wool,
											  Player = _GameTableContext.CurrentPlayer,
											  Price = 100,
											  Quantity = 10
										  });
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
		public List<TradeItem> MyTradeItems
		{
			get { return Player.TradeItems.Values.ToList(); }
		}

		public IEnumerable<TradeItemContext> TradeItems
		{
			get
			{
				var items = new List<TradeItem>();
				foreach (var player in _GameTableContext.Players)
				{
					if (player != null && player != _GameTableContext.CurrentPlayer)
						items.AddRange(player.TradeItems.Values);
				}
				return items.Select(item => new TradeItemContext(this, item)).ToArray();
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
								OnPropertyChanged("MyTradeItems");
								OnPropertyChanged("AvailableTradeItems");
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
									Player.AddTradeItem(1, 1, (Material)material);
									OnPropertyChanged("AvailableTradeItems");
									OnPropertyChanged("MyTradeItems");
								},
								material => material is Material && AvailableTradeItems.Count() != 0
							));
			}
		}
	}
}
