using System;
using System.Collections.Generic;

namespace Catan.Model
{
	public struct Hexid
	{
		int column;
		int row;
		public Hexid(int c, int r)
		{
			column = c;
			row = r;
		}
        public int getCol()
        {
            return column;
        }
        public int getRow()
        {
            return row;
        }
	}
	/// <summary>
	/// A térkép alkotóeleme.
	/// </summary>
	public class Hexagon : IDisposable
	{
		public List<Hexagon> Neighbours { get; protected set; }

		/// <summary>
		/// A rabló itt található?
		/// </summary>
		public bool HasRobber
		{
			//read property
			get;
			//write property
			set;
		}


		/// <summary>
		/// A mezõ nyersanyaga. Csak getter.
		/// </summary>
		public Material Material
		{
			//read property
			get;
			//write property
			set;
		}

		/// <summary>
		/// Hányas dobásra termel nyersanyagot. Csak getter.
		/// A víz produce numbere 0!
		/// </summary>
		public int ProduceNumber
		{
			//read property
			get;
			//write property
			set;
		}

		public virtual void Dispose()
		{

		}

		/// <summary>
		/// Ha van út, a gazdája van benne, egyébként null.
		/// 6 elemû tömb. Elsõ elem 1 óránál, a többi az óramutató járásának megfelelõen.
		/// </summary>
		public Player[] Roads { get; set; }
		/// <summary>
		/// Települések a csúcsokon. Ahol nincs ott null.
		/// 6 elemû tömb, elsõ eleme a 12 óránál található település, a többi az óramutató
		/// járásának megfelelõen.
		/// </summary>
		private Settlement[] Settlements;
		
		public Settlement m_Settlement;

		public Hexagon()
			: this(null)
		{

		}

		public Hexagon(IEnumerable<Hexagon> neighbours)
		{
			if (neighbours == null)
				Neighbours = new List<Hexagon>();
			else
				Neighbours = new List<Hexagon>(neighbours);
			Settlements = new Settlement[6];
			Roads = new Player[6];
		}

        public Hexid Id
        {
            get;
            set;
        }

	   

		/// <summary>
		/// Konstruktor, inicializálja a hexagont.
		/// </summary>
		/// <param name="produceNumber">Mely dobásra termel</param>
		/// <param name="material">A mezõ nyersanyaga</param>
		public Hexagon(int produceNumber, Material material, Hexid id)
			: this(null)
		{
			Material = material;
			ProduceNumber = produceNumber;
			Id = id;
		}

		/// <summary>
		/// Lekéri az adott pozíción található utat.
		/// </summary>
		/// <param name="Position"></param>
		public Player GetRoad(int Position)
		{
			if (Position >= 0 && Position <= 5)
			{
				return Roads[Position];
			}
			return null;
		}

		/// <summary>
		/// Lekéri az adott pozíción található települést.
		/// </summary>
		/// <param name="Position"></param>
		public Settlement GetSettlement(int Position)
		{
			if (Position >= 0 && Position <= 5)
			{
				return Settlements[Position];
			}
			return null;
		}

		/// 
		/// <param name="Player">Tulajdonos</param>
		/// <param name="Position">hely</param>
		public void SetRoad(Player Player, int Position)
		{

		}

		/// <summary>
		/// Beállítja az adott sarokra a megadott települést.
		/// </summary>
		/// <param name="Settlement"></param>
		/// <param name="Position"></param>
		public void SetTown(Settlement Settlement, int Position)
		{

		}

	}
}