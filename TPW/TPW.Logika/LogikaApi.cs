using System;
using System.Collections.Generic;
using TPW.Dane;

namespace TPW.Logika
{
    public class LogikaApi : LogikaApiBase
    {
        private readonly SimulationManager simManager;
        private readonly DaneApiBase dane;

        public override IEnumerable<IBall> Balls => simManager.Balls;
        public override double PlaneWidth { get => simManager.PlaneWidth; set => simManager.PlaneWidth = value; }
        public override double PlaneHeight { get => simManager.PlaneHeight; set => simManager.PlaneHeight = value; }

        public LogikaApi (DaneApiBase? dane)
        {
            this.dane = dane ?? DaneApiBase.GetApi();
            this.simManager = new SimulationManager (this.dane.CreatePlane(0,0));
        }

        public override void GenerateRandomBalls(uint ballsNum, double radius, double minVel, double maxVel)
        {
            simManager.ClearBalls();
            simManager.CreateRandomBalls(ballsNum, radius, minVel, maxVel);
        }

        public override void StartSimulation()
        {
            simManager.StartSimulation();
        }

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
