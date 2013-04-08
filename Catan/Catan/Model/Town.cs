using System.Collections.Generic;

namespace Catan.Model
{
    /// <summary>
    /// Város. A különbségek felüldefiniálva (pl több nyersanyagot termel).
    /// </summary>
    public class Town : Settlement
    {

        public Town()
        {

        }

        ~Town()
        {

        }

        public override void Dispose()
        {

        }

        /// <summary>
        /// Konstruktor, meghívja az õs konstruktorát.
        /// </summary>
        /// <param name="Fields"></param>
        /// <param name="Owner"></param>
        public Town(Hexagon[] Fields, Player Owner)
        {

        }

        /// <summary>
        /// Lásd õs, csak többet termel.
        /// </summary>
        /// <param name="Dice"></param>
        public Dictionary<Material, int> Produce(int Dice)
        {

            return null;
        }

    }
}