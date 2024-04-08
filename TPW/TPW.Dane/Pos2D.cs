using System.Net.NetworkInformation;

namespace TPW.Dane
{
    public struct Pos2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Pos2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Pos2D operator *(Pos2D pos, int mul)
        {
            return new Pos2D { X = pos.X * mul, Y = pos.Y * mul };
        }
        public static Pos2D operator *(Pos2D pos, float mul)
        {
            return new Pos2D { X = pos.X * mul, Y = pos.Y * mul };
        }
        public static Pos2D operator *(Pos2D pos, double mul)
        {
            return new Pos2D { X = pos.X * mul, Y = pos.Y * mul };
        }

        public static Pos2D operator +(Pos2D pos1, Pos2D pos2)
        {
            return new Pos2D { X = pos1.X + pos2.X, Y = pos1.Y + pos2.Y };
        }

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
