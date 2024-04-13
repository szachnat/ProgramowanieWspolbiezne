using System;
using TPW.Logika;
using TPW.Dane;
using System.Collections.ObjectModel;

namespace TPW.Prezentacja.Model
{
    /// <summary>
    /// Baza klas z ModelApi
    /// </summary>
    public abstract class ModelApiBase : IDisposable
    {
        public abstract ObservableCollection<IModelBall> Balls { get; }
        public abstract double PlaneWidth { get; set; }
        public abstract double PlaneHeight { get; set; }
        public abstract void GenerateBalls(uint ballsNum, double radius, double minVel, double maxVel);
        public abstract void Start();
        public abstract void Stop();
        public abstract void Dispose();

        public static ModelApiBase GetApi(LogikaApiBase ? logika = default)
        {
            return new ModelApi(logika ?? LogikaApiBase.GetApi());
        }
    }

}
