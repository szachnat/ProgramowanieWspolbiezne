using System.Diagnostics;
using System.Threading;
using System;

namespace TPW.Dane
{
    /// <summary>
    /// klasa tworzącqa obiekty kulek
    /// </summary>
    public class Ball : IBall
    {
        #region BallBase

        private readonly long _id;
        private double m_mass;
        private double m_radius;
        private Pos2D m_pos;
        private Pos2D m_vel;

        private readonly object mass_lock = new();
        private readonly object radius_lock = new();
        private readonly object pos_lock = new();
        private readonly object vel_lock = new();

        /// <summary>
        /// Konstruktor kulek
        /// </summary>
        /// <param name="id">ID kulki</param>
        /// <param name="radius">Promień kulki</param>
        /// <param name="pos">Pozycja startowa kulki</param>
        /// <param name="vel">Prędkość kulki</param>
        /// <param name="mass">Masa kulki</param>
        public Ball (long id, double radius, Pos2D pos, Pos2D vel, double mass=10)
        {
            this._id = id;
            this.m_mass = mass;
            this.m_radius = radius;
            this.m_pos = pos;
            this.m_vel = vel;
            this.m_endThread = false;
            this.m_thread = new Thread(new ThreadStart(this.ThreadMethod))
            {
                IsBackground = true
            };
        }

        /// <summary>
        /// Setter masy kulki
        /// </summary>
        /// <param name="mass">Nowa masa</param>
        public void SetMass(double mass)
        {
            lock (this.mass_lock)
            {
                this.m_mass = mass;
            }
        }

        /// <summary>
        /// Setter promienia kulki
        /// </summary>
        /// <param name="radius">Nowy promień</param>
        public void SetRadius(double radius)
        {
            lock (this.radius_lock)
            {
                this.m_radius = radius;
            }
        }

        /// <summary>
        /// Setter pozycji kulki
        /// </summary>
        /// <param name="pos">Nowa pozycja</param>
        public void SetPos(Pos2D pos)
        {
            lock (this.pos_lock)
            {
                this.m_pos = pos;
            }
        }

        /// <summary>
        /// Setter prędkości kulki
        /// </summary>
        /// <param name="pos">Nowa prędkośc</param>
        public void SetVel(Pos2D vel)
        {
            lock (this.vel_lock)
            {
                this.m_vel = vel;
            }
        }

        /// <summary>
        /// Getter ID
        /// </summary>
        /// <returns>Zwraca ID</returns>
        public long GetId()
        {
            return this._id;
        }

        /// <summary>
        /// Getter masy
        /// </summary>
        /// <returns>Zwraca mase</returns>
        public double GetMass()
        {
            lock (this.mass_lock)
            {
                return this.m_mass;
            }
        }

        /// <summary>
        /// Getter promienia
        /// </summary>
        /// <returns>Zwraca promień</returns>
        public double GetRadius()
        {
            lock (this.radius_lock)
            {
                return this.m_radius;
            }
        }

        /// <summary>
        /// Getter pozycji
        /// </summary>
        /// <returns>Zwraca pozycje</returns>
        public Pos2D GetPos()
        {
            lock (this.pos_lock)
            {
                return this.m_pos;
            }
        }

        /// <summary>
        /// Getter prędkości
        /// </summary>
        /// <returns>Zwraca prędkość</returns>
        public Pos2D GetVel()
        {
            lock (this.vel_lock)
            {
                return this.m_vel;
            }
        }

        #endregion Ballbase

        #region INotifyPositionChanged

        //deklaracja zdarzenia
        public event PositionChangeEventHandler? OnPositionChange;

        #endregion INotifyPositionChanged

        #region Thread

        private readonly Thread m_thread;
        private bool m_endThread;

        /// <summary>
        /// Tworzenie wątku
        /// </summary>
        public void StartThread()
        {
            if (this.m_thread.ThreadState != System.Threading.ThreadState.Background) 
            {
                m_thread.Start();
            }
        }

        /// <summary>
        /// Poruszanie piłki (jako wątku)
        /// </summary>
        private void ThreadMethod()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            while(!m_endThread)
            {
                Pos2D previous = this.GetPos();

                TimeSpan elapsed = stopwatch.Elapsed;
                this.SetPos(this.GetPos() + this.GetVel() * elapsed.TotalSeconds);
                OnPositionChange?.Invoke(this, new PositionChangeEventArgs(previous, this.m_vel, elapsed.TotalSeconds));
                stopwatch.Restart();
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// Usuwanie wątku
        /// </summary>
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
            Delegate[] delegates = OnPositionChange?.GetInvocationList();
            if (delegates != null)
            {
                foreach (Delegate d in delegates)
                {
                    OnPositionChange -= (PositionChangeEventHandler)d;
                }
            }

            EndThread();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
