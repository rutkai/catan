using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catan.Common;
using Catan.Model;

namespace Catan.ViewModel
{
	/// <summary>
	/// Kereskedelemért felelős nézetmodell osztály
	/// </summary>
	public class TradeItemContext : ViewModelBase
	{
		private DelegateCommand<int> _BuyCommand;
		public TradeItem TradeItem { get; protected set; }
		public TradeContext TradeContext { get; protected set; }

		public TradeItemContext(TradeContext context, TradeItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			TradeItem = item;
			TradeContext = context;
		}

		public int Quantity
		{
			get { return TradeItem.Quantity; }
			set
			{
				TradeItem.Quantity = value;
				OnPropertyChanged("Quantity");
			}
		}

		public Material Material
		{
			get { return TradeItem.Material; }
			set
			{
				TradeItem.Material = value;
				OnPropertyChanged("Material");
			}
		}

		public int Price
		{
			get { return TradeItem.Price; }
			set
			{
				TradeItem.Price = value;
				OnPropertyChanged("Price");
			}
		}

		public Player Player
		{
			get { return TradeItem.Player; }
			set
			{
				TradeItem.Player = value;
				OnPropertyChanged("Player");
			}
		}

		public DelegateCommand<int> BuyCommand
		{
			get
			{
				return Lazy.Init(ref _BuyCommand,
						() => new DelegateCommand<int>(
							quantity =>
							{
								Quantity -= quantity;
							}
						));
			}
		}
	}
}
