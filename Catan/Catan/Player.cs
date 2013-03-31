using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Catan
{
    public enum Material {Wood, Clay, Iron, Wheat, Wool};

    //class Settlement { }

    class Player
    {
        private Boolean LongestRoad = false;
        private Settlement[] Settlements;
        public Color PlayerColor;
        public Dictionary<Material, int> Materials;
        public String Name;

	    public Player(String name, Color pc)
	    {
            Name = name;
            PlayerColor = pc;
            Materials = new Dictionary<Material, int>();
            Settlements = new Settlement[2];
	    }

        public void AddMaterials(Dictionary<Material, int> mat)
        {
            foreach (KeyValuePair<Material, int> ms in mat)
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

        public void AddSettlements(Settlement set)
        { 
        
        }

        public Player BuildRoad()
        {
            Player p = new Player("", Colors.Blue);
            return p;
        }

        public Settlement BuildSettlement()
        {
            Settlement set = new Settlement();
            return set;
        }

        public void CollectMaterials(int num)
        {
        
        }

        public void CollectStarterMaterials()
        {

        }

        public int GetPoints()
        { 
            //minden körben újra számoljuk vagy nem kellene egy adattag?
            return 0;
        }

        public void RemoveMaterials(Dictionary<Material, int> mat)
        {

        }

        public void SetLongestRoad()
        {
            LongestRoad = true;
        }

        public void UnsetLongestRoad()
        {
            LongestRoad = false;
        }

        public void UpgradeSettlement(Settlement set)
        { 
        
        }
    }
}
