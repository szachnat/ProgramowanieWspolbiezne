namespace TPW.Dane
{
    /// <summary>
    /// Baza klas z Api danych
    /// </summary>
    public abstract class DaneApiBase
    {
        public abstract Ball CreateRandomBall(double radius, Pos2D minPos, Pos2D maxPos, double minVel, double maxVel);
        public abstract Plane CreatePlane(double width, double height);

        /// <summary>
        /// Getter Api
        /// </summary>
        /// <returns>Zwraca nowy obiekt DaneApi</returns>
        public static DaneApiBase GetApi()
        {
            return new DaneApi();
        }
    }
}
