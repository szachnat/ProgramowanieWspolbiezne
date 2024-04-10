using System;
using System.Collections.Generic;
using TPW.Dane;

namespace TPW.Logika
{
    /// <summary>
    /// Api logiki
    /// </summary>
    public class LogikaApi : LogikaApiBase
    {
        private readonly SimulationManager simManager;
        private readonly DaneApiBase dane;

        public override IEnumerable<IBall> Balls => simManager.Balls;
        public override double PlaneWidth { get => simManager.PlaneWidth; set => simManager.PlaneWidth = value; }
        public override double PlaneHeight { get => simManager.PlaneHeight; set => simManager.PlaneHeight = value; }

        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="dane">DaneApiBase</param>
        public LogikaApi (DaneApiBase? dane)
        {
            this.dane = dane ?? DaneApiBase.GetApi();
            this.simManager = new SimulationManager (this.dane.CreatePlane(0,0));
        }

        /// <summary>
        /// Generuje określoną ilość kulek z prędkością z przedziału
        /// </summary>
        /// <param name="ballsNum">Ilość kulek</param>
        /// <param name="radius">Promień kulek</param>
        /// <param name="minVel">Minimalna prędkość kulek</param>
        /// <param name="maxVel">Maksymalna prędkość kulek</param>
        public override void GenerateRandomBalls(uint ballsNum, double radius, double minVel, double maxVel)
        {
            simManager.ClearBalls();
            simManager.CreateRandomBalls(ballsNum, radius, minVel, maxVel);
        }

        /// <summary>
        /// Zaczyna symulację
        /// </summary>
        public override void StartSimulation()
        {
            simManager.StartSimulation();
        }

        /// <summary>
        /// Zatrzymuje symulację
        /// </summary>
        public override void StopSimulation()
        {
            simManager.StopSimulation();
        }

        public override void Dispose()
        {
            simManager.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
