using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catan.Common;

namespace Catan.Model
{
	/// <summary>
	/// Játékvezérlő
	/// </summary>
	public class GameController
	{
		private int _CurrentPlayerIndex;

		/// <summary>
		/// Játékosokat tároló lista
		/// </summary>
		public IEnumerable<Player> Players { get; protected set; }

		public Map Map { get; protected set; }

		public Player CurrentPlayer
		{
			get { return Players.ElementAt(_CurrentPlayerIndex); }
		}

		private static GameController _Instance;

		/// <summary>
		/// Singleton osztályként használjuk a játékvezérlőt
		/// </summary>
		public static GameController Instance
		{
			get { return Lazy.Init(ref _Instance, () => new GameController()); }
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		private GameController()
		{
			Players = new List<Player>();
			_CurrentPlayerIndex = 0;
		}

		/// <summary>
		/// Játékvezérlő inicializálása
		/// </summary>
		public void Init(uint mapSize, IEnumerable<Player> players)
		{
			if (players == null)
				throw new ArgumentNullException("players");

			var _players = players as List<Player> ?? players.ToList();

			if (!_players.Any())
				throw new Exception("Legalább egy játékos meg kell adni!");

			Players = _players;
			_CurrentPlayerIndex = 0;
			Map = new Map(mapSize);
		}

		/// <summary>
		/// Következő kör elindítása
		/// </summary>
		public void Step()
		{
			if (Players != null && Players.Count() > 0)
				_CurrentPlayerIndex = (_CurrentPlayerIndex + 1) % Players.Count();
		}
	}
}
