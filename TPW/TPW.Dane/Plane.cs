namespace TPW.Dane
{
    /// <summary>
    /// Plansza na kulki
    /// </summary>
    public class Plane
    {
        private double m_width;
        private double m_height;

        /// <summary>
        /// Konstruktor planszy
        /// </summary>
        /// <param name="width">Szerokość planszy</param>
        /// <param name="height">Wysokość planszy</param>
        public Plane(double width, double height) {
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        /// Setter szerokości
        /// </summary>
        /// <param name="width">Nowa szerokość zerokość planszy</param>
        public void SetWidth(double width)
        {
            this.m_width = width;
        }

        /// <summary>
        /// Setter wysokości
        /// </summary>
        /// <param name="height">Nowa wysokość planszy</param>
        public void SetHeight(double height)
        {
            this.m_height = height;
        }

        /// <summary>
        /// Getter szerokości
        /// </summary>
        /// <returns>Zwraca szerokość planszy</returns>
        public double GetWidth()
        {
            return m_width;
        }

        /// <summary>
        /// Getter wysokości
        /// </summary>
        /// <returns>Zwraca wysokość planszy</returns>
        public double GetHeight()
        {
            return m_height;
        }
    }
}
