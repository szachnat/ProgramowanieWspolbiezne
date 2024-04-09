using System;
using System.Collections.Generic;
using TPW.Dane;

namespace TPW.Logika
{
    public abstract class LogikaApiBase : IDisposable
    {
        public abstract IEnumerable<IBall> Balls { get; }
        public abstract double PlaneWidth { get; set; }
        public abstract double PlaneHeight { get; set; }

        public abstract void GenerateRandomBalls(uint ballsNum, double radius, double minVel, double maxVel);
        public abstract void StartSimulation();
        public abstract void StopSimulation();

        public abstract void Dispose();

        public static LogikaApiBase GetApi(DaneApiBase? dane = null)
        {
            return new LogikaApi(dane ?? DaneApiBase.GetApi());
        }
    }
}
