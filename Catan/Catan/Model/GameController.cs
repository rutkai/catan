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

        public int Dobas1 { get; set; }
        public int Dobas2 { get; set; }
        public int WinnerScore = 9;
        private int _CurrentPlayerIndex;
        private Player Winner = null;
        private int size = 7;
        private int cellNumber = 0;

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
        public void Init(uint mapSize, int winnersc, IEnumerable<Player> players)
        {
            if (players == null)
                throw new ArgumentNullException("players");

            var _players = players as List<Player> ?? players.ToList();

            if (!_players.Any())
                throw new Exception("Legalább egy játékost meg kell adni!");

            Players = _players;
            _CurrentPlayerIndex = 0;
            WinnerScore = winnersc;
            size = (int)mapSize;
            for (int i = size - (int)Math.Floor(size / 2.0); i < size; i++)
            {
                cellNumber += i * 2;
            }
            cellNumber += size;
            Map = new Map(mapSize);
        }

        /// <summary>
        /// Következő kör elindítása
        /// </summary>
        public void Step()
        {
            if (HasWinner())
            {
                Player winner = Players.First(x => (x.GetPoints() >= WinnerScore));
                throw new Exception("Játék vége! Nyert: " + winner.Name);
            }
            else
            {
                if (Players != null && Players.Count() > 0)
                    _CurrentPlayerIndex = (_CurrentPlayerIndex + 1) % Players.Count();
                Random dobas = new Random();
                Dobas1 = dobas.Next(1, 7);
                Dobas2 = dobas.Next(1, 7);
                int result = Dobas1 + Dobas2;
                foreach (Hexagon h in Hexagons)
                {
                    h.Produce(result);
                }
            }
        }

        /// <summary>
        /// Út megépítése
        /// </summary>
        public void BuildRoad(int position, Hexagon h)
        {
            var set1 = h.Settlements[position];
            var set2 = h.Settlements[(position + 5) % 6];
            var road1 = h.Roads[(position +1) % 6];
            var road2 = h.Roads[(position + 5) % 6];
            if ((set1 != null && set1.Owner == CurrentPlayer || set2 != null && set2.Owner == CurrentPlayer) || (road1 != null && road1.Color == CurrentPlayer.Color) || (road2 != null && road2.Color == CurrentPlayer.Color))
            {
                CurrentPlayer.BuildRoad(); //dobhat exceptiont!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                h.SetRoad(CurrentPlayer, position);
            }
            else
            {
                throw new Exception("Csak saját település vagy saját út mellé lehet utat építeni!");
            }
        }

        /// <summary>
        /// Település építése
        /// </summary>
        public void BuildSettlement(int position, Hexagon h)
        {
            Settlement set1 = h.Settlements[(position + 1) % 6];
            Settlement set2 = h.Settlements[(position + 5) % 6];
            Settlement set3 = null;
            if(h.Neighbours[(position + 1) % 6] != null)
               set3 = h.Neighbours[(position + 1) % 6].GetSettlement((position + 5) % 6);

            if (set1 != null || set2 != null || set3 != null)
            {
                throw new Exception("Szomszédos csúcsokra nem építhető település!");
            }
            else
            {
                    Settlement set = h.GetSettlement(position);
                    set = CurrentPlayer.BuildSettlement(); //dobhat exceptiont!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    h.SetTown(set, position);
            }
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
                    Winner = p;
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
        public void SetAllNeighbours()
        {
            int half = (int)(Math.Floor(size/2.0));
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
                if ((h.Id.getRow() == 0 && h.Id.getCol() >= half) || h.Id.getCol() == size-1)
                {
                    h.Neighbours.Add(null);
                }
                else if (h.Id.getCol() < half)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow())));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow() - 1)));
                }
                //2. szomszéd
                if (h.Id.getCol() == size-1 || h.Id.getCol() + h.Id.getRow() == size-1+half)
                {
                    h.Neighbours.Add(null);
                }
                else if (h.Id.getCol() < half)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow() + 1)));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() + 1 && x.Id.getRow() == h.Id.getRow())));
                }
                //3. szomszéd
                if (h.Id.getCol() + half == h.Id.getRow() || h.Id.getCol() + h.Id.getRow() == size-1+half)
                {
                    h.Neighbours.Add(null);
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() && x.Id.getRow() == h.Id.getRow() + 1)));
                }
                //4. szomszéd
                if (h.Id.getCol() + half == h.Id.getRow() || h.Id.getCol() == 0)
                {
                    h.Neighbours.Add(null);
                }
                else if (h.Id.getCol() <= half)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow())));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow() + 1)));
                }
                //5. szomszéd
                if ((h.Id.getCol() <= half && h.Id.getRow() == 0) || h.Id.getCol() == 0)
                {
                    h.Neighbours.Add(null);
                }
                else if (h.Id.getCol() <= half)
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow() - 1)));
                }
                else
                {
                    h.Neighbours.Add(Hexagons.Find(x => (x.Id.getCol() == h.Id.getCol() - 1 && x.Id.getRow() == h.Id.getRow())));
                }

                var random = new Random();

                while(h.Neighbours.Exists(x => (x != null && x.ProduceNumber == h.ProduceNumber)))
                {
                    h.ProduceNumber = random.Next(2, 13);
                }
                int shouldBeNum = (cellNumber*5)/36+1;
                int numberOfSixs = Hexagons.FindAll(x => x.ProduceNumber == 6).Count();
                int numberOfEights = Hexagons.FindAll(x => x.ProduceNumber == 8).Count();
                if (numberOfSixs < shouldBeNum)
                {
                    for (int i = 0; i < shouldBeNum - numberOfSixs; i++ )
                    {
                        Hexagons.Find(x => (x.ProduceNumber!= 6 && !x.Neighbours.Exists(y => y!=null && y.ProduceNumber==6))).ProduceNumber=6;
                    }
                }
                if (numberOfEights < shouldBeNum)
                {
                    for (int i = 0; i < shouldBeNum - numberOfSixs; i++)
                    {
                        Hexagons.Find(x => (x.ProduceNumber != 8 && !x.Neighbours.Exists(y => y!=null && y.ProduceNumber == 8))).ProduceNumber = 8;
                    }
                }
                foreach (Material m in (Material[])Enum.GetValues(typeof(Material)))
                {
                    if (!Hexagons.Exists(x => x.Material == m))
                    {
                        Hexagons[random.Next(0, cellNumber)].Material = m;
                    }
                }
            }
        }
    }
}
