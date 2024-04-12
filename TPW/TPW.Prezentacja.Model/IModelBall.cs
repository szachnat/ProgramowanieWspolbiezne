using System;
using System.ComponentModel;
using System.Windows.Media;
using TPW.Dane;

namespace TPW.Prezentacja.Model
{
    public interface IModelBall : INotifyPropertyChanged, IDisposable
    {
        double Diameter { get; }
        Pos2D CanvasPos { get; set; }
        Brush ? Color { get; }
    }
}
