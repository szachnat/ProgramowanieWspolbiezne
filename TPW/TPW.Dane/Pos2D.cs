using System.Net.NetworkInformation;

namespace TPW.Dane
{
    /// <summary>
    /// Obiekt zawier¹cy wspó³rzêdne na p³aszczy¿nie dwuwymiarowej
    /// </summary>
    public struct Pos2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Konstruktor pozycji
        /// </summary>
        /// <param name="x">Wspó³rzêdna x</param>
        /// <param name="y">Wspó³rzêdna y</param>
        public Pos2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Mno¿enie
        /// </summary>
        /// <param name="pos">Pozycja(czynnik)</param>
        /// <param name="mul">Drugi czynnik mno¿enia - int</param>
        public static Pos2D operator *(Pos2D pos, int mul)
        {
            return new Pos2D { X = pos.X * mul, Y = pos.Y * mul };
        }
        /// <summary>
        /// Mno¿enie
        /// </summary>
        /// <param name="pos">Pozycja(czynnik)</param>
        /// <param name="mul">Drugi czynnik mno¿enia - float</param>
        public static Pos2D operator *(Pos2D pos, float mul)
        {
            return new Pos2D { X = pos.X * mul, Y = pos.Y * mul };
        }
        /// <summary>
        /// Mno¿enie
        /// </summary>
        /// <param name="pos">Pozycja(czynnik)</param>
        /// <param name="mul">Drugi czynnik mno¿enia - double</param>
        public static Pos2D operator *(Pos2D pos, double mul)
        {
            return new Pos2D { X = pos.X * mul, Y = pos.Y * mul };
        }

        /// <summary>
        /// Dodawanie
        /// </summary>
        /// <param name="pos1">Pierwszy sk³adnik</param>
        /// <param name="pos2">Drugi sk³adnik</param>
        public static Pos2D operator +(Pos2D pos1, Pos2D pos2)
        {
            return new Pos2D { X = pos1.X + pos2.X, Y = pos1.Y + pos2.Y };
        }

        /// <summary>
        /// odejmowanie
        /// </summary>
        /// <param name="pos1">Odjemna</param>
        /// <param name="pos2">Odjemnik</param>
        public static Pos2D operator -(Pos2D pos1, Pos2D pos2)
        {
            return new Pos2D { X = pos1.X - pos2.X, Y = pos1.Y - pos2.Y };
        }

        public static bool operator ==(Pos2D pos1, Pos2D pos2)
        {
            return pos1.X == pos2.X && pos1.Y == pos2.Y;
        }
        public static bool operator !=(Pos2D pos1, Pos2D pos2)
        {
            return pos1.X != pos2.X || pos1.Y != pos2.Y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Pos2D) return false;
            return this == (Pos2D)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }


    }

}
