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
		private Settlement[] _Settlements;

		public Settlement[] Settlements
		{
			get { return _Settlements; }
			set { _Settlements = value; }
		}

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
			Neighbours = new List<Hexagon>();
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
		/// <param name="position"></param>
		public Settlement GetSettlement(int position)
		{
			if (position >= 0 && position <= 5)
			{
				return Settlements[position];
			}
			return null;
		}

		/// 
		/// <param name="player">Tulajdonos</param>
		/// <param name="position">hely</param>
		public void SetRoad(Player player, int position)
		{
            if (position >= 0 && position <= 5)
            {
                Roads[position] = player;
                var hexagon = Neighbours[position];
                if (hexagon!= null)
                    Neighbours[position].Roads[(position + 3) % 6]=player;
            }
			
		}

		/// <summary>
		/// Beállítja az adott sarokra a megadott települést.
		/// </summary>
		/// <param name="settlement"></param>
		/// <param name="position"></param>
		public void SetTown(Settlement settlement, int position)
		{
            if (position >= 0 && position <= 5)
            {
                Settlements[position] = settlement;
                var hexagon1 = Neighbours[position];
                if (hexagon1 != null)
                    hexagon1.Settlements[(position + 2) % 6]=settlement;
                var hexagon2 = Neighbours[(position + 1) % 6];
                if (hexagon2 != null)
                {
                    hexagon2.Settlements[(position + 4) % 6] = settlement;
                }
            }
			
		}

        /// <summary>
        /// Az adott dobásnak megfelelõen visszatér a termelt nyersanyagokkal.
        /// </summary>
        /// <param name="Dice">Kockadobás eredménye mindkét kockával</param>
        public void Produce(int Dice)
        {
            Dictionary<Material, int> materials = new Dictionary<Material,int>();
            
            if (this.ProduceNumber == Dice)
            {
                foreach (Settlement sett in Settlements)
                {
                    if (sett != null)
                    {
                        if (sett.GetType().ToString() == "Settlement")
                        {
                            materials.Add(this.Material, 1);
                        }
                        else
                        {
                            materials.Add(this.Material, 2);
                        }
                        sett.Owner.AddMaterials(materials);
                    }
                    materials.Clear();
                }
            }
        }
	}
}