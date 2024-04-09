using TPW.Dane;

namespace TPW.Logika
{
    public class CollisionManager
    {
        public static double TimeOfCollisionWithLine(Pos2D startObjPos, Pos2D objVel, Pos2D linePoint1, Pos2D linePoint2)
        {
            double xDiff = linePoint2.X - linePoint1.X;
            double time;
            if (xDiff != 0)
            {
                double a = (linePoint2.Y - linePoint1.Y) / xDiff;
                double b = linePoint1.Y - a * linePoint1.X;

                time = (objVel.Y - a * objVel.X) != 0 ? (a * startObjPos.X + b - startObjPos.Y) / (objVel.Y - a * objVel.X) : double.PositiveInfinity;
            }
            else
            {
                time = objVel.X != 0 ? (linePoint1.X - startObjPos.X) / objVel.X : double.PositiveInfinity;
            }
            if (time != double.PositiveInfinity)
            {
                //punkt kolizji
                Pos2D col = objVel * time + startObjPos;

                // minimalny i maksymalna wartoœæ x
                double minX = linePoint1.X > linePoint2.X ? linePoint2.X : linePoint1.X;
                double maxX = linePoint1.X > linePoint2.X ? linePoint1.X : linePoint2.X;

                // minimalna i maksymalna wartoœæ y
                double minY = linePoint1.Y > linePoint2.Y ? linePoint2.Y : linePoint1.Y;
                double maxY = linePoint1.Y > linePoint2.Y ? linePoint1.Y : linePoint2.Y;

                if (col.X < minX || col.X > maxX || col.Y < minY || col.Y > maxY)
                {
                    time = double.PositiveInfinity;
                }
            }

            return time;
        }
    }
}
