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
        private Player Owner;
        public Hexagon m_Hexagon;

        public Settlement()
        {

        }

        public Hexagon[] getFields()
        {
            return Fields;
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
        /// Az adott dobásnak megfelelõen visszatér a termelt nyersanyagokkal.
        /// </summary>
        /// <param name="Dice">Kockadobás eredménye mindkét kockával</param>
        public Dictionary<Material, int> Produce(int Dice)
        {

            return null;
        }

    }
}