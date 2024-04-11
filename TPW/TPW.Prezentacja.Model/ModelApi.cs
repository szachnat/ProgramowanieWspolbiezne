using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TPW.Dane;
using TPW.Logika;
namespace TPW.Prezentacja.Model
{
    internal class ModelApi : ModelApiBase
    {
        private ObservableCollection<IModelBall> _balls;
        public override ObservableCollection<IModelBall> Balls => _balls;
        public override double PlaneWidth { get => logika.PlaneWidth; set => logika.PlaneWidth = value; }
        public override double PlaneHeight { get => logika.PlaneHeight; set => logika.PlaneHeight = value; }
        private readonly LogikaApiBase logika;

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
        }
        public override void Stop()
        {
            this.logika.StopSimulation();
        }
        public override void Dispose()
        {
            logika.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
