using System.Collections.Generic;

namespace Catan.Model
{
    /// <summary>
    /// Település objektum.
    /// </summary>
    public class Settlement
    {
        private Hexagon[] Fields;

        /// <summary>
        /// Tulajdonos
        /// </summary>
        public Player Owner { get; set; }

        public Hexagon m_Hexagon;

        public Hexagon[] getFields()
        {
            return Fields;
        }

        public bool IsTown
        {
            get
            {
                return this is Town;
            }
        }

        /// <summary>
        /// Konstruktor. Inicializálja az objektumot.
        /// </summary>
        /// <param name="Fields">A várossal szomszédos mezõk.</param>
        /// <param name="Owner">Tulajdonos</param>
        public Settlement(Hexagon[] Fields, Player Owner)
        {
            this.Fields = Fields;
            this.Owner = Owner;
        }

        /// <summary>
        /// Lásd õs, csak többet termel.
        /// </summary>
        /// <param name="Dice"></param>
        public virtual Dictionary<Material, int> Produce(int Dice)
        {

            return null;
        }

    }
}