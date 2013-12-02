using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Catan.Model
{
    /// <summary>
    /// Xml fájlba mentés és betöltés osztálya
    /// </summary>
    public class XmlManager
    {
        /// <summary>
        /// Játék állapotának mentése
        /// </summary>
        /// <param name="hexagons">Mezők</param>
        /// <param name="players">Játékosok</param>
        public static void Save(string filename, List<Hexagon> hexagons, IEnumerable<Player> players)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode tableNode = doc.CreateElement("Table");
            doc.AppendChild(tableNode);
            XmlNode scoreNode = doc.CreateElement("Winnerscore");
            scoreNode.InnerText = GameController.Instance.WinnerScore.ToString();
            tableNode.AppendChild(scoreNode);
            XmlNode sizeNode = doc.CreateElement("Tablesize");
            sizeNode.InnerText = GameController.Instance.size.ToString();
            tableNode.AppendChild(sizeNode);

            XmlNode playersNode = doc.CreateElement("Players");
            tableNode.AppendChild(playersNode);
            foreach (var player in players)
            {
                XmlNode playerNode = doc.CreateElement("Player");
                XmlAttribute colorAttribute = doc.CreateAttribute("color");
                colorAttribute.Value = player.Color.ToString();
                playerNode.Attributes.Append(colorAttribute);
                XmlAttribute nameAttribute = doc.CreateAttribute("name");
                nameAttribute.Value = player.Name;
                playerNode.Attributes.Append(nameAttribute);
                XmlAttribute goldAttribute = doc.CreateAttribute("gold");
                goldAttribute.Value = player.Gold.ToString();
                playerNode.Attributes.Append(goldAttribute);

                XmlNode materialsNode = doc.CreateElement("Materials");
                foreach (var material in player.Materials)
                {
                    XmlAttribute materialAttribute = doc.CreateAttribute(material.Key.ToString());
                    materialAttribute.Value = material.Value.ToString();
                    materialsNode.Attributes.Append(materialAttribute);
                }

                playerNode.AppendChild(materialsNode);
                playersNode.AppendChild(playerNode);
            }

            XmlNode hexagonsNode = doc.CreateElement("Hexagons");
            tableNode.AppendChild(hexagonsNode);
            foreach (var hex in hexagons)
            {
                XmlNode hexagonNode = doc.CreateElement("Hexagon");
                XmlAttribute colAttribute = doc.CreateAttribute("col");
                colAttribute.Value = hex.Id.getCol().ToString();
                hexagonNode.Attributes.Append(colAttribute);
                XmlAttribute rowAttribute = doc.CreateAttribute("row");
                rowAttribute.Value = hex.Id.getRow().ToString();
                hexagonNode.Attributes.Append(rowAttribute);
                XmlAttribute materialAttribute = doc.CreateAttribute("material");
                materialAttribute.Value = hex.Material.ToString();
                hexagonNode.Attributes.Append(materialAttribute);
                XmlAttribute numberAttribute = doc.CreateAttribute("number");
                numberAttribute.Value = hex.ProduceNumber.ToString();
                hexagonNode.Attributes.Append(numberAttribute);
                hexagonsNode.AppendChild(hexagonNode);

                XmlNode settlementsNode = doc.CreateElement("Settlements");
                hexagonNode.AppendChild(settlementsNode);

                for (var i = 0; i < hex.Settlements.Length; ++i)
                {
                    if (hex.Settlements[i] != null)
                    {
                        XmlNode settlementNode = doc.CreateElement("Settlement");
                        XmlAttribute nodeNumberAttribute = doc.CreateAttribute("nodeNumber");
                        nodeNumberAttribute.Value = i.ToString();
                        settlementNode.Attributes.Append(nodeNumberAttribute);
                        XmlAttribute typeAttribute = doc.CreateAttribute("type");
                        typeAttribute.Value = hex.Settlements[i] is Town ? "Town" : "Settlement";
                        settlementNode.Attributes.Append(typeAttribute);
                        XmlAttribute ownerAttribute = doc.CreateAttribute("owner");
                        ownerAttribute.Value = hex.Settlements[i].Owner.Color.ToString();
                        settlementNode.Attributes.Append(ownerAttribute);
                        settlementsNode.AppendChild(settlementNode);
                    }
                }

                XmlNode roadsNode = doc.CreateElement("Roads");
                hexagonNode.AppendChild(roadsNode);

                for (var i = 0; i < hex.Roads.Length; ++i)
                {
                    if (hex.Roads[i] != null)
                    {
                        XmlNode roadNode = doc.CreateElement("Road");
                        XmlAttribute sideAttribute = doc.CreateAttribute("side");
                        sideAttribute.Value = i.ToString();
                        roadNode.Attributes.Append(sideAttribute);
                        XmlAttribute ownerAttribute = doc.CreateAttribute("owner");
                        ownerAttribute.Value = hex.Roads[i].Player.Color.ToString();
                        roadNode.Attributes.Append(ownerAttribute);
                        roadsNode.AppendChild(roadNode);
                    }
                }
            }
            doc.Save(filename);
        }

        /// <summary>
        /// Xml visszatöltése lementett állapotból
        /// </summary>
        /// <param name="filename">betöltendő fájlnév</param>
        public static void Load(String filename)
        {
            int size, score;
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode scoreNode = doc.DocumentElement.SelectSingleNode("/Table/Winnerscore");
            score = Int32.Parse(scoreNode.InnerText);
            XmlNode sizeNode = doc.DocumentElement.SelectSingleNode("/Table/Tablesize");
            size = Int32.Parse(sizeNode.InnerText);

            XmlNodeList pnodes = doc.DocumentElement.SelectNodes("/Table/Players/Player");
            (GameController.Instance.Players as List<Player>).Clear();
            foreach (XmlNode node in pnodes)
            {
                var attr = node.Attributes;
                if (attr.Count != 0)
                {
                    Player p = new Player();
                    PlayerColor c = (PlayerColor)Enum.Parse(typeof(PlayerColor), attr["color"].Value, true);
                    p.Color = c;
                    p.Name = attr["name"].Value;
                    p.Gold = Int32.Parse(attr["gold"].Value);

                    (GameController.Instance.Players as List<Player>).Add(p);
                }
            }

            var random = new Random();
            var materials = new[]
            {
                Material.Wood,
                Material.Wool,
                Material.Clay,
                Material.Wheat,
                Material.Iron
            };

            for (var j = 0; j < size; ++j)
            {
                for (var i = 0; i < size - Math.Abs(Math.Floor(size / 2.0) - j); ++i)
                {
                    var h = new Hexagon(10, materials[random.Next(0, materials.Length)], new Hexid(j, i));
                    GameController.Instance.Hexagons.Add(h);
                }
            }

            foreach (Hexagon hex in GameController.Instance.Hexagons)
            {
                GameController.Instance.InitializeHexagonNeighbours(hex, (int)(Math.Floor(size / 2.0)), new Random());
            }
            
            XmlNodeList hnodes = doc.DocumentElement.SelectNodes("/Table/Hexagons/Hexagon");
            foreach (XmlNode node in hnodes)
            {
                Hexagon hex = null;
                Nullable<Hexid> id = null;
                var index = 0;
                
                if (node.Attributes.Count != 0)
                {
                    id = new Hexid(Int32.Parse(node.Attributes["col"].Value), Int32.Parse(node.Attributes["row"].Value));
                    index = GameController.Instance.Hexagons.FindIndex(x => x.Id.Equals(id));
                    hex = GameController.Instance.Hexagons[index];
                    string mat = node.Attributes["material"].Value;
                    int produceNumber = Int32.Parse(node.Attributes["number"].Value);
                    Material material = (Material)Enum.Parse(typeof(Material), mat, true);
                    hex.ProduceNumber = produceNumber;
                    hex.Material = material;
                }
                
                XmlNode settlNode = node.ChildNodes[0];
                if (settlNode.HasChildNodes)
                {
                    for (int i = 0; i < settlNode.ChildNodes.Count; ++i)
                    {
                        Int32 num = Int32.Parse(settlNode.ChildNodes[i].Attributes["nodeNumber"].Value);
                        string owner = settlNode.ChildNodes[i].Attributes["owner"].Value;
                        Player p = GameController.Instance.Players.First(x => x.Color.ToString() == owner);
                        hex.SetTown(new Settlement(p), num);
                    }
                }
                XmlNode roadNode = node.ChildNodes[1];
                if (roadNode.HasChildNodes)
                {
                    for (int i = 0; i < roadNode.ChildNodes.Count; ++i)
                    {
                        Int32 side = Int32.Parse(roadNode.ChildNodes[i].Attributes["side"].Value);
                        string owner = roadNode.ChildNodes[i].Attributes["owner"].Value;
                        Player p = GameController.Instance.Players.First(x => x.Color.ToString() == owner);
                        hex.SetRoad(p, side);
                    }
                }
                GameController.Instance.Hexagons[index] = hex;
            }
        }
    }
}
