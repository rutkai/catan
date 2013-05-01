using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catan.Model
{
	public class TradeItem
	{
		/// <summary>
		/// Eladni kívánt nyersanyag
		/// </summary>
		public Material Material { get; set; }

		/// <summary>
		/// Nyersanyag ára
		/// </summary>
		public int Price { get; set; }

		/// <summary>
		/// Nyersanyag tulajdonosa
		/// </summary>
		public Player Player { get; set; }

		/// <summary>
		/// Eladható nyersanyag mennyisége
		/// </summary>
		public int Quantity { get; set; }
	}
}
