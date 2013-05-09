using System;
using System.Collections.Generic;
namespace Catan.Model
{
    public class NotEnoughMaterialsException : System.Exception
    {
        public NotEnoughMaterialsException() : base() { }
        public NotEnoughMaterialsException(string message) : base(message) { }
        public NotEnoughMaterialsException(string message, System.Exception inner) : base(message, inner) { }

        protected NotEnoughMaterialsException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

    public class ItsAlreadyTownException : System.Exception
    {
        public ItsAlreadyTownException() : base() { }
        public ItsAlreadyTownException(string message) : base(message) { }
        public ItsAlreadyTownException(string message, System.Exception inner) : base(message, inner) { }

        protected ItsAlreadyTownException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

	/// <summary>
	/// Játékos osztály. Tartalmazza a játékosok beállításait.
	/// </summary>
	public class Player
	{
		public Dictionary<Material, TradeItem> TradeItems { get; protected set; }

		public int Gold { get; set; }

		/// <summary>
		/// Csak getter.
		/// </summary>
		public PlayerColor Color
		{
			//read property
			get;
			//write property
			set;
		}
		/// <summary>
		/// Õvé a leghosszabb út?
		/// </summary>
		private bool LongestRoad;
		/// <summary>
		/// A játékos nyersanyagkészlete. Csak getter.
		/// </summary>
		public Dictionary<Material, int> Materials
		{
			//read property
			get;
			//write property
			set;
		}
		/// <summary>
		/// Csak getter.
		/// </summary>
		public string Name
		{
			//read property
			get;
			//write property
			set;
		}
		/// <summary>
		/// Települések listája.
		/// </summary>
		private List<Settlement> Settlements;
		public Material m_Material;
		public PlayerColor m_PlayerColor;
		public Settlement m_Settlement;

        public Player(string name, PlayerColor pc)
		{
            Name = name;
            m_PlayerColor = pc;
            Materials = new Dictionary<Material, int>();
            Settlements = new List<Settlement>();
		}

		/// <summary>
		/// Konstruktor. Inicializálja az objektumot.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Color"></param>
		public Player(string name, PlayerColor color)
		{
			Name = name;
			Color = color;
			TradeItems = new Dictionary<Material, TradeItem>();
			Gold = 1000;
		}

		/// <summary>
		/// Hozzáadja a paramterben lévõ nyersanyagokat a játékos készletéhez.
		/// </summary>
		/// <param name="Materials"></param>
		public void AddMaterials(Dictionary<Material, int> mats)
		{
            foreach (KeyValuePair<Material, int> ms in mats)
            {
                if (Materials.ContainsKey(ms.Key))
                {
                    Materials[ms.Key] += ms.Value;
                }
                else
                {
                    Materials.Add(ms.Key, ms.Value);
                }
            }
		}

		/// <summary>
		/// Hozzáad egy új települést vagy várost.
		/// </summary>
		/// <param name="Settlement"></param>
		public void AddSettlement(Settlement Settlement)
		{
            Settlements.Add(Settlement);
		}

		public void AddTradeItem(int price, int quantity, Material material)
		{
			TradeItems.Add(material, new TradeItem { Material = material, Player = this, Price = price, Quantity = quantity });
		}

		/// <summary>
		/// Kereskedés más játékosokkal
		/// </summary>
		public void TradeWithPlayer(Player player, int quantity, Material material)
		{
			if (player == null)
				throw new ArgumentNullException("player");

			if (player == this)
				throw new Exception("Saját magával nem kereskedhet a játékos!");

			TradeItem item;
			if (TradeItems.TryGetValue(material, out item))
			{
				item.Quantity -= quantity;
				Gold += quantity * item.Price;
				player.Gold -= quantity * item.Price;
			}
		}

		/// <summary>
		/// Levonja a szükséges nyersanyagokat majd visszatér önmagával. Ha nincs elég
		/// nyersanyag kivételt dob.
		/// </summary>
		public Player BuildRoad()
		{
            if (Materials[Material.Wood] > 0 && Materials[Material.Clay] > 0)
            {
                Materials[Material.Wood]--;
                Materials[Material.Clay]--;
            }
            else
            {
                throw new NotEnoughMaterialsException();
            }
            return this;
		}

