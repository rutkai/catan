using System.Collections.Generic;

namespace Catan.Model
{
    /// <summary>
    /// Város. A különbségek felüldefiniálva (pl több nyersanyagot termel).
    /// </summary>
    public class Town : Settlement
    {

        
        /// <summary>
        /// Konstruktor, meghívja az õs konstruktorát.
        /// </summary>
        /// <param name="Fields"></param>
        /// <param name="Owner"></param>
        public Town(Hexagon[] Fields, Player Owner):base(Fields,Owner)
        {
            
        }

        /// <summary>
        /// Lásd õs, csak többet termel.
        /// </summary>
        /// <param name="Dice"></param>
        public override Dictionary<Material, int> Produce(int Dice)
        {

            return null;
        }

    }
}