using TPW.Dane;

namespace TPW.Logika
{
    /// <summary>
    /// Klasa implementuj¹ca zderzenia
    /// </summary>
    public class CollisionManager
    {
        /// <summary>
        /// Okreœla czas zderzenia z nieuchom¹ lini¹
        /// </summary>
        /// <param name="objPos">Opcz¹tkowa pozycja obiektu</param>
        /// <param name="objVel">Prêdkoœæ obiektu</param>
        /// <param name="linePoint1">Pocz¹tkowy punkt linii</param>
        /// <param name="linePoint2">Koñcowy punkt linii</param>
        /// <returns>Zwraca czas, po którym nast¹pi kolizja</returns>
        public static double TimeOfCollisionWithStaticLine(Pos2D objPos, Pos2D objVel, Pos2D linePoint1, Pos2D linePoint2)
        {
            return TimeOfCollisionWithMovingLine(objPos, objVel, linePoint1, linePoint2, Pos2D.Zero);
        }

        /// <summary>
        /// Okreœla czas zderzenia z ruchom¹ lini¹
        /// </summary>
        /// <param name="objPos">Opcz¹tkowa pozycja obiektu</param>
        /// <param name="objVel">Prêdkoœæ obiektu</param>
        /// <param name="linePoint1">Pocz¹tkowy punkt linii</param>
        /// <param name="linePoint2">Koñcowy punkt linii</param>
        /// <returns>Zwraca czas, po którym nast¹pi kolizja</returns>
        public static double TimeOfCollisionWithMovingLine(Pos2D objPos, Pos2D objVel, Pos2D linePoint1, Pos2D linePoint2, Pos2D lineVel)
        {
            if (objVel == Pos2D.Zero && lineVel == Pos2D.Zero) return double.PositiveInfinity;
            if (objVel - lineVel == Pos2D.Zero) return double.PositiveInfinity;

            // Ax + By + C = 0
            double A = linePoint1.Y - linePoint2.Y;
            double B = linePoint2.X - linePoint1.X;
            double C = (linePoint1.X * linePoint2.Y) - (linePoint2.X * linePoint1.Y);

            double tc = (-A * objPos.X - B * objPos.Y - C) / (A * (objVel.X - lineVel.X) + B * (objVel.Y - lineVel.Y));
            if (tc < 0) return double.PositiveInfinity;

            // Sprawdzanie czy punkt le¿y na lini ograniczonej punktami
            Pos2D cPos = objPos + objVel * tc;
            Pos2D lPos1 = linePoint1 + lineVel * tc;
            //Pos2D lPos2 = linePoint2 + lineVel * tc;

            // dx = B
            // dy = -A
            Pos2D d = new(B, -A);
            // rx = cp.x - lp1.x
            // ry = cp.y - lp1.y
            Pos2D r = cPos - lPos1;
            // d_p = dx * rx + dy * ry
            double dot_product = d.DotProduct(r);
            // l_s = dx * dx + dy * dy
            double length_squared = d.DotProduct(d);

            if (dot_product < 0 || dot_product > length_squared)
                return double.PositiveInfinity;

            return tc;
        }




        public static double TimeOfCollisionWithStaticCircle(Pos2D objPos, Pos2D objVel, Pos2D ballPos, double radius, double startAngle = 0, double endAngle = 0)
        {
            return TimeOfCollisionWithMovingCircle(objPos, objVel, ballPos, Pos2D.Zero, radius, startAngle, endAngle);
        }

