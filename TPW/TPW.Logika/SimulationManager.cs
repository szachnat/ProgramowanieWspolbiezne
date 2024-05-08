using System;
using System.Collections.Generic;
using System.ComponentModel;
using TPW.Dane;

namespace TPW.Logika
{
    /// <summary>
    /// Klasa oidpowiedzialna za symulację
    /// </summary>
    public class SimulationManager : IDisposable
    {
        private readonly Plane m_plane;
        private readonly DaneApiBase dane;
        public List<IBall> Balls { get; private set; }

        public double PlaneWidth
        {
            get 
            { 
                return m_plane.GetWidth();
            }
            set
            {
                if (m_plane != null && m_plane.GetWidth() != value) 
                {
                    m_plane.SetWidth(value);
                }
            }
        }
        public double PlaneHeight
        {
            get
            {
                return m_plane.GetHeight();
            }
            set
            {
                if (m_plane != null && m_plane.GetHeight() != value)
                {
                    m_plane.SetHeight(value);
                }
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="plane">Plansza</param>
        /// <param name="dane">Ewentualne DaneApiBase</param>
        public SimulationManager (Plane plane, DaneApiBase? dane = default)
        {
            this.m_plane = plane;
            this.dane = dane ?? DaneApiBase.GetApi();
            this.Balls = new List<IBall>();
        }

        /// <summary>
        /// Funkcja generująca kulki z parametrami z zakresu na planszy
        /// </summary>
        /// <param name="ballsNum">Ilość kulek</param>
        /// <param name="radius">Promień kulek</param>
        /// <param name="minVel">Minimaslna prędkość</param>
        /// <param name="maxVel">Maksymalna prędkość</param>
        public void CreateRandomBalls(uint ballsNum, double radius, double minVel, double maxVel)
        {
            Balls = new List<IBall>();
            Pos2D planePos = new(PlaneWidth, PlaneHeight);
            for (int i = 0; i < ballsNum; i++)
            {
                Ball newBall = this.dane.CreateRandomBall(radius, Pos2D.Zero, new Pos2D(PlaneWidth, PlaneHeight), minVel, maxVel);

                while (IsInBall(newBall))
                {
                    double minX = Pos2D.Zero.X + newBall.GetRadius();
                    double maxX = planePos.X - newBall.GetRadius();

                    double minY = Pos2D.Zero.Y + newBall.GetRadius();
                    double maxY = planePos.Y - newBall.GetRadius();

                    Random rnd = new();
                    newBall.SetPos(new(rnd.NextDouble() * (maxX - minX) + minX, rnd.NextDouble() * (maxY - minY) + minY));
                }

                newBall.OnPositionChange += CheckCollisions;

                Balls.Add(newBall);
            }
        }

        /// <summary>
        /// Rozpoczęcie symulacji kulek na planszy
        /// </summary>
        public void StartSimulation()
        {
            foreach(IBall b in Balls)
            {
                b.StartThread();
            }
        }

        /// <summary>
        /// Zakończenie symulacji kulek na planszy
        /// </summary>
        public void StopSimulation()
        {
            foreach (IBall b in Balls)
            {
                b.EndThread();
            }
        }

        /// <summary>
        /// Zatrzymanie symulacji i usunięcie kulek z planszy
        /// </summary>
        public void ClearBalls()
        {
            StopSimulation();
            foreach (IBall b in Balls)
            {
                b?.Dispose();
            }
            Balls.Clear();
        }

        public void Dispose()
        {
            ClearBalls();
            GC.SuppressFinalize(this);
        }

        private bool IsInBall(IBall ball)
        {
            foreach (IBall b in Balls)
            {
                double x = b.GetPos().X - ball.GetPos().X;
                double y = b.GetPos().Y - ball.GetPos().Y;
                double r = b.GetRadius() + ball.GetRadius();

                if (x * x + y * y < r * r)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckCollisions(object source, PositionChangeEventArgs e)
        {
            IBall ball = (IBall)source;
            Pos2D lastPos = e.LastPos;
            Pos2D currVel = e.Vel;
            double totalTime = e.ElapsedSeconds;
            for (int x = 0; x < 6; x++)
            {
                (lastPos, currVel, totalTime) = CheckBallsCollisions(ball, lastPos, currVel, totalTime);
                (lastPos, currVel, totalTime) = CheckPlaneBordersCollisions(ball, lastPos, currVel, totalTime);
            }
        }

        private (Pos2D, Pos2D, double) CheckBallsCollisions(IBall ball, Pos2D lastPos, Pos2D lastVel, double totalTime)
        {
            // Other Balls List (In Move Dist to current Ball)
            double moveDist = lastVel.Length + ball.GetRadius();
            List<(double, IBall)> nearBalls = new();
            for (int j = 0; j < Balls.Count; j++)
            {
                if (Balls[j] != ball)
                {
                    double dist = Math.Clamp((Balls[j].GetPos() - lastPos).Length - Balls[j].GetRadius(), 0, double.MaxValue);
                    nearBalls.Add((dist, Balls[j]));
                }
            }
            // Sort By Dist
            nearBalls = nearBalls.OrderBy((e1) => e1.Item1).ToList();
            double tc;
            for (int j = 0; j < nearBalls.Count; j++)
            {
                (double dist, IBall b) = nearBalls[j];
                Pos2D lastPos2 = b.GetPos();
                Pos2D vel2 = b.GetVel();
                tc = CollisionManager.TimeOfCollisionWithMovingCircle(lastPos, lastVel, lastPos2, vel2, ball.GetRadius() + b.GetRadius());
                if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                {

                    lastPos += lastVel * tc;
                    ball.SetPos(lastPos);
                    Pos2D newPos2 = lastPos2 + vel2 * tc;
                    b.SetPos(newPos2);
                    //(lastVel, vel2) = CollisionManager.VelocitiesAfterBallsCollision(lastVel, ball.GetMass(), lastPos, vel2, b.GetMass(), newPos2);
                    (lastVel, vel2) = CollisionManager.VelocitiesAfterCollision(lastVel, ball.GetMass(), vel2, b.GetMass());
                    ball.SetVel(lastVel);
                    b.SetVel(vel2);
                    totalTime -= tc;

                    (lastPos, lastVel, totalTime) = CheckPlaneBordersCollisions(ball, lastPos, lastVel, totalTime);
                }
            }

            return (lastPos, lastVel, totalTime);
        }

        private (Pos2D, Pos2D, double) CheckPlaneBordersCollisions(IBall ball, Pos2D lastPos, Pos2D lastVel, double totalTime)
        {
            // Plane Points:
            Pos2D topLeftPlanePoint = new() { X = 0 + ball.GetRadius(), Y = 0 + ball.GetRadius() };
            Pos2D topRightPlanePoint = new() { X = PlaneWidth - ball.GetRadius(), Y = 0 + ball.GetRadius() };
            Pos2D bottomRightPlanePoint = new() { X = PlaneWidth - ball.GetRadius(), Y = PlaneHeight - ball.GetRadius() };
            Pos2D bottomLeftPlanePoint = new() { X = 0 + ball.GetRadius(), Y = PlaneHeight - ball.GetRadius() };

            if (totalTime > 0)
            {
                double tc;
                // Plane Top Line:
                if (lastVel.Y < 0 && lastPos.Y > topLeftPlanePoint.Y)
                {
                    tc = CollisionManager.TimeOfCollisionWithStaticLine(lastPos, lastVel, topLeftPlanePoint, topRightPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos += lastVel * tc;
                        ball.SetPos(lastPos);
                        lastVel.Y *= -1;
                        ball.SetVel(lastVel);
                        totalTime -= tc;
                    }
                }

                // Plane Right Line
                if (lastVel.X > 0 && lastPos.X < topRightPlanePoint.X)
                {
                    tc = CollisionManager.TimeOfCollisionWithStaticLine(lastPos, lastVel, topRightPlanePoint, bottomRightPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos += lastVel * tc;
                        ball.SetPos(lastPos);
                        lastVel.X *= -1;
                        ball.SetVel(lastVel);
                        totalTime -= tc;
                    }
                }

                // Plane Bottom Line:
                if (lastVel.Y > 0 && lastPos.Y < bottomRightPlanePoint.Y)
                {
                    tc = CollisionManager.TimeOfCollisionWithStaticLine(lastPos, lastVel, bottomLeftPlanePoint, bottomRightPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos += lastVel * tc;
                        ball.SetPos(lastPos);
                        lastVel.Y *= -1;
                        ball.SetVel(lastVel);
                        totalTime -= tc;
                    }
                }

                // Plane Left Line
                if (lastVel.X < 0 && lastPos.X > bottomLeftPlanePoint.X)
                {
                    tc = CollisionManager.TimeOfCollisionWithStaticLine(lastPos, lastVel, bottomLeftPlanePoint, topLeftPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos += lastVel * tc;
                        ball.SetPos(lastPos);
                        lastVel.X *= -1;
                        ball.SetVel(lastVel);
                        totalTime -= tc;
                    }
                }

                ball.SetPos(lastPos + lastVel * totalTime);
            }
            return (lastPos, lastVel, totalTime);
        }
    }
}
