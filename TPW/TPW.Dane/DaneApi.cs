using System;

namespace TPW.Dane
{
    /// <summary>
    /// Api danych
    /// </summary>
    internal class DaneApi : DaneApiBase
    {
        /// <summary>
        /// Funkcja generująca kulkę z losowymi parametrami z podanych zakresów
        /// </summary>
        /// <param name="radius">Promień kulki</param>
        ///  <param name="minPos">Najmniejsza pozycja kulki</param>
        ///  <param name="maxPos">Największa pozycja kulki</param>
        ///  <param name="minVel">Minimalna prędkość kulki</param>
        ///  <param name="maxVel">Maksymalna prędkość kulki</param>
        ///  <returns>Kulkę o parametrach losowych z podanych zakresów</returns>
        public override Ball CreateRandomBall(double radius, Pos2D minPos, Pos2D maxPos, double minVel, double maxVel)
        {
            Random rnd = new();

            double minX = minPos.X + radius;
            double maxX = maxPos.X - radius;

            double minY = minPos.Y + radius;
            double maxY = maxPos.Y - radius;

            double temp;
            if (minX > maxX)
            {
                temp = minX;
                minX = maxX;
                maxX = temp;
            }

            if (minY > maxY)
            {
                temp = minY;
                minY = maxY;
                maxY = temp;
            }

            Pos2D pos = new(rnd.NextDouble() * (maxX - minX) + minX, rnd.NextDouble() * (maxY - minY) + minY);
            Pos2D vel = new(rnd.NextDouble() * (maxVel - minVel) + minVel, rnd.NextDouble() * (maxVel - minVel) + minVel);

            return new Ball(rnd.NextInt64(), radius, pos, vel);
        }

        /// <summary>
        /// Funkcja generująca planszę 
        /// </summary>
        /// <param name="width">Szerokość planszy</param>
        ///  <param name="height">Wysokość planszy</param>
        ///  <returns>Planszę</returns>
        public override Plane CreatePlane(double width, double height)
        {
            return new Plane(width, height);
        }
    }
}
