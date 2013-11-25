using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

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
        public static void Save(List<Hexagon> hexagons, IEnumerable<Player> players)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode hexagonsNode = doc.CreateElement("Hexagons");
            doc.AppendChild(hexagonsNode);
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

                XmlNode settlementNode = doc.CreateElement("Settlement");
                for (var i=0; i<hex.Settlements.Length; ++i)
                {
                    if (hex.Settlements[i] != null)
                    {
                        XmlAttribute nodeNumberAttribute = doc.CreateAttribute("nodeNumber");
                        nodeNumberAttribute.Value = i.ToString();
                        settlementNode.Attributes.Append(nodeNumberAttribute);
                        XmlAttribute typeAttribute = doc.CreateAttribute("type");
                        typeAttribute.Value = hex.Settlements[i] is Town ? "Town" : "Settlement";
                        settlementNode.Attributes.Append(typeAttribute);
                        XmlAttribute ownerAttribute = doc.CreateAttribute("owner");
                        ownerAttribute.Value = hex.Settlements[i].Owner.Name;
                        settlementNode.Attributes.Append(ownerAttribute);
                    }
                }
                
                settlementsNode.AppendChild(settlementNode);

                XmlNode roadsNode = doc.CreateElement("Roads");
                hexagonNode.AppendChild(roadsNode);

                XmlNode roadNode = doc.CreateElement("Road");
                for (var i = 0; i < hex.Roads.Length; ++i)
                {
                    XmlAttribute sideAttribute = doc.CreateAttribute("side");
                    sideAttribute.Value = i.ToString();
                    roadNode.Attributes.Append(sideAttribute);
                    XmlAttribute ownerAttribute = doc.CreateAttribute("owner");
                    ownerAttribute.Value = hex.Roads[i].Player.Color.ToString();
                    roadNode.Attributes.Append(ownerAttribute);
                }
                roadsNode.AppendChild(roadNode);
            }

            XmlNode playersNode = doc.CreateElement("Players");
            doc.AppendChild(playersNode);
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

            doc.Save("catan.xml");
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {

        }
    }
}