		/// <summary>
		/// Készít egy új települést. Hozzáadja a listájához, levonja a nyersanyagot, majd
		/// visszatér vele. Ha nincs elég nyersanyag akkor kivételt dob.
		/// </summary>
		public Settlement BuildSettlement()
		{
            Settlement set = new Settlement();
            if (Materials[Material.Wood] > 0 && Materials[Material.Clay] > 0 && Materials[Material.Wheat] > 0 && Materials[Material.Wool] > 0)
            {
                Materials[Material.Wood]--;
                Materials[Material.Clay]--;
                Materials[Material.Wheat]--;
                Materials[Material.Wool]--;
                Settlements.Add(set);
            }
            else
            {
                throw new NotEnoughMaterialsException();
            }
            return set;
		}

		/// <summary>
		/// Az adott dobásnak megfelelõen a nyersanyagok betakarításra kerülnek.
		/// </summary>
		/// <param name="Dice">Kockadobás</param>
		public void CollectMaterials(int Dice)
		{
            foreach (Settlement s in Settlements)
            {
                Dictionary<Material, int> settlemetMaterial = s.Produce(Dice);
                foreach (KeyValuePair<Material, int> ms in settlemetMaterial)
                {
                    if (Materials.ContainsKey(ms.Key))
                    {
                        Materials[ms.Key] += ms.Value;
                    }
                    else
                    {
                        Materials.Add(ms.Key, ms.Value);
                    }
                }
            }
		}

		/// <summary>
		/// Betakarítja a kezdõ nyersanyagokat (a várossal szomszédos mezõkrõl 1-1 => town.
		/// produce / 2).
		/// </summary>
		public void CollectStarterMaterials()
		{
            for (int i = 2; i <= 12; i++)
            {
                CollectMaterials(i);
            }
		}

		/// <summary>
		/// A játékos pontszáma.
		/// </summary>
		public int GetPoints()
		{
            int point = 0;
            foreach (Settlement s in Settlements)
            {
                if (s.GetType().ToString() == "Settlement")
                {
                    point++;
                }
                else
                {
                    point += 2;
                }
            }
            if (LongestRoad)
            {
                point += 2;
            }
            return point;
		}

		/// <summary>
		/// Elvesz megadott számú és típusú nyersanyagot a játékos készletébõl. Negatív
		/// értéknél kivétel!
		/// </summary>
		/// <param name="Materials"></param>
		public void RemoveMaterials(Dictionary<Material, int> mats)
		{
            foreach (KeyValuePair<Material, int> ms in mats)
            {
                if (Materials.ContainsKey(ms.Key) && Materials[ms.Key] >= ms.Value)
                {
                    Materials[ms.Key] -= ms.Value;
                }
                else
                {
                    throw new NotEnoughMaterialsException();
                }
            }
		}

		/// <summary>
		/// Igazra állítja a leghosszabb utat
		/// </summary>
		public void SetLongestRoad()
		{
            LongestRoad = true;
		}

		/// <summary>
		/// Hamisra állítja a LongestRoad-ot.
		/// </summary>
		public void UnsetLongestRoad()
		{
            LongestRoad = false;
		}

		/// <summary>
		/// Egy települést várossá alakít. Ha nincs elég nyersanyag, akkor kivételt dob. Ha
		/// a paraméter már város akkor is. :)
		/// </summary>
		/// <param name="Settlement"></param>
		public void UpgradeSettlement(Settlement settlement)
		{
            if (settlement.GetType().ToString() != "Town")
            {
                if (Materials[Material.Iron] >= 3 && Materials[Material.Wheat] >= 2)
                {
                    Materials[Material.Wheat] -= 3;
                    Materials[Material.Iron] -= 2;
                    Town town = new Town(settlement.getFields(), this);
                    Settlements.Remove(settlement);
                    Settlements.Add(town);
                }
                else
                {
                    throw new NotEnoughMaterialsException();
                }
            }
            else
            {
                throw new ItsAlreadyTownException();
            }
		}

	}
}