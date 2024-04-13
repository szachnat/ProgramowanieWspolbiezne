using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using TPW.Dane;

namespace TPW.Prezentacja.Model
{
    public class ModelBall : IModelBall
    {
        private readonly IBall ball;

        public double Diameter
        {
            get
            {
                return ball.GetRadius() * 2;
            }
        }

        public Pos2D CanvasPos
        {
            get
            {
                return ball.GetPos() - (new Pos2D(1, 1) * ball.GetRadius());
            }
            set
            {
                double radius = ball.GetRadius();
                Pos2D canvasPos = ball.GetPos() - (new Pos2D(1, 1) * radius);
                if (canvasPos.X != value.X || canvasPos.Y != value.Y)
                {
                    ball.SetPos(value + (new Pos2D(1, 1) * radius));
                }
            }
        }

        public Brush? Color { get; }

        public ModelBall(IBall ball, Brush? color)
        {
            this.ball = ball;
            this.ball.OnPositionChange += BallPositionUpdate;
            this.Color = color;
        }

        private void BallPositionUpdate(object sender, PositionChangeEventArgs e)
        {
            OnPropertyChanged(nameof(CanvasPos));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            ball.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
