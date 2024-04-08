using System.Diagnostics;
using System.Threading;
using System;

namespace TPW.Dane
{
    public class Ball : IBall
    {
        #region BallBase

        private readonly long _id;
        private double m_radius;
        private Pos2D m_pos;
        private Pos2D m_vel;

        public Ball (long id, double radius, Pos2D pos, Pos2D vel)
        {
            this._id = id;
            this.m_radius = radius;
            this.m_pos = pos;
            this.m_vel = vel;
            this.m_endThread = false;
            this.m_thread = new Thread(new ThreadStart(this.ThreadMethod))
            {
                IsBackground = true
            };
        }

        public void SetRadius(double radius)
        {
            this.m_radius = radius;
        }

        public void SetPos(Pos2D pos)
        {
            this.m_pos = pos;
        }

        public void SetVel(Pos2D vel)
        {
            this.m_vel = vel;
        }

        public long GetId()
        {
            return this._id;
        }

        public double GetRadius()
        {
            return this.m_radius;
        }

        public Pos2D GetPos()
        {
            return this.m_pos;
        }

        public Pos2D GetVel()
        {
            return this.m_vel;
        }

        #endregion Ballbase

        #region INotifyPositionChanged

        public event PositionChangeEventHandler? OnPositionChange;

        #endregion INotifyPositionChanged

        #region Thread

        private readonly Thread m_thread;
        private bool m_endThread;

        public void StartThread()
        {
            if (this.m_thread.ThreadState != System.Threading.ThreadState.Background) 
            {
                m_thread.Start();
            }
        }

        private void ThreadMethod()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            while(!m_endThread)
            {
                Pos2D previous = this.m_pos;

                TimeSpan elapsed = stopwatch.Elapsed;
                stopwatch.Restart();
                Pos2D current = previous + (this.m_vel * elapsed.TotalSeconds);
                this.m_pos = current;

                OnPositionChange?.Invoke(this, new PositionChangeEventArgs(previous, current, elapsed.TotalSeconds));
                Thread.Sleep(10);
            }
        }

        public void EndThread()
        {
            if(this.m_thread.ThreadState == System.Threading.ThreadState.Background)
            {
                this.m_endThread = true;
                this.m_thread?.Join();
            }
        }
        #endregion Thread

        #region IDisposable

        public void Dispose()
        {
            EndThread();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
