using System;
using System.Collections.Generic;
using System.ComponentModel;
using TPW.Dane;

namespace TPW.Logika
{
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

        public SimulationManager (Plane plane, DaneApiBase? dane = default)
        {
            this.m_plane = plane;
            this.dane = dane ?? DaneApiBase.GetApi();
            this.Balls = new List<IBall>();
        }
        
        public void CreateRandomBalls(uint ballsNum, double radius, double minVel, double maxVel)
        {
            Balls = new List<IBall>();
            for (int i = 0; i < ballsNum; i++)
            {
                Ball newBall = this.dane.CreateRandomBall(radius, new Pos2D( 0,0), new Pos2D (PlaneWidth,PlaneHeight), minVel, maxVel);
                newBall.OnPositionChange += CheckCollisions;
                Balls.Add(newBall);
            }
        }

        public void StartSimulation()
        {
            foreach(IBall b in Balls)
            {
                b.StartThread();
            }
        }

        public void StopSimulation()
        {
            foreach (IBall b in Balls)
            {
                b.EndThread();
            }
        }

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

        private (Pos2D, Pos2D) CheckCollisionsRecursion(Pos2D lastPos, Pos2D newPos, Pos2D currVel, double radius, double totalTime, uint maxNum = 3, uint num = 0)
        {
            // Plane Points:
            Pos2D topLeftPlanePoint = new() { X = 0 + radius, Y = 0 + radius };
            Pos2D topRightPlanePoint = new() { X = PlaneWidth - radius, Y = 0 + radius };
            Pos2D bottomRightPlanePoint = new() { X = PlaneWidth - radius, Y = PlaneHeight - radius };
            Pos2D bottomLeftPlanePoint = new() { X = 0 + radius, Y = PlaneHeight - radius };

            if(totalTime > 0)
            {
                double tc;
                // Plane Top Line:
                if (newPos.Y <= topLeftPlanePoint.Y && currVel.Y < 0)
                {
                    tc = CollisionManager.TimeOfCollisionWithLine(lastPos, currVel, topLeftPlanePoint, topRightPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos = currVel * tc + lastPos;
                        currVel.Y *= -1;
                        totalTime -= tc;
                        newPos = currVel * totalTime + lastPos;
                    }
                }

                // Plane Right Line
                if (newPos.X >= topRightPlanePoint.X && currVel.X > 0)
                {
                    tc = CollisionManager.TimeOfCollisionWithLine(lastPos, currVel, topRightPlanePoint, bottomRightPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos = currVel * tc + lastPos;
                        currVel.X *= -1;
                        totalTime -= tc;
                        newPos = currVel * totalTime + lastPos;
                    }
                }

                // Plane Bottom Line:
                if (newPos.Y >= bottomRightPlanePoint.Y && currVel.Y > 0)
                {
                    tc = CollisionManager.TimeOfCollisionWithLine(lastPos, currVel, bottomLeftPlanePoint, bottomRightPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos = currVel * tc + lastPos;
                        currVel.Y *= -1;
                        totalTime -= tc;
                        newPos = currVel * totalTime + lastPos;
                    }
                }

                // Plane Left Line
                if (newPos.X <= bottomLeftPlanePoint.X && currVel.X < 0)
                {
                    tc = CollisionManager.TimeOfCollisionWithLine(lastPos, currVel, bottomLeftPlanePoint, topLeftPlanePoint);
                    if (tc != double.PositiveInfinity && tc >= 0 && tc <= totalTime)
                    {
                        lastPos = currVel * tc + lastPos;
                        currVel.X *= -1;
                        totalTime -= tc;
                        newPos = currVel * totalTime + lastPos;
                    }
                }
            }

            if (++num >= maxNum)
            {
                return (newPos, currVel);
            }
            return CheckCollisionsRecursion(lastPos, newPos, currVel, radius, totalTime, maxNum, num);
        }

        private void CheckCollisions(object source, PositionChangeEventArgs e)
        {
            IBall b =  (IBall)source;
            Pos2D currVel = b.GetVel();
            Pos2D newPos = e.NewPos;
            (newPos, currVel) = CheckCollisionsRecursion(e.LastPos, newPos, currVel, b.GetRadius(), e.ElapsedSeconds);

            if(currVel != b.GetVel())
            {
                b.SetVel(currVel);
            }

            if (newPos != e.NewPos)
            {
                b.SetPos(newPos);
            }
        }
    }
}
