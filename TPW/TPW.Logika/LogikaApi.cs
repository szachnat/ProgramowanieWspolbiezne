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
        public override IEnumerable<IBall> Balls => simManager.Balls;
        public override double PlaneWidth { get => simManager.PlaneWidth; set => simManager.PlaneWidth = value; }
        public override double PlaneHeight { get => simManager.PlaneHeight; set => simManager.PlaneHeight = value; }

        private readonly SimulationManager simManager;
        private readonly DaneApiBase dane;


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
            BallLogger.Log("LogikaApi: Generating Random Balls", LogType.DEBUG);
            simManager.ClearBalls();
            simManager.CreateRandomBalls(ballsNum, radius, minVel, maxVel);
            BallLogger.Log("LogikaApi: Generated Random Balls", LogType.DEBUG);
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
