using System;
using System.Collections.Generic;
namespace Catan.Model
{
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
		private Settlement[] Settlements;
		public Material m_Material;
		public PlayerColor m_PlayerColor;
		public Settlement m_Settlement;

		public Player()
			: this("", PlayerColor.Blue)
		{

		}

		public virtual void Dispose()
		{

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
			Gold = 0;
		}

		/// <summary>
		/// Hozzáadja a paramterben lévõ nyersanyagokat a játékos készletéhez.
		/// </summary>
		/// <param name="Materials"></param>
		public void AddMaterials(Dictionary<Material, int> Materials)
		{

		}

		/// <summary>
		/// Hozzáad egy új települést vagy várost.
		/// </summary>
		/// <param name="Settlement"></param>
		public void AddSettlement(Settlement Settlement)
		{

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

			return null;
		}

		/// <summary>
		/// Készít egy új települést. Hozzáadja a listájához, levonja a nyersanyagot, majd
		/// visszatér vele. Ha nincs elég nyersanyag akkor kivételt dob.
		/// </summary>
		public Settlement BuildSettlement()
		{

			return null;
		}

		/// <summary>
		/// Az adott dobásnak megfelelõen a nyersanyagok betakarításra kerülnek.
		/// </summary>
		/// <param name="Dice">Kockadobás</param>
		public void CollectMaterials(int Dice)
		{

		}

		/// <summary>
		/// Betakarítja a kezdõ nyersanyagokat (a várossal szomszédos mezõkrõl 1-1 => town.
		/// produce / 2).
		/// </summary>
		public void CollectStarterMaterials()
		{

		}

		/// <summary>
		/// A játékos pontszáma.
		/// </summary>
		public int GetPoints()
		{

			return 0;
		}

		/// <summary>
		/// Elvesz megadott számú és típusú nyersanyagot a játékos készletébõl. Negatív
		/// értéknél kivétel!
		/// </summary>
		/// <param name="Materials"></param>
		public void RemoveMaterials(Dictionary<Material, int> Materials)
		{

		}

		/// <summary>
		/// Igazra állítja a leghosszabb utat
		/// </summary>
		public void SetLongestRoad()
		{

		}

		/// <summary>
		/// Hamisra állítja a LongestRoad-ot.
		/// </summary>
		public void UnsetLongestRoad()
		{

		}

		/// <summary>
		/// Egy települést várossá alakít. Ha nincs elég nyersanyag, akkor kivételt dob. Ha
		/// a paraméter már város akkor is. :)
		/// </summary>
		/// <param name="Settlement"></param>
		public void UpgradeSettlement(Settlement Settlement)
		{

		}

	}
}