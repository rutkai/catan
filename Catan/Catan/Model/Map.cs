namespace Catan.Model
{

    /// <summary>
    /// A térkép osztály.
    /// </summary>
    public class Map
    {

        /// <summary>
        /// A térkép
        /// </summary>
        public Hexagon[][] map
        {
            //read property
            get;
            //write property
            set;
        }
        public Hexagon m_Hexagon;

        public virtual void Dispose()
        {

        }

        /// <summary>
        /// Konstruktor. Elkészíti a térképet.
        /// </summary>
        public Map()
        {

        }

        /// <summary>
        /// Visszaadja a leghosszabb út hosszát.
        /// </summary>
        public int LongestRoadLength()
        {

            return 0;
        }

        /// <summary>
        /// Visszaadja a leghosszabb út tulajdonosát
        /// </summary>
        public Player LongestRoadOwner()
        {

            return null;
        }

    }
}