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
            XmlNode hexagonsNode = doc.CreateElement("Hexagons");
            tableNode.AppendChild(hexagonsNode);
            foreach (var hex in hexagons) {
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

                for (var i = 0; i < hex.Settlements.Length; ++i) {
                    if (hex.Settlements[i] != null) {
                        XmlNode settlementNode = doc.CreateElement("Settlement");
                        XmlAttribute nodeNumberAttribute = doc.CreateAttribute("nodeNumber");
                        nodeNumberAttribute.Value = i.ToString();
                        settlementNode.Attributes.Append(nodeNumberAttribute);
                        XmlAttribute typeAttribute = doc.CreateAttribute("type");
                        typeAttribute.Value = hex.Settlements[i] is Town ? "Town" : "Settlement";
                        settlementNode.Attributes.Append(typeAttribute);
                        XmlAttribute ownerAttribute = doc.CreateAttribute("owner");
                        ownerAttribute.Value = hex.Settlements[i].Owner.Name;
                        settlementNode.Attributes.Append(ownerAttribute);
                        settlementsNode.AppendChild(settlementNode);
                    }
                }

                XmlNode roadsNode = doc.CreateElement("Roads");
                hexagonNode.AppendChild(roadsNode);

                for (var i = 0; i < hex.Roads.Length; ++i) {
                    if (hex.Roads[i] != null) {
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

            XmlNode playersNode = doc.CreateElement("Players");
            tableNode.AppendChild(playersNode);
            foreach (var player in players) {
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
                foreach (var material in player.Materials) {
                    XmlAttribute materialAttribute = doc.CreateAttribute(material.Key.ToString());
                    materialAttribute.Value = material.Value.ToString();
                    materialsNode.Attributes.Append(materialAttribute);
                }

                playerNode.AppendChild(materialsNode);
                playersNode.AppendChild(playerNode);
            }
            doc.Save(filename);
        }

        /// <summary>
        /// Xml visszatöltése lementett állapotból
        /// </summary>
        /// <param name="filename">betöltendő fájlnév</param>
        public static void Load(String filename)
        {
            int size;
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNodeList hnodes = doc.DocumentElement.SelectNodes("/Table/Hexagons/Hexagon");
            size = Convert.ToInt32(hnodes[hnodes.Count - 1].Attributes["col"].Value) + 1;
            foreach (XmlNode node in hnodes)
            {
                //Console.Write("<" + node.Name);
                if (node.Attributes.Count != 0)
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        //Console.Write(" " + attr.Name + "='" + attr.Value + "' ");
                    }
                }
                XmlNode settlNode = node.ChildNodes[0];
                if (settlNode.HasChildNodes)
                {
                    //Console.Write("     <" + f.ChildNodes[0].Name);
                    foreach (XmlAttribute attr in settlNode.ChildNodes[0].Attributes)
                    {
                        //Console.Write(" " + attr.Name + "='" + attr.Value + "' ");
                    }
                }
                XmlNode roadNode = node.ChildNodes[1];
                if (roadNode.HasChildNodes)
                {
                    //Console.Write("     <" + g.ChildNodes[0].Name);
                    foreach (XmlAttribute attr in roadNode.ChildNodes[0].Attributes)
                    {
                        //Console.Write(" " + attr.Name + "='" + attr.Value + "' ");
                    }
                }
            }

            XmlNodeList pnodes = doc.DocumentElement.SelectNodes("/Table/Players/Player");
            foreach (XmlNode node in pnodes)
            {
                if (node.Attributes.Count != 0)
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        
                    }
                }   
            }
        }
    }
}
