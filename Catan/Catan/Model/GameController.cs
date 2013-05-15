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
		private int WinnerScore = 10;
		private int _CurrentPlayerIndex;

		public List<Hexagon> Hexagons;

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
			Hexagons = new List<Hexagon>();
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
				throw new Exception("Legalább egy játékost meg kell adni!");

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

		/// <summary>
		/// Út megépítése
		/// </summary>
		public void BuildRoad(int position, Hexagon h)
		{
			CurrentPlayer.BuildRoad(); //dobhat exceptiont!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			h.SetRoad(CurrentPlayer, position);
		}

		/// <summary>
		/// Település építése
		/// </summary>
		public void BuildSettlement(int position, Hexagon h)
		{
			Settlement set = h.GetSettlement(position);
			set = CurrentPlayer.BuildSettlement(); //dobhat exceptiont!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			h.SetTown(set, position);
		}

		/// <summary>
		/// Kör vége
		/// </summary>
		public void EndTurn()
		{
			if (!HasWinner())
				Step();
		}

		/// <summary>
		/// Van-e nyertes
		/// </summary>
		public bool HasWinner()
		{
			bool isWinner = false;
			foreach (Player p in Players)
			{
				if (p.GetPoints() >= WinnerScore)
				{
					isWinner = true;
				}
			}
			return isWinner;
		}

		/// <summary>
		/// Új játék
		/// </summary>
		public void NewGame()
		{

			//Players = newGameWindow.getPlayers();
			int j = 0;
			/// <summary>
			/// Fisher–Yates shuffle
			/// </summary>
			for (int i = Players.Count() - 1; i > 0; i--)
			{
				j = Convert.ToInt32(new Random(i));
				Player temp = Players.ToArray()[j];
				Players.ToArray()[j] = Players.ToArray()[i];
				Players.ToArray()[i] = temp;

			}

			PlaceInitial();
		}


		public void PlaceInitial()
		{

		}

		/// <summary>
		/// Település fejlesztése
		/// </summary>
		public void UpgradeSettlement(int position, Hexagon h)
		{
			Settlement set = h.GetSettlement(position);
			CurrentPlayer.UpgradeSettlement(set);
			h.SetTown(set, position);
		}

		/// <summary>
		/// Beállítja az összes hexagon szomszédait
		/// </summary>
		public void setAllNeighbours()
		{
			foreach (Hexagon h in Hexagons)
			{
				//0. szomszéd
				if (h.Id.getRow() == 0)
				{
					h.Neighbours.Add(null);
				}
				else
				{
					h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() && x.Id.getRow() == h.Id.getRow() - 1)));
				}
				//1. szomszéd
				if ((h.Id.getRow() == 0 && h.Id.getCol() >= 3) || h.Id.getCol() == 6)
				{
					h.Neighbours.Add(null);
				}
                else if (h.Id.getCol() < 3)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow())));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow()-1)));
                }
				//2. szomszéd
				if (h.Id.getCol() == 6 || h.Id.getCol() + h.Id.getRow() == 9)
				{
					h.Neighbours.Add(null);
				}
                else if (h.Id.getCol() < 3)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow()+1)));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow())));
                }
				//3. szomszéd
				if (h.Id.getCol() + 3 == h.Id.getRow() || h.Id.getCol() + h.Id.getRow() == 9)
				{
					h.Neighbours.Add(null);
				}
				else
				{
					h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() && x.Id.getRow() == h.Id.getRow() + 1)));
				}
				//4. szomszéd
				if (h.Id.getCol() + 3 == h.Id.getRow() || h.Id.getCol() == 0)
				{
					h.Neighbours.Add(null);
				}
                else if (h.Id.getCol() <= 3)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow())));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow()+1)));
                }
				//5. szomszéd
				if ((h.Id.getCol() <= 3 && h.Id.getRow() == 0) || h.Id.getCol() == 0)
				{
					h.Neighbours.Add(null);
				}
                else if (h.Id.getCol() <= 3)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow()-1)));
                }
                else
                { 
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow())));
                }
			}
		}
	}
}