        public static double TimeOfCollisionWithMovingCircle(Pos2D objPos, Pos2D objVel, Pos2D ballPos, Pos2D ballVel, double radius, double startAngle = 0, double endAngle = 0)
        {
            if (radius == 0) return double.PositiveInfinity;

            // Wypadkowe Vel (xv - xov, yv - yov)
            Pos2D wypVel = objVel - ballVel;
            // Wypadkowe Pos (xs - xo, ys - yo)
            Pos2D wypPos = objPos - ballPos;

            // at^2 + bt + c = 0
            double a = wypVel.X * wypVel.X + wypVel.Y * wypVel.Y;
            double b = 2 * (wypPos.X * wypVel.X + wypPos.Y * wypVel.Y);
            double c = wypPos.X * wypPos.X + wypPos.Y * wypPos.Y - radius * radius;

            // delt = b^2 - 4ac
            double delt = b * b - 4 * a * c;
            if (delt <= 0) return double.PositiveInfinity;

            double t1 = (-b - Math.Sqrt(delt)) / (2 * a);
            double t2 = (-b + Math.Sqrt(delt)) / (2 * a);

            if (t1 >= 0 || t2 >= 0)
            {
                // test start i end Angle:
                startAngle %= 360;
                endAngle %= 360;
                if (endAngle < startAngle)
                {
                    (endAngle, startAngle) = (startAngle, endAngle);
                }
                if (endAngle == startAngle)
                {
                    endAngle += 360;
                }

                // Testowanie czy nale¿¹ pozycje kolizji do ³uku rysowanego od startAngle do endAngle
                Pos2D cPos;
                Pos2D bPos;
                double cos;
                double alpha;
                double radians;
                if (t1 >= 0 && t2 >= 0)
                {
                    double tmin = Math.Min(t1, t2);
                    if (startAngle == endAngle) return tmin;

                    // Tesotowanie min(t1, t2)
                    cPos = objPos + objVel * tmin;
                    bPos = ballPos + ballVel * tmin;

                    cos = (cPos.X - bPos.X) / radius;
                    radians = Math.Asin((cPos.Y - bPos.Y) / radius);
                    if (cos < 0) radians += Math.PI;

                    alpha = ((radians * 180) / Math.PI) % 360;
                    if (alpha >= startAngle && alpha <= endAngle) return tmin;

                    // Jeœli pozycja dla tmin nie nale¿y do ³uku
                    double tmax = Math.Max(t1, t2);
                    cPos = objPos + objVel * tmax;
                    bPos = ballPos + ballVel * tmax;

                    cos = (cPos.X + bPos.X) / radius;
                    radians = Math.Asin((cPos.Y - bPos.Y) / radius);
                    if (cos < 0) radians += Math.PI;

                    alpha = ((radians * 180) / Math.PI) % 360;
                    if (alpha >= startAngle && alpha <= endAngle) return tmax;
                }
                else if (t1 >= 0 || t2 >= 0)
                {
                    double tmin = Math.Min(t1, t2);
                    cPos = objPos + objVel * tmin;
                    bPos = ballPos + ballVel * tmin;

                    cos = (cPos.X + bPos.X) / radius;
                    radians = Math.Asin((cPos.Y - bPos.Y) / radius);
                    if (cos < 0) radians += Math.PI;

                    alpha = ((radians * 180) / Math.PI) % 360;
                    if (alpha >= startAngle && alpha <= endAngle) return tmin;
                }
            }

            return double.PositiveInfinity;
        }

        // Calculating Bounce
        public static (Pos2D, Pos2D) VelocitiesAfterCollision(Pos2D vel1, double mass1, Pos2D vel2, double mass2)
        {
            if (mass1 == 0 && mass2 == 0) return (Pos2D.Zero, Pos2D.Zero);

            double massSum = mass1 + mass2;
            Pos2D newVel1 = ((mass1 - mass2) * vel1 + 2 * mass2 * vel2) / massSum;
            Pos2D newVel2 = (2 * mass1 * vel1 + (mass2 - mass1) * vel2) / massSum;

            return (newVel1, newVel2);
        }

        public static (Pos2D, Pos2D) VelocitiesAfterBallsCollision(Pos2D vel1, double mass1, Pos2D center1, Pos2D vel2, double mass2, Pos2D center2)
        {
            if (mass1 == 0 && mass2 == 0) return (Pos2D.Zero, Pos2D.Zero);

            Pos2D centerLine = (center1 - center2);

            // K¹t pomiêdzy osi¹ X a lini¹ ³¹cz¹c¹ œrodki kul
            double alpha = Math.Acos(centerLine.X / centerLine.Length);
            // K¹t pomiêdzy lini¹ ³¹cz¹c¹ œrodki kul a wektorem prêdkoœci pierwszej kuli
            double theta1 = Math.Acos(vel1.X / vel1.Length);
            // K¹t pomiêdzy lini¹ ³¹cz¹c¹ œrodki kul a wektorem prêdkoœci pierwszej kuli
            double theta2 = Math.Acos(vel2.X / vel2.Length);

            double massSum = mass1 + mass2;

            // Nowy wektor prêdkoœci dla 1 cia³a
            double v1FirstHalf = vel1.Length * Math.Cos(theta1 - alpha) * (mass1 - mass2) + 2 * mass2 * vel2.Length * Math.Cos(theta2 - alpha);
            double v1SecondHalf = vel1.Length * Math.Sin(theta1 - alpha);
            double v1x = (v1FirstHalf * Math.Cos(alpha)) / massSum + v1SecondHalf * Math.Cos(alpha + Math.PI / 2);
            double v1y = (v1FirstHalf * Math.Sin(alpha)) / massSum + v1SecondHalf * Math.Sin(alpha + Math.PI / 2);
            Pos2D newVel1 = new(v1x, v1y);

            // Nowy wektor prêdkoœci dla 2 cia³a
            double v2FirstHalf = vel2.Length * Math.Cos(theta2 - alpha) * (mass2 - mass1) + 2 * mass1 * vel1.Length * Math.Cos(theta1 - alpha);
            double v2SecondHalf = vel2.Length * Math.Sin(theta2 - alpha);
            double v2x = (v2FirstHalf * Math.Cos(alpha)) / massSum + v2SecondHalf * Math.Cos(alpha + Math.PI / 2);
            double v2y = (v2FirstHalf * Math.Sin(alpha)) / massSum + v2SecondHalf * Math.Sin(alpha + Math.PI / 2);
            Pos2D newVel2 = new(v2x, v2y);

            return (newVel1, newVel2);
        }
    }
}
