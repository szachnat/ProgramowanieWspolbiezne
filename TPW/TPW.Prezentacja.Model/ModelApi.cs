using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TPW.Dane;
using TPW.Logika;
using System.Reactive;
namespace TPW.Prezentacja.Model
{
    /// <summary>
    /// Klasa modelu api
    /// </summary>
    internal class ModelApi : ModelApiBase
    {
        private ObservableCollection<IModelBall> _balls;
        public override ObservableCollection<IModelBall> Balls => _balls;
        /// <summary>
        /// Szerokość planszy
        /// </summary>
        public override double PlaneWidth { get => logika.PlaneWidth; set => logika.PlaneWidth = value; }
        /// <summary>
        /// Wysokość planszy
        /// </summary>
        public override double PlaneHeight { get => logika.PlaneHeight; set => logika.PlaneHeight = value; }
        private readonly LogikaApiBase logika;

        private readonly Subject<Unit> _stopSubject = new Subject<Unit>();

        public ModelApi(LogikaApiBase? logika = default)
        {
            this.logika = logika ?? LogikaApiBase.GetApi();
            this._balls = new ObservableCollection<IModelBall>();
        }

        public override void GenerateBalls(uint ballsNum, double radius, double minVel, double maxVel)
        {
            this.logika.GenerateRandomBalls(ballsNum, radius, minVel, maxVel);
            this._balls = new ObservableCollection<IModelBall>();
            Random rnd = new();
            foreach (IBall ball in this.logika.Balls)
            {
                Brush color = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
                this._balls.Add(new ModelBall(ball, color));
            }
        }

        public override void Start()
        {
            this.logika.StartSimulation();
            BallLogger.StartLogging();
        }
        public override void Stop()
        {
            this.logika.StopSimulation();
            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                BallLogger.StopLogging();
                _stopSubject.OnNext(Unit.Default);
            });
        }
        public override void Dispose()
        {
            logika.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
